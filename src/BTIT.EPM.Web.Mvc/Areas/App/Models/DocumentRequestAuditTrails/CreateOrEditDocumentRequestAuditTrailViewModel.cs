using BTIT.EPM.DigitalSignature.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace BTIT.EPM.Web.Areas.App.Models.DocumentRequestAuditTrails
{
    public class CreateOrEditDocumentRequestAuditTrailModalViewModel
    {
       public CreateOrEditDocumentRequestAuditTrailDto DocumentRequestAuditTrail { get; set; }

	   		public string DocumentRequestDocumentTitle { get; set;}

		public string RecipientFirstName { get; set;}


       public List<DocumentRequestAuditTrailDocumentRequestLookupTableDto> DocumentRequestAuditTrailDocumentRequestList { get; set;}

public List<DocumentRequestAuditTrailRecipientLookupTableDto> DocumentRequestAuditTrailRecipientList { get; set;}


	   public bool IsEditMode => DocumentRequestAuditTrail.Id.HasValue;
    }
}