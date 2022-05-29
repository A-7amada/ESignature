
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class CreateOrEditContactDto : EntityDto<long?>
    {

		[StringLength(ContactConsts.MaxFirstNameLength, MinimumLength = ContactConsts.MinFirstNameLength)]
		public string FirstName { get; set; }
		
		
		[StringLength(ContactConsts.MaxLastNameLength, MinimumLength = ContactConsts.MinLastNameLength)]
		public string LastName { get; set; }
		
		
		[RegularExpression(ContactConsts.EmailRegex)]
		[StringLength(ContactConsts.MaxEmailLength, MinimumLength = ContactConsts.MinEmailLength)]
		public string Email { get; set; }
		
		
		[RegularExpression(ContactConsts.PhoneNumberRegex)]
		[StringLength(ContactConsts.MaxPhoneNumberLength, MinimumLength = ContactConsts.MinPhoneNumberLength)]
		public string PhoneNumber { get; set; }
		
		

    }
}