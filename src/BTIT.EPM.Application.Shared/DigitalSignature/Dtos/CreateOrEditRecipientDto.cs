using BTIT.EPM.Lookups;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class CreateOrEditRecipientDto : EntityDto<long?>
    {

		public RecipientType Type { get; set; }
		
		
		[StringLength(RecipientConsts.MaxFirstNameLength, MinimumLength = RecipientConsts.MinFirstNameLength)]
		public string FirstName { get; set; }
		
		
		[StringLength(RecipientConsts.MaxLastNameLength, MinimumLength = RecipientConsts.MinLastNameLength)]
		public string LastName { get; set; }
		
		
		[RegularExpression(RecipientConsts.EmailRegex)]
		[StringLength(RecipientConsts.MaxEmailLength, MinimumLength = RecipientConsts.MinEmailLength)]
		public string Email { get; set; }
		
		
		public bool IsSigner { get; set; }
		
		
		public Guid? Code { get; set; }
		
		
		public DateTime? ViewDate { get; set; }
		
		
		public DateTime? SignatureDate { get; set; }
		
		
		[StringLength(RecipientConsts.MaxSignerPinLength, MinimumLength = RecipientConsts.MinSignerPinLength)]
		public string SignerPin { get; set; }
		
		
		public bool IsSigned { get; set; }
		
		
		public int SigneOrder { get; set; }
		
		
		[StringLength(RecipientConsts.MaxFieldNameLength, MinimumLength = RecipientConsts.MinFieldNameLength)]
		public string FieldName { get; set; }
		
		
		public DateTime? SignerPinExpiry { get; set; }
		
		
		[RegularExpression(RecipientConsts.MobileNumberRegex)]
		[StringLength(RecipientConsts.MaxMobileNumberLength, MinimumLength = RecipientConsts.MinMobileNumberLength)]
		public string MobileNumber { get; set; }
		
		
		public DateTime? SentDate { get; set; }
		
		
		public bool IsSent { get; set; }
		
		
		public short? SignerPinTriesCount { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 		 public long? DocumentRequestId { get; set; }
		 
		 
    }
}