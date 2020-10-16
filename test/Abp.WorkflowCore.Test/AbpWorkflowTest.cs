using Abp.TestBase;
using Shouldly;
using System;
using WorkflowCore.Interface;
using Xunit;

namespace Abp.WorkflowCore.Test
{
    public class AbpWorkflowTest : AbpIntegratedTestBase<AbpWorkflowCoreTestModule>
    {
        protected readonly IWorkflowRegistry _registry;

        public AbpWorkflowTest(IWorkflowRegistry registry)
        {
            _registry = registry;
        }

        [Fact]
        public void TestAbpWorkflowIsRegistiedAuto()
        {
            var ret = _registry.IsRegistered("HelloAbpWorkflow", 1);
            ret.ShouldBeTrue();

        }
    }
}
