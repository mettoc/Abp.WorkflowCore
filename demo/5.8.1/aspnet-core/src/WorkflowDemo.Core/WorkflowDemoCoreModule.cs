using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.WorkflowCore;
using Abp.WorkflowCore.Persistence.EntityFrameworkCore;
using Abp.Zero;
using Abp.Zero.Configuration;
using WorkflowDemo.Authorization.Roles;
using WorkflowDemo.Authorization.Users;
using WorkflowDemo.Configuration;
using WorkflowDemo.Localization;
using WorkflowDemo.MultiTenancy;
using WorkflowDemo.Timing;

namespace WorkflowDemo
{
    [DependsOn(typeof(AbpZeroCoreModule),typeof(AbpWorkflowCoreEFCoreModule))]
    public class WorkflowDemoCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            WorkflowDemoLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = WorkflowDemoConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WorkflowDemoCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
