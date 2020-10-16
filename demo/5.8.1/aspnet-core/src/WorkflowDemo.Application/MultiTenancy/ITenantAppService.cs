using Abp.Application.Services;
using WorkflowDemo.MultiTenancy.Dto;

namespace WorkflowDemo.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

