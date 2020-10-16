using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowDemo.Application.Workflows.StepBodys
{
    public class StartWorkflowInput
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public Dictionary<string, object> Inputs { get; set; } = new Dictionary<string, object>();
    }
}
