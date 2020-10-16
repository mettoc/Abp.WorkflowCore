using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Workflow;
using System;
using WorkflowCore.Interface;

namespace Abp.WorkflowCore
{
    [DependsOn(typeof(AbpWorkflowModule))]
    public class AbpWorkflowCoreModule : AbpModule
    {
        public override void Initialize()
        {
            base.Initialize();
            IocManager.RegisterAssemblyByConvention(typeof(AbpWorkflowCoreModule).GetAssembly());
            IocManager.IocContainer.Install(new WorkflowInstaller(IocManager));
           
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            var host = IocManager.Resolve<IWorkflowHost>();

            host.Start();
            IocManager.Resolve<AbpWorkflowManager>().Initialize();
        }
        public override void Shutdown()
        {
            base.Shutdown();
            IocManager.Resolve<IWorkflowHost>().Stop();

        }

    }
}
