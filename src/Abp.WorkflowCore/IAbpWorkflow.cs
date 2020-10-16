using Abp.Workflow;

using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;

namespace Abp.WorkflowCore
{
    public interface IAbpWorkflow : IWorkflow<WorkflowParamDictionary>
    {
    }
}
