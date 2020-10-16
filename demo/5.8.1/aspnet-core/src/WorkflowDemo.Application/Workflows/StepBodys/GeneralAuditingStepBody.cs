
using Abp;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Notifications;
using Abp.WorkflowCore;
using System;
using System.Linq;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowDemo.Authorization.Users;
using WorkflowDemo.Core.Workflows;
using WorkflowDemo.Core.Workflows.Entities;

namespace WorkflowDemo.Application.Workflows.StepBodys
{
    /// <summary>
    /// 通用审批
    /// </summary>
    public class GeneralAuditingStepBody : StepBody, ITransientDependency
    {

        private const string ActionName = "AuditEvent";
        protected readonly INotificationPublisher _notificationPublisher;
        protected readonly IAbpPersistenceProvider _abpPersistenceProvider;
        protected readonly UserManager _userManager;

        public readonly IRepository<PersistedWorkflowAuditor, Guid> _auditorRepository;

        public GeneralAuditingStepBody(INotificationPublisher notificationPublisher, UserManager userManager, IAbpPersistenceProvider abpPersistenceProvider,
            IRepository<PersistedWorkflowAuditor, Guid> auditorRepository)
        {
            _notificationPublisher = notificationPublisher;
            _abpPersistenceProvider = abpPersistenceProvider;
            _userManager = userManager;
            _auditorRepository = auditorRepository;
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public long UserId { get; set; }

        [UnitOfWork]
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            if (!context.ExecutionPointer.EventPublished)
            {
                var workflow = _abpPersistenceProvider.GetPersistedWorkflow(new Guid(context.Workflow.Id)).Result;
                var workflowDefinition = _abpPersistenceProvider.GetPersistedWorkflowDefinition(context.Workflow.WorkflowDefinitionId, context.Workflow.Version).Result;

                var userIdentityName = _userManager.Users.Where(u => u.Id == workflow.CreatorUserId).Select(u => u.FullName).FirstOrDefault();

                //通知审批人
                var notificationData = new NotificationData();
                notificationData.Properties["content"] = $"【{userIdentityName}】提交的{workflowDefinition.Title}需要您审批！";
                _notificationPublisher.PublishAsync("Task", notificationData,
                    userIds: new UserIdentifier[] { new UserIdentifier(workflow.TenantId, UserId) },
                     entityIdentifier: new EntityIdentifier(workflow.GetType(), workflow.Id)
                    ).Wait();
                //添加审核人记录
                var auditUserInfo = _userManager.GetUserById(UserId);
                _auditorRepository.Insert(new PersistedWorkflowAuditor() { WorkflowId = workflow.Id, ExecutionPointerId = context.ExecutionPointer.Id, Status = EnumAuditStatus.UnAudited, UserId = UserId, TenantId = workflow.TenantId, UserIdentityName = auditUserInfo.FullName });
                DateTime effectiveDate = DateTime.MinValue;
                return ExecutionResult.WaitForEvent(ActionName, Guid.NewGuid().ToString(), effectiveDate);
            }
            var pass = _auditorRepository.GetAll().Any(u => u.ExecutionPointerId == context.ExecutionPointer.Id && u.UserId == UserId && u.Status == EnumAuditStatus.Pass);

            if (!pass)
            {
                context.Workflow.Status = WorkflowStatus.Complete;
                return ExecutionResult.Next();
            }
            return ExecutionResult.Next();
        }
    }
}
