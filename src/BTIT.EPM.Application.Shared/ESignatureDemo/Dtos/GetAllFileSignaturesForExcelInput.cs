using Abp.Application.Services.Dto;
using System;

namespace BTIT.EPM.ESignatureDemo.Dtos
{
    public class GetAllFileSignaturesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescribtionFilter { get; set; }

    }
}