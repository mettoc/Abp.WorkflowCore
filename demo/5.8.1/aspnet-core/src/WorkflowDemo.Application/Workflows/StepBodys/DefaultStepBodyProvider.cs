using Abp.UI.Inputs;
using Abp.Workflow;

namespace WorkflowDemo.Application.Workflows.StepBodys
{
    public class DefaultStepBodyProvider : AbpStepBodyProvider
    {
        public override void Build(IAbpStepBodyDefinitionContext context)
        {
            var step1 = new AbpWorkflowStepBody();
            step1.Name = "FixedUserAudit";
            step1.DisplayName = "指定用户审核";
            step1.StepBodyType = typeof(GeneralAuditingStepBody);
            step1.Inputs.Add(new WorkflowParam()
            {
                InputType = new SelectUserInputType(),
                Name = "UserId",
                DisplayName = "审核人"
            });
            context.Create(step1);
            var step2 = new AbpWorkflowStepBody();
            step2.Name = "FixedRoleAudit";
            step2.DisplayName = "指定角色审核";
            step2.StepBodyType = typeof(RoleAuditingStepBody);
            step2.Inputs.Add(new WorkflowParam()
            {
                InputType = new SelectRoleInputType(),
                Name = "RoleName",
                DisplayName = "审核角色名"
            });
            context.Create(step2);
        }
    }
}
