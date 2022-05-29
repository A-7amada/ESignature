using System.Threading.Tasks;
using Abp.Application.Services;
using BTIT.EPM.Install.Dto;

namespace BTIT.EPM.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}