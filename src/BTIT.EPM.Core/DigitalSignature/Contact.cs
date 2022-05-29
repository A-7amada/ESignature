using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace BTIT.EPM.DigitalSignature
{
	[Table("Contacts")]
    public class Contact : FullAuditedEntity<long> , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[StringLength(ContactConsts.MaxFirstNameLength, MinimumLength = ContactConsts.MinFirstNameLength)]
		public virtual string FirstName { get; set; }
		
		[StringLength(ContactConsts.MaxLastNameLength, MinimumLength = ContactConsts.MinLastNameLength)]
		public virtual string LastName { get; set; }
		
		[RegularExpression(ContactConsts.EmailRegex)]
		[StringLength(ContactConsts.MaxEmailLength, MinimumLength = ContactConsts.MinEmailLength)]
		public virtual string Email { get; set; }
		
		[RegularExpression(ContactConsts.PhoneNumberRegex)]
		[StringLength(ContactConsts.MaxPhoneNumberLength, MinimumLength = ContactConsts.MinPhoneNumberLength)]
		public virtual string PhoneNumber { get; set; }
		

    }
}