using Abp.Application.Services.Dto;
using System;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class GetAllDocumentRequestsForExcelInput
    {
		public string Filter { get; set; }

		public string DocumentTitleFilter { get; set; }

		public int StatusFilter { get; set; }

		public int IsSigningOrderedFilter { get; set; }



    }
}