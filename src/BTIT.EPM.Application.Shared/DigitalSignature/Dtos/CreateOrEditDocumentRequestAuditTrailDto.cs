using BTIT.EPM.Lookups;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class CreateOrEditDocumentRequestAuditTrailDto : EntityDto<long?>
    {

		public AuditTrailType Type { get; set; }
		
		
		[StringLength(DocumentRequestAuditTrailConsts.MaxClientIpAddressLength, MinimumLength = DocumentRequestAuditTrailConsts.MinClientIpAddressLength)]
		public string ClientIpAddress { get; set; }
		
		
		 public long DocumentRequestId { get; set; }
		 
		 		 public long? RecipientId { get; set; }
		 
		 
    }
}