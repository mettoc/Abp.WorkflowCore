using Abp.Modules;
using Abp.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.Workflow
{
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpWorkflowModule : AbpModule
    {
        public override void Initialize()
        {
          
            IocManager.RegisterAssemblyByConvention(typeof(AbpWorkflowModule).GetAssembly());
          
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            IocManager.Resolve<WorkflowDefinitionManager>().Initialize();
        }


    }
}
