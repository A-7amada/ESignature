using BTIT.EPM.Documents.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace BTIT.EPM.Web.Areas.App.Models.Documents
{
    public class CreateOrEditDocumentModalViewModel
    {
       public CreateOrEditDocumentDto Document { get; set; }

	   		public string BinaryObjectTenantId { get; set;}

		public string DocumentRequestDocumentTitle { get; set;}


       public List<DocumentBinaryObjectLookupTableDto> DocumentBinaryObjectList { get; set;}

public List<DocumentDocumentRequestLookupTableDto> DocumentDocumentRequestList { get; set;}


	   public bool IsEditMode => Document.Id.HasValue;
    }
}