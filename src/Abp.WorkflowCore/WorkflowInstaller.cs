using Abp.Dependency;
using Abp.Workflow;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Reflection;

namespace Abp.WorkflowCore
{
    internal class WorkflowInstaller : IWindsorInstaller
    {
        private readonly IIocResolver _iocResolver;

        private IAbpWorkflowRegistry serviceSelector;

        public WorkflowInstaller(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            serviceSelector = container.Resolve<IAbpWorkflowRegistry>();
            container.Kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }

        private void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            /* This code checks if registering component implements any IEventHandler<TEventData> interface, if yes,
             * gets all event handler interfaces and registers type to Event Bus for each handling event.
             */
            if (!typeof(IAbpWorkflow).GetTypeInfo().IsAssignableFrom(handler.ComponentModel.Implementation))
            {
                return;
            }

            var interfaces = handler.ComponentModel.Implementation.GetTypeInfo().GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (!typeof(IAbpWorkflow).GetTypeInfo().IsAssignableFrom(@interface))
                {
                    continue;
                }
                serviceSelector.RegisterWorkflow( handler.ComponentModel.Implementation);
            }
        }
    }
}
