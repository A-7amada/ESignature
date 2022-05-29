using Abp.Dependency;
using Abp.Extensions;
using Abp.MultiTenancy;
using BTIT.EPM.Url;

namespace BTIT.EPM.Web.Url
{
    public abstract class AppUrlServiceBase : IAppUrlService, ITransientDependency
    {
        public abstract string EmailActivationRoute { get; }

        public abstract string PasswordResetRoute { get; }

        public abstract string ViewAndSignDocumentRoute { get; }

        public abstract string ViewAndSignDocumentRoutePin { get; }

        protected readonly IWebUrlService WebUrlService;
        protected readonly ITenantCache TenantCache;

        protected AppUrlServiceBase(IWebUrlService webUrlService, ITenantCache tenantCache)
        {
            WebUrlService = webUrlService;
            TenantCache = tenantCache;
        }

        public string CreateEmailActivationUrlFormat(int? tenantId)
        {
            return CreateEmailActivationUrlFormat(GetTenancyName(tenantId));
        }

        public string CreatePasswordResetUrlFormat(int? tenantId)
        {
            return CreatePasswordResetUrlFormat(GetTenancyName(tenantId));
        }

        public string CreateEmailActivationUrlFormat(string tenancyName)
        {
            var activationLink = WebUrlService.GetSiteRootAddress(tenancyName).EnsureEndsWith('/') + EmailActivationRoute + "?userId={userId}&confirmationCode={confirmationCode}";

            if (tenancyName != null)
            {
                activationLink = activationLink + "&tenantId={tenantId}";
            }

            return activationLink;
        }

       

        public string CreatePasswordResetUrlFormat(string tenancyName)
        {
            var resetLink = WebUrlService.GetSiteRootAddress(tenancyName).EnsureEndsWith('/') + PasswordResetRoute + "?userId={userId}&resetCode={resetCode}";

            if (tenancyName != null)
            {
                resetLink = resetLink + "&tenantId={tenantId}";
            }

            return resetLink;
        }

        public string CreateViewAndSignDocumentAsyncUrlFormat( bool hasPin, int? tenantId)
        {
            return CreateViewAndSignDocumentAsyncUrlFormat(hasPin, GetTenancyName(tenantId));
        }

        public string CreateViewAndSignDocumentAsyncUrlFormat(bool hasPin, string tenancyName)
        {
            var activationLink = WebUrlService.GetSiteRootAddress(tenancyName).EnsureEndsWith('/') + (hasPin ? ViewAndSignDocumentRoutePin : ViewAndSignDocumentRoute) + "?recipientId={recipientId}&documentRequestId={documentRequestId}&recipientCode={recipientCode}";

            if (tenancyName != null)
            {
                activationLink = activationLink + "&tenantId={tenantId}";
            }

            return activationLink;
        }


        private string GetTenancyName(int? tenantId)
        {
            return tenantId.HasValue ? TenantCache.Get(tenantId.Value).TenancyName : null;
        }
    }
}