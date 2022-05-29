using Microsoft.Extensions.Configuration;

namespace BTIT.EPM.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
