using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.Workflow
{
    public interface IAbpWorkflowRegistry
    {
        void RegisterWorkflow(Type type);
    }
}
