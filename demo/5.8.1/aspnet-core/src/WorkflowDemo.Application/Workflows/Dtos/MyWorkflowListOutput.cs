using Abp.Application.Services.Dto;
using Abp.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using WorkflowCore.Models;

namespace WorkflowDemo.Application.Workflows.Dtos
{
    public class MyWorkflowListOutput : EntityDto<Guid>
    {
        public string Title { get; set; }
        public int Version { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        public WorkflowStatus Status { get; set; }
        public string CurrentStepName { get; set; }
        public string CurrentStepTitle
        {
            get
            {
                return Nodes.FirstOrDefault(i => i.Key == CurrentStepName)?.Title;
            }
        }
        [JsonIgnore]
        public IEnumerable<WorkflowNode> Nodes { get; set; }

    }
}
