using BTIT.EPM.Lookups;

using System;
using Abp.Application.Services.Dto;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class DocumentRequestDto : EntityDto<long>
    {
		public string DocumentTitle { get; set; }

		public DocumentRequestStatus Status { get; set; }

		public bool IsSigningOrdered { get; set; }

		public string Recipients { get; set; }		

		public DateTime CreationTime { get; set; }

		public string FileGuid { get; set; }

	}
}