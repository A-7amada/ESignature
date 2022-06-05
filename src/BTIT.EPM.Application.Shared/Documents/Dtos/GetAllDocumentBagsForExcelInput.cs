using Abp.Application.Services.Dto;
using System;

namespace BTIT.EPM.Documents.Dtos
{
    public class GetAllDocumentBagsForExcelInput
    {
        public string Filter { get; set; }

        public string DocumentBagIdFilter { get; set; }

    }
}