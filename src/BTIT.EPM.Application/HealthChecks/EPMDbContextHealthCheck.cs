using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using BTIT.EPM.EntityFrameworkCore;

namespace BTIT.EPM.HealthChecks
{
    public class EPMDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public EPMDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("EPMDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("EPMDbContext could not connect to database"));
        }
    }
}
