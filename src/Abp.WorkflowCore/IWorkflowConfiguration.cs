using Abp.Collections;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;

namespace Abp.WorkflowCore
{


    public interface IWorkflowConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        ITypeList<AbpWorkflowProvider> Providers { get; }
       
    }


    internal class WorkflowConfiguration : IWorkflowConfiguration,ISingletonDependency
    {
        public ITypeList<AbpWorkflowProvider> Providers { get; }

      

        public WorkflowConfiguration()
        {
            Providers = new TypeList<AbpWorkflowProvider>();
        
        }
    }
}
