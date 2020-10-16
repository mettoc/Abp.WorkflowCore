using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using WorkflowDemo.Configuration.Dto;

namespace WorkflowDemo.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : WorkflowDemoAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
