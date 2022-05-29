using System;
using System.Web;
using Abp.Runtime.Security;
using Abp.Runtime.Validation;
using BTIT.EPM.Authorization.Accounts.Dto;
using BTIT.EPM.DigitalSignature.Dtos;

namespace BTIT.EPM.Web.Areas.App.Models.DocumentRequests
{
    public class ViewAndSignDocumentEmailViewModel : ViewAndSignDocumentEmailInput, IShouldNormalize
    {
        /// <summary>
        /// Tenant id.
        /// </summary>
        public int? TenantId { get; set; }

        protected override void ResolveParameters()
        {
            base.ResolveParameters();

            if (!string.IsNullOrEmpty(c))
            {
                var parameters = SimpleStringCipher.Instance.Decrypt(c);
                var query = HttpUtility.ParseQueryString(parameters);

                if (query["tenantId"] != null)
                {
                    TenantId = Convert.ToInt32(query["tenantId"]);
                }
            }
        }

        public void ManualResolveParameters()
        {
            this.ResolveParameters();
        }
    }
}