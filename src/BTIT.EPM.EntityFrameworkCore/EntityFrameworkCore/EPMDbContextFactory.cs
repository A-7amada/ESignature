using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using BTIT.EPM.Configuration;
using BTIT.EPM.Web;

namespace BTIT.EPM.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class EPMDbContextFactory : IDesignTimeDbContextFactory<EPMDbContext>
    {
        public EPMDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<EPMDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            EPMDbContextConfigurer.Configure(builder, configuration.GetConnectionString(EPMConsts.ConnectionStringName));

            return new EPMDbContext(builder.Options);
        }
    }
}