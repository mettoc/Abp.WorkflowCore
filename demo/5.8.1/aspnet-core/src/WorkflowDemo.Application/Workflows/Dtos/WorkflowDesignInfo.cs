using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Workflow;
using System.Collections.Generic;

namespace WorkflowDemo.Application.Workflows
{
    [AutoMapFrom(typeof(PersistedWorkflowDefinition))]
    [AutoMapTo(typeof(PersistedWorkflowDefinition))]
    public class WorkflowDesignInfo : EntityDto<string>
    {
        public string Color { get; set; }
        public string Group { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }
        public IEnumerable<IEnumerable<IEnumerable<WorkflowFormData>>> Inputs { get; set; }
        public IEnumerable<WorkflowNode> Nodes { get; set; }
        public WorkflowDesignInfo()
        {
            Version = 1;
        }
    }
}
