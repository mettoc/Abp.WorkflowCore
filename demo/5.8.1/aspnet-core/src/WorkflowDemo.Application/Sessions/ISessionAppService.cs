using System.Threading.Tasks;
using Abp.Application.Services;
using WorkflowDemo.Sessions.Dto;

namespace WorkflowDemo.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
