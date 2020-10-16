using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Workflow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.Modules
{
    public static class AbpStartupConfigurationExtension
    {
        /// <summary>
        /// 获取码表类型配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IAbpStepBodyConfiguration GetWorkflowConfiguration(this IAbpStartupConfiguration config)
        {
            if (!config.IocManager.IsRegistered<IAbpStepBodyConfiguration>())
            {
                config.IocManager.Register<IAbpStepBodyConfiguration, AbpStepBodyConfiguration>(DependencyLifeStyle.Singleton);
            }
            return config.IocManager.Resolve<IAbpStepBodyConfiguration>();
        }
    }
}
