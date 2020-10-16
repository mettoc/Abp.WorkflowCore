using System;
using System.Reflection;

namespace Abp.Workflow
{
    public class AbpWorkflowStepBody
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public WorkflowParamDictionary Inputs { get; set; } = new WorkflowParamDictionary();

        public Type StepBodyType { get; set; }
    }
}
