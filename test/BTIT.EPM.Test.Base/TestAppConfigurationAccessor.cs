using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using BTIT.EPM.Configuration;

namespace BTIT.EPM.Test.Base
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(EPMTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}
