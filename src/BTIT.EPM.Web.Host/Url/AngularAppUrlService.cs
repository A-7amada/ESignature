using Abp.MultiTenancy;
using BTIT.EPM.Url;

namespace BTIT.EPM.Web.Url
{
    public class AngularAppUrlService : AppUrlServiceBase
    {
        public override string EmailActivationRoute => "account/confirm-email";

        public override string PasswordResetRoute => "account/reset-password";

        public override string ViewAndSignDocumentRoute => "App/DocumentRequests/ViewAndSignDocument";

        public override string ViewAndSignDocumentRoutePin => "App/DocumentRequests/ViewAndSignDocumentWithPin";

        public AngularAppUrlService(
                IWebUrlService webUrlService,
                ITenantCache tenantCache
            ) : base(
                webUrlService,
                tenantCache
            )
        {

        }
    }
}