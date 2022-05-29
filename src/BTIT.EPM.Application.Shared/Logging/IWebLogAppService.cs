using Abp.Application.Services;
using BTIT.EPM.Dto;
using BTIT.EPM.Logging.Dto;

namespace BTIT.EPM.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
