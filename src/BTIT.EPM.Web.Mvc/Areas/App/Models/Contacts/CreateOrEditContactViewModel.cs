using BTIT.EPM.DigitalSignature.Dtos;

using Abp.Extensions;

namespace BTIT.EPM.Web.Areas.App.Models.Contacts
{
    public class CreateOrEditContactModalViewModel
    {
       public CreateOrEditContactDto Contact { get; set; }

	   
       
	   public bool IsEditMode => Contact.Id.HasValue;
    }
}