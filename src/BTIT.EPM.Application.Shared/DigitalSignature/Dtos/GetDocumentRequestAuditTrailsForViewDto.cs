using BTIT.EPM.Lookups;
using System;
using System.Collections.Generic;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class GetDocumentRequestAuditTrailsForViewDto
    {
		public List<GetDocumentRequestAuditTrailForViewDetailsDto> DocumentRequestAuditTrailForViewDetailsDto { get; set; }

		public string DocumentRequestDocumentTitle { get; set;}

		public string FileName { get; set;}

        public DocumentRequestStatus Status { get; set; }

        public Guid DocumentId { get; set; }


    }
}