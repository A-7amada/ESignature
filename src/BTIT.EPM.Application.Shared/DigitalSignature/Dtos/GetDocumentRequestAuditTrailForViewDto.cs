using BTIT.EPM.Documents.Dtos;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class GetDocumentRequestAuditTrailForViewDto
    {
		public DocumentRequestAuditTrailDto DocumentRequestAuditTrail { get; set; }

        public DocumentDto Document { get; set; }

        public string DocumentRequestDocumentTitle { get; set;}

		public string RecipientFirstName { get; set;}
    }
}