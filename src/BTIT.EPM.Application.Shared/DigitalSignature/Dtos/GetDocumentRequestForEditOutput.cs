using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class GetDocumentRequestForEditOutput
    {
		public CreateOrEditDocumentRequestDto DocumentRequest { get; set; }


    }
}