using Abp.Application.Services.Dto;
using System;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class GetAllDocumentRequestAuditTrailsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int TypeFilter { get; set; }

		public string ClientIpAddressFilter { get; set; }


		 public string DocumentRequestDocumentTitleFilter { get; set; }

		 		 public string RecipientFirstNameFilter { get; set; }

		 
    }
}