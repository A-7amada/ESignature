using System;

namespace BTIT.EPM.DigitalSignature.Dtos
{
	public class DocumentRequestViewAndSignDto
	{
		public string Name { get; set; }
		public virtual string Email { get; set; }
		public virtual bool IsSigner { get; set; }
		public virtual Guid Code { get; set; }
		public virtual DateTime? ViewDate { get; set; }
		public virtual DateTime? SignatureDate { get; set; }
		public virtual DateTime? SentDate { get; set; }
		public virtual string SignerPin { get; set; }
		public virtual bool IsSigned { get; set; }
		public virtual bool IsSent { get; set; }
		public virtual int SigneOrder { get; set; }
		public virtual long? UserId { get; set; }
		public virtual string UserName { get; set; }
		public virtual long? DocumentRequestId { get; set; }
		public DocumentRequestDto DocumentRequest { get; set; }
	}
}
