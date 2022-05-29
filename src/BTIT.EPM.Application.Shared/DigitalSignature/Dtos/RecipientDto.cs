using BTIT.EPM.Lookups;

using System;
using Abp.Application.Services.Dto;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class RecipientDto : EntityDto<long>
    {
		public RecipientType Type { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public bool IsSigner { get; set; }

		public Guid Code { get; set; }

		public DateTime? ViewDate { get; set; }

		public DateTime? SignatureDate { get; set; }

		public string SignerPin { get; set; }

		public bool IsSigned { get; set; }

		public int SigneOrder { get; set; }

		public string FieldName { get; set; }

		public DateTime? SentDate { get; set; }

		public bool IsSent { get; set; }

		public short? SignerPinTriesCount { get; set; }


		 public long? UserId { get; set; }

		 		 public long? DocumentRequestId { get; set; }

		 
    }
}