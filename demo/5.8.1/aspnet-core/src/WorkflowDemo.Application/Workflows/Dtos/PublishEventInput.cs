using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowDemo.Application.Workflows.StepBodys
{
    public class PublishEventInput
    {
        public string  EventKey { get; set; }

        public string  EventName { get; set; }

        public object EventData { get; set; }
    }
}
