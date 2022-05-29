using Abp.Application.Services.Dto;
using BTIT.EPM.Lookups;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class DocumentRequestAuditTrailDto : EntityDto<long>
    {
        public AuditTrailType Type { get; set; }

        public string ClientIpAddress { get; set; }

        public long DocumentRequestId { get; set; }

        public long? RecipientId { get; set; }

        public string CreationDate { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}