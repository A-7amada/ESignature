using BTIT.EPM.Lookups;
using System;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class GetDocumentRequestAuditTrailForViewDetailsDto
    {
		public AuditTrailType Type { get; set; }

		public string ClientIpAddress { get; set; }

		public string RecipientName { get; set; }

		public string RecipientEmail { get; set; }

		public string Message { get; set; }

		public DateTime CreationTime { get; set; }

	}
}