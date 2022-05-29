using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Abp.Runtime.Security;
using Abp.Runtime.Validation;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class ViewAndSignDocumentEmailInput : IShouldNormalize
    {
        public long RecipientId { get; set; }

        public long DocumentRequestId { get; set; }

        public string RecipientCode { get; set; }
        public string SignerPin { get; set; }

        /// <summary>
        /// Encrypted values for {TenantId}, {UserId} and {ConfirmationCode}
        /// </summary>
        public string c { get; set; }

        public void Normalize()
        {
            ResolveParameters();
        }

        protected virtual void ResolveParameters()
        {
            if (!string.IsNullOrEmpty(c))
            {
                var parameters = SimpleStringCipher.Instance.Decrypt(c);
                var query = HttpUtility.ParseQueryString(parameters);

                if (query["recipientId"] != null)
                {
                    RecipientId = Convert.ToInt32(query["recipientId"]);
                }

                if (query["documentRequestId"] != null)
                {
                    DocumentRequestId = Convert.ToInt32(query["documentRequestId"]);
                }

                if (query["recipientCode"] != null)
                {
                    RecipientCode = query["recipientCode"];
                }
            }
        }
    }
}