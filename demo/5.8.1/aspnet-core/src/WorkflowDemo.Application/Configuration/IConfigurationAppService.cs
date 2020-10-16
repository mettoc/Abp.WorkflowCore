using System.Threading.Tasks;
using WorkflowDemo.Configuration.Dto;

namespace WorkflowDemo.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
