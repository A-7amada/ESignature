using BTIT.EPM.Lookups;
using BTIT.EPM.DigitalSignature;
using BTIT.EPM.DigitalSignature;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace BTIT.EPM.DigitalSignature
{
	[Table("DocumentRequestAuditTrails")]
    public class DocumentRequestAuditTrail : FullAuditedEntity<long> , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual AuditTrailType Type { get; set; }
		
		[StringLength(DocumentRequestAuditTrailConsts.MaxClientIpAddressLength, MinimumLength = DocumentRequestAuditTrailConsts.MinClientIpAddressLength)]
		public virtual string ClientIpAddress { get; set; }
		

		public virtual long DocumentRequestId { get; set; }
		
        [ForeignKey("DocumentRequestId")]
		public DocumentRequest DocumentRequestFk { get; set; }
		
		public virtual long? RecipientId { get; set; }
		
        [ForeignKey("RecipientId")]
		public Recipient RecipientFk { get; set; }
		
    }
}