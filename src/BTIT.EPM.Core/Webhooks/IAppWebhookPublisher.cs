using System.Threading.Tasks;
using BTIT.EPM.Authorization.Users;

namespace BTIT.EPM.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
