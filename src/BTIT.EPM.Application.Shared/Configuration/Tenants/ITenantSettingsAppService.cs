using System.Threading.Tasks;
using Abp.Application.Services;
using BTIT.EPM.Configuration.Tenants.Dto;

namespace BTIT.EPM.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
