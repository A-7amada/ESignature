using System.Threading.Tasks;
using Abp.Application.Services;

namespace BTIT.EPM.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
