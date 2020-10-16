using Abp;
using Abp.Collections.Extensions;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.Workflow
{
    public interface IAbpStepBodyDefinitionContext
    {
        void Create(AbpWorkflowStepBody entity);
    }

    public abstract class AbpStepBodyDefinitionContextBase : IAbpStepBodyDefinitionContext
    {
        protected readonly Dictionary<string, AbpWorkflowStepBody> AbpStepBodys;
        public AbpStepBodyDefinitionContextBase()
        {

            AbpStepBodys = new Dictionary<string, AbpWorkflowStepBody>();
        }
        public void Create(AbpWorkflowStepBody entity)
        {
            if (AbpStepBodys.ContainsKey(entity.Name))
            {
                throw new AbpException("There is already a AbpStepBody with name: " + entity.Name);
            }
            AbpStepBodys[entity.Name] = entity;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<AbpWorkflowStepBody> GetAllStepBodys()
        {
            return AbpStepBodys.Values;
        }
        public AbpWorkflowStepBody GetStepBodyOrNull(string name)
        {
            return AbpStepBodys.GetOrDefault(name);
        }

        public void RemoveStepBody(string name)
        {
            AbpStepBodys.Remove(name);
        }
    }
}
