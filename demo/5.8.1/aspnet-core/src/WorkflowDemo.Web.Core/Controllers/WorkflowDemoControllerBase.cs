using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace WorkflowDemo.Controllers
{
    public abstract class WorkflowDemoControllerBase: AbpController
    {
        protected WorkflowDemoControllerBase()
        {
            LocalizationSourceName = WorkflowDemoConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
