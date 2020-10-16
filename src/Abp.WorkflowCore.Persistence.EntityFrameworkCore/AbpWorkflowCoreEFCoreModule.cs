using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Models;

namespace Abp.WorkflowCore.Persistence.EntityFrameworkCore
{
    [DependsOn(typeof(AbpWorkflowCoreModule))]
    public class AbpWorkflowCoreEFCoreModule : AbpModule
    {
        public override void Initialize()
        {
          
            base.Initialize();
            IocManager.RegisterAssemblyByConvention(typeof(AbpWorkflowCoreEFCoreModule).Assembly);
        }
    }
}
