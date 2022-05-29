
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BTIT.EPM.Documents.Dtos
{
    public class CreateOrEditDocumentDto : EntityDto<long?>
    {

		[Required]
		[StringLength(DocumentConsts.MaxFileNameLength, MinimumLength = DocumentConsts.MinFileNameLength)]
		public string FileName { get; set; }
		
		
		[StringLength(DocumentConsts.MaxExtensionLength, MinimumLength = DocumentConsts.MinExtensionLength)]
		public string Extension { get; set; }
		
		
		[Range(DocumentConsts.MinSizeValue, DocumentConsts.MaxSizeValue)]
		public int Size { get; set; }
		
		
		[StringLength(DocumentConsts.MaxContentTypeLength, MinimumLength = DocumentConsts.MinContentTypeLength)]
		public string ContentType { get; set; }
		
		
		public bool IsActive { get; set; }
		
		
		 public Guid? BinaryObjectId { get; set; }
		 
		 		 public long? DocumentRequestId { get; set; }
		 
		 
    }
}