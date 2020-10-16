using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Json;
using Abp.Linq.Extensions;
using WorkflowDemo.Application.Workflows.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Workflow;
using Abp.WorkflowCore.Persistence;
using Abp.WorkflowCore;
using Abp.Application.Services;
using Abp.Extensions;
using Abp.Workflow.Persistence;

namespace WorkflowDemo.Application.Workflows.StepBodys
{

    [AbpAuthorize]
    public class WorkflowAppService : AsyncCrudAppService<PersistedWorkflowDefinition, WorkflowDesignInfo, string, WorkflowListInput, WorkflowDesignInfo, WorkflowDesignInfo>
    {

        private readonly AbpWorkflowManager _abpWorkflowManager;

        private IGuidGenerator _guidGenerator;
        private readonly IRepository<PersistedWorkflow, Guid> _workflowRepository;

        public WorkflowAppService(IRepository<PersistedWorkflowDefinition, string> repository, IRepository<PersistedWorkflow, Guid> workflowRepository, AbpWorkflowManager abpWorkflowManager, IGuidGenerator guidGenerator) : base(repository)
        {
            _abpWorkflowManager = abpWorkflowManager;
            _guidGenerator = guidGenerator;
            _workflowRepository = workflowRepository;
        }
        /// <summary>
        /// 获取所有分组
        /// </summary>
        /// <returns></returns>

        public async Task<IEnumerable<string>> GetAllGroupAsync()
        {
            var data = await _abpWorkflowManager.AsyncQueryableExecuter.ToListAsync(_abpWorkflowManager.WorkflowDefinitions.GroupBy(u => u.Group).Select(u => u.Key));
            return data.Where(u => u != null || u != "");
        }


        /// <summary>
        /// 我发起的流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<MyWorkflowListOutput>> GetMyWorkflowAsync(WorkflowListInput input)
        {
            var query = _workflowRepository.GetAll().Where(u => u.CreatorUserId == AbpSession.UserId);

            query = query.WhereIf(input.Title.IsNotEmpty(), u => u.WorkflowDefinition.Title.Contains(input.Title));
            return await query.OrderByDescending(u => u.CreationTime).ToPagedResultAsync(input, u => new MyWorkflowListOutput()
            {
                Title = u.WorkflowDefinition.Title,
                Version = u.Version,
                Id = u.Id,
                Status = u.Status,
                CompleteTime = u.CompleteTime,
                CreationTime = u.CreationTime,
                CurrentStepName = u.ExecutionPointers.OrderBy(i => i.StepId).Last().StepName,
                Nodes = u.WorkflowDefinition.Nodes
            });
        }




        public IEnumerable<AbpWorkflowStepBody> GetAllStepBodys()
        {
            return _abpWorkflowManager.GetAllStepBodys();
        }

        protected override IQueryable<PersistedWorkflowDefinition> CreateFilteredQuery(WorkflowListInput input)
        {
            var query = base.CreateFilteredQuery(input);
            query = query.WhereIf(input.Title.IsNotEmpty(), u => u.Title.Contains(input.Title));
            return query;
        }

        /// <summary>
        /// 获取所有分组的流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, IEnumerable<WorkflowDesignInfo>>> GetAllWithGroupAsync(WorkflowListInput input)
        {
            var list = await _abpWorkflowManager.AsyncQueryableExecuter.ToListAsync(this.CreateFilteredQuery(input).Select(u => u));
            return list.GroupBy(u => u.Group).OrderBy(i => i.Key).ToDictionary(u => u.Key, u => u.Select(i => MapToEntityDto(i)));
        }


        public override Task<PagedResultDto<WorkflowDesignInfo>> GetAllAsync(WorkflowListInput input)
        {
            return base.GetAllAsync(input);
        }

        public override Task<WorkflowDesignInfo> GetAsync(EntityDto<string> input)
        {
            return base.GetAsync(input);
        }

        public override async Task<WorkflowDesignInfo> CreateAsync(WorkflowDesignInfo input)
        {
            var entity = await ApplyCreate(input);
            entity.Id = _guidGenerator.Create().ToString();
            await _abpWorkflowManager.CreateAsync(entity);
            return null;
        }
        public async override Task<WorkflowDesignInfo> UpdateAsync(WorkflowDesignInfo input)
        {
            var entity = await ApplyUpdate(input);
            await _abpWorkflowManager.UpdateAsync(entity);
            return null;
        }
        

        /// <summary>
        /// 启动工作流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task StartAsync(StartWorkflowInput input)
        {
            await _abpWorkflowManager.StartWorlflow(input.Id, input.Version, input.Inputs);
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input">待审核ID</param>
        /// <returns></returns>
        public async Task<WorkflowDto> GetDetailsAsync(EntityDto<Guid> input)
        {
            var temp = await _abpWorkflowManager.AsyncQueryableExecuter.FirstOrDefaultAsync(_workflowRepository.GetAll().Where(u => u.Id == input.Id).Select(u => new
            {
                u.Id,
                u.Version,
                u.WorkflowDefinitionId,
                u.WorkflowDefinition.Title,
                u.CreateUserIdentityName,
                u.CreationTime,
                u.WorkflowDefinition.Inputs,
                u.Data,
                u.CompleteTime,
                u.Status,
                u.WorkflowDefinition.Nodes,
                ExecutionRecords = u.ExecutionPointers.OrderBy(i => i.StartTime).Select(i => new WorkflowExecutionRecord()
                {
                    ExecutionPointerId = i.Id,
                    EndTime = i.EndTime,
                    StartTime = i.StartTime,
                    StepId = i.StepId,
                    StepName = i.StepName
                })
            }));
            WorkflowDto result = new WorkflowDto()
            {
                Id = temp.Id,
                Data = temp.Data.FromJsonString<Dictionary<string, object>>(),

                CompleteTime = temp.CompleteTime,
                Status = temp.Status,
                Title = temp.Title,
                CreationTime = temp.CreationTime,
                UserName = temp.CreateUserIdentityName,
                Version = temp.Version,
                WorkflowDefinitionId = temp.WorkflowDefinitionId,
                Inputs = temp.Inputs,
                ExecutionRecords = temp.ExecutionRecords
            };
            foreach (var item in result.ExecutionRecords)
            {
                item.StepTitle = temp.Nodes.FirstOrDefault(i => i.Key == item.StepName)?.Title;
            }
            return result;
        }



        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="input"></param>
        public async Task PublicEvent(PublishEventInput input)
        {
            await _abpWorkflowManager.PublishEventAsync(input.EventName, input.EventKey, input.EventData);
        }

        protected virtual Task<PersistedWorkflowDefinition> ApplyUpdate(WorkflowDesignInfo input)
        {
            var entity = _abpWorkflowManager.WorkflowDefinitions.Where(u => u.Id == input.Id && u.Version == input.Version).FirstOrDefault();
            entity = ObjectMapper.Map(input, entity);

            return Task.FromResult(entity);
        }

        protected virtual  Task<PersistedWorkflowDefinition> ApplyCreate(WorkflowDesignInfo input)
        {
            return Task.FromResult(MapToEntity(input));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task DeleteAsync(EntityDto<string> input)
        {
            await _abpWorkflowManager.DeleteAsync(input.Id);
        }



    }

}
