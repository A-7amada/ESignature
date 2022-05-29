using BTIT.EPM.Documents.Dtos;
using System.Collections.Generic;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class DocumentAuditTrailDto
    {
        public List<DocumentRequestAuditTrailDto> DocumentRequestAuditTrails { get; set; }
        public DocumentDto Document { get; set; }

        public string DocumentRequestTitle { get; set; }
    }
}
