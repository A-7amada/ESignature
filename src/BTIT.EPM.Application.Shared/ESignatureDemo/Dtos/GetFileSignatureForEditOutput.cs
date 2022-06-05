using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using BTIT.EPM.Documents.Dtos;
using System.Collections.Generic;

namespace BTIT.EPM.ESignatureDemo.Dtos
{
    public class GetFileSignatureForEditOutput
    {
        public CreateOrEditFileSignatureDto FileSignature { get; set; }
        public List<DocumentDto> Documents { get; set; }

    }
}