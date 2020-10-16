using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using WorkflowDemo.Configuration;

namespace WorkflowDemo.Web.Host.Startup
{
    [DependsOn(
       typeof(WorkflowDemoWebCoreModule))]
    public class WorkflowDemoWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public WorkflowDemoWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WorkflowDemoWebHostModule).GetAssembly());
        }
    }
}
