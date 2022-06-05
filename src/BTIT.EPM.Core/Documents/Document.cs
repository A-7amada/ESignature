using BTIT.EPM.Storage;
using BTIT.EPM.DigitalSignature;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace BTIT.EPM.Documents
{
	[Table("Documents")]
    public class Document : FullAuditedEntity<long> , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		[StringLength(DocumentConsts.MaxFileNameLength, MinimumLength = DocumentConsts.MinFileNameLength)]
		public virtual string FileName { get; set; }
		
		[StringLength(DocumentConsts.MaxExtensionLength, MinimumLength = DocumentConsts.MinExtensionLength)]
		public virtual string Extension { get; set; }
		
		[Range(DocumentConsts.MinSizeValue, DocumentConsts.MaxSizeValue)]
		public virtual int Size { get; set; }
		
		[StringLength(DocumentConsts.MaxContentTypeLength, MinimumLength = DocumentConsts.MinContentTypeLength)]
		public virtual string ContentType { get; set; }
		
		public virtual bool IsActive { get; set; }
		

		public virtual Guid? BinaryObjectId { get; set; }
		
        [ForeignKey("BinaryObjectId")]
		public BinaryObject BinaryObjectFk { get; set; }


		public virtual long? DocumentBagId { get; set; }

		[ForeignKey("DocumentBagId")]
		public DocumentBag DocumentBagFk { get; set; }

		public virtual long? DocumentRequestId { get; set; }
		
        [ForeignKey("DocumentRequestId")]
		public DocumentRequest DocumentRequestFk { get; set; }

		[NotMapped]
		public string Recipients { get; set; }



	}
}