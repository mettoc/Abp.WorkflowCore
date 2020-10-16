using System.Threading.Tasks;
using Abp.Application.Services;
using WorkflowDemo.Authorization.Accounts.Dto;

namespace WorkflowDemo.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
