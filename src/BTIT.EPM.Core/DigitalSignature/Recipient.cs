using BTIT.EPM.Lookups;
using BTIT.EPM.Authorization.Users;
using BTIT.EPM.DigitalSignature;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace BTIT.EPM.DigitalSignature
{
	[Table("Recipients")]
    public class Recipient : FullAuditedEntity<long> , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual RecipientType Type { get; set; }
		
		[StringLength(RecipientConsts.MaxFirstNameLength, MinimumLength = RecipientConsts.MinFirstNameLength)]
		public virtual string FirstName { get; set; }
		
		[StringLength(RecipientConsts.MaxLastNameLength, MinimumLength = RecipientConsts.MinLastNameLength)]
		public virtual string LastName { get; set; }
		
		[RegularExpression(RecipientConsts.EmailRegex)]
		[StringLength(RecipientConsts.MaxEmailLength, MinimumLength = RecipientConsts.MinEmailLength)]
		public virtual string Email { get; set; }
		
		public virtual bool IsSigner { get; set; }
		
		public virtual Guid Code { get; set; }
		
		public virtual DateTime? ViewDate { get; set; }
		
		public virtual DateTime? SignatureDate { get; set; }
		
		[StringLength(RecipientConsts.MaxSignerPinLength, MinimumLength = RecipientConsts.MinSignerPinLength)]
		public virtual string SignerPin { get; set; }
		
		public virtual bool IsSigned { get; set; }
		
		public virtual int SigneOrder { get; set; }
		
		[StringLength(RecipientConsts.MaxFieldNameLength, MinimumLength = RecipientConsts.MinFieldNameLength)]
		public virtual string FieldName { get; set; }
		
		public virtual DateTime? SignerPinExpiry { get; set; }
		
		[RegularExpression(RecipientConsts.MobileNumberRegex)]
		[StringLength(RecipientConsts.MaxMobileNumberLength, MinimumLength = RecipientConsts.MinMobileNumberLength)]
		public virtual string MobileNumber { get; set; }
		
		public virtual DateTime? SentDate { get; set; }
		
		public virtual bool IsSent { get; set; }
		
		public virtual short? SignerPinTriesCount { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual long? DocumentRequestId { get; set; }
		
        [ForeignKey("DocumentRequestId")]
		public DocumentRequest DocumentRequestFk { get; set; }
		
    }
}