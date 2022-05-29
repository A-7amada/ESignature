using Abp.MultiTenancy;
using BTIT.EPM.Url;

namespace BTIT.EPM.Web.Url
{
    public class MvcAppUrlService : AppUrlServiceBase
    {
        public override string EmailActivationRoute => "Account/EmailConfirmation";

        public override string PasswordResetRoute => "Account/ResetPassword";

        public override string ViewAndSignDocumentRoute => "App/DocumentRequests/ViewAndSignDocument";

        public override string ViewAndSignDocumentRoutePin => "App/DocumentRequests/ViewAndSignDocumentWithPin";

        public MvcAppUrlService(
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