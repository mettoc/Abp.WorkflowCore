using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using WorkflowDemo.Application.Workflows.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using Abp.Application.Services;
using WorkflowDemo.Core.Workflows.Entities;
using Abp.WorkflowCore.Persistence;
using Abp.WorkflowCore;
using WorkflowDemo.Core.Workflows;
using Abp.Extensions;
using WorkflowDemo.Authorization.Users;
using Abp.Workflow.Persistence;

namespace WorkflowDemo.Application.Workflows
{

    [AbpAuthorize]
    public class WorkflowAuditAppService : ApplicationService
    {
        private IRepository<PersistedWorkflowAuditor, Guid> _auditorRepos;
        private IRepository<PersistedWorkflow, Guid> _workflowRepos;
        private readonly AbpWorkflowManager _abpWorkflowManager;
        private readonly UserManager _userManager;
        public WorkflowAuditAppService(IRepository<PersistedWorkflowAuditor, Guid> auditorRepos, AbpWorkflowManager abpWorkflowManager, IRepository<PersistedWorkflow, Guid> workflowRepos, UserManager userManager)
        {
            _auditorRepos = auditorRepos;
            _abpWorkflowManager = abpWorkflowManager;
            _workflowRepos = workflowRepos;
            _userManager = userManager;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<GetMyAuditPageListOutput>> GetAllAsync(GetMyAuditPageListInput input)
        {
            var result = await _auditorRepos.GetAll().Where(u => u.UserId == AbpSession.UserId)
                .WhereIf(input.AuditedMark.HasValue && input.AuditedMark.Value, u => u.Status != EnumAuditStatus.UnAudited)
                .WhereIf(input.AuditedMark.HasValue && !input.AuditedMark.Value, u => u.Status == EnumAuditStatus.UnAudited)
                .OrderByDescending(u => u.CreationTime).ToPagedResultAsync(input, u => new GetMyAuditPageListOutput()
                {
                    Id = u.Id,
                    ExecutionPointerId = u.ExecutionPointerId,
                    CreationTime = u.Workflow.CreationTime,
                    Title = u.Workflow.WorkflowDefinition.Title,
                    UserName = u.Workflow.CreateUserIdentityName,
                    WorkflowDefinitionId = u.Workflow.WorkflowDefinitionId,
                    Version = u.Workflow.WorkflowDefinition.Version,
                    WorkflowId = u.WorkflowId,
                    Status = u.Status,
                    AuditTime = u.AuditTime
                });
            return result;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input">流程ID</param>
        /// <returns></returns>
        public async Task<WorkflowAuditDto> GetAuditRecordsAsync(EntityDto<Guid> input)
        {

            var query = _auditorRepos.GetAll().Where(u => u.WorkflowId == input.Id).Select(u => new
            {
                u.ExecutionPointerId,
                AuditTime = u.AuditTime,
                Status = u.Status,
                Remark = u.Remark,
                UserIdentityName = u.UserIdentityName,
                UserHeadPhoto = u.UserHeadPhoto,
                u.UserId,
            });
            var data = await _abpWorkflowManager.AsyncQueryableExecuter.ToListAsync(query);
            //审核记录
            var resords = data.GroupBy(i => i.ExecutionPointerId).Select(u => new
            {
                ExecutionPointerId = u.Key,
                AuditRecords = u.Select(i => new WorkflowAuditRecord()
                {
                    UserId = i.UserId,
                    AuditTime = i.AuditTime,
                    Status = i.Status,
                    Remark = i.Remark,
                    UserIdentityName = i.UserIdentityName,
                    UserHeadPhoto = i.UserHeadPhoto,
                })
            })
             .ToDictionary(i => i.ExecutionPointerId, i => i.AuditRecords);

            return new WorkflowAuditDto() { NeedAudit = data.Any(i => i.UserId == AbpSession.UserId && i.Status == EnumAuditStatus.UnAudited), AuditRecords = resords };
        }


        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task AuditAsync(WorkflowAuditInput input)
        {
            var entity = await _auditorRepos.FirstOrDefaultAsync(u => u.ExecutionPointerId == input.ExecutionPointerId && u.UserId == AbpSession.UserId);
            if (entity == null)
            {
                throw new UserFriendlyException("不需要您审核");
            }

            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            entity.Status = input.Pass ? EnumAuditStatus.Pass : EnumAuditStatus.Unapprove;
            entity.Remark = input.Remark;
            entity.AuditTime = DateTime.Now;
            entity.UserIdentityName = user.FullName;
            entity.TenantId = AbpSession.TenantId;
            await CurrentUnitOfWork.SaveChangesAsync();

            //终止流程
            if (!input.Pass)
            {
                await _abpWorkflowManager.TerminateWorkflow(entity.WorkflowId.ToString());
                return;
            }
            if (!_auditorRepos.GetAll().Any(u => u.ExecutionPointerId == input.ExecutionPointerId && u.Status == EnumAuditStatus.UnAudited))
            {
                var pointer = await _abpWorkflowManager.PersistenceProvider.GetPersistedExecutionPointer(input.ExecutionPointerId);
                await _abpWorkflowManager.PublishEventAsync(pointer.EventName, pointer.EventKey, null);
            }
        }
    }
}
