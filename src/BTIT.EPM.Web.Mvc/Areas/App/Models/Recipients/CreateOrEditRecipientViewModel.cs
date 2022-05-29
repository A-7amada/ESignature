using BTIT.EPM.DigitalSignature.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace BTIT.EPM.Web.Areas.App.Models.Recipients
{
    public class CreateOrEditRecipientModalViewModel
    {
       public CreateOrEditRecipientDto Recipient { get; set; }

	   		public string UserName { get; set;}

		public string DocumentRequestDocumentTitle { get; set;}


       public List<RecipientUserLookupTableDto> RecipientUserList { get; set;}

public List<RecipientDocumentRequestLookupTableDto> RecipientDocumentRequestList { get; set;}


	   public bool IsEditMode => Recipient.Id.HasValue;
    }
}