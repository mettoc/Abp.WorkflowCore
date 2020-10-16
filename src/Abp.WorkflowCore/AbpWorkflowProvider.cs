using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;

namespace Abp.WorkflowCore
{
    public abstract class AbpWorkflowProvider : ITransientDependency
    {
        /// <summary>
        /// 设置码表类型
        /// </summary>
        /// <param name="context"></param>
        public abstract void Builds(IWorkflowBuilder context);
    }
}
