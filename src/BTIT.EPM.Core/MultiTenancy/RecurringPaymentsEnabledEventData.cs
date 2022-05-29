using Abp.Events.Bus;

namespace BTIT.EPM.MultiTenancy
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}