using BTIT.EPM.Lookups;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class CreateOrEditDocumentRequestDto : EntityDto<long?>
    {

		[StringLength(DocumentRequestConsts.MaxDocumentTitleLength, MinimumLength = DocumentRequestConsts.MinDocumentTitleLength)]
		public string DocumentTitle { get; set; }
		
		
		public string MessageBody { get; set; }
		
		
		public DocumentRequestStatus Status { get; set; }
		
		
		public bool IsSigningOrdered { get; set; }
		
		

    }
}