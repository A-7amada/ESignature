using BTIT.EPM.Lookups;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;
using BTIT.EPM.Documents;

namespace BTIT.EPM.DigitalSignature
{
	[Table("DocumentRequests")]
    public class DocumentRequest : FullAuditedEntity<long> , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[StringLength(DocumentRequestConsts.MaxDocumentTitleLength, MinimumLength = DocumentRequestConsts.MinDocumentTitleLength)]
		public virtual string DocumentTitle { get; set; }
		
		public virtual string MessageBody { get; set; }
		
		public virtual DocumentRequestStatus Status { get; set; }
		
		public virtual bool IsSigningOrdered { get; set; }

		public List<DocumentRequestAuditTrail> DocumentRequestAuditTrails { get; set; }

		public List<Document> Documents { get; set; }
		public virtual ICollection<Recipient> Recipients { get; set; }
	}
}