using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.Workflow
{
    public class WorkflowDefinitionManager : AbpStepBodyDefinitionContextBase, ISingletonDependency
    {
        private readonly IAbpStepBodyConfiguration _baseCodeTypeConfiguration;
        private readonly IIocManager _iocManager;

        public WorkflowDefinitionManager(IAbpStepBodyConfiguration baseCodeTypeConfiguration, IIocManager iocManager)
        {
            _baseCodeTypeConfiguration = baseCodeTypeConfiguration;
            _iocManager = iocManager;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        internal void Initialize()
        {
            foreach (var providerType in _baseCodeTypeConfiguration.Providers)
            {
                using (var provider = _iocManager.ResolveAsDisposable<AbpStepBodyProvider>(providerType))
                {
                    provider.Object.Build(this);
                }
            }
        }
    }
}
