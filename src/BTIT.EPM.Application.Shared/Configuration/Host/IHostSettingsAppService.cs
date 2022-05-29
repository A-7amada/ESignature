using System.Threading.Tasks;
using Abp.Application.Services;
using BTIT.EPM.Configuration.Host.Dto;

namespace BTIT.EPM.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
