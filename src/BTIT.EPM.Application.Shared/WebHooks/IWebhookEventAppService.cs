using System.Threading.Tasks;
using Abp.Webhooks;

namespace BTIT.EPM.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
