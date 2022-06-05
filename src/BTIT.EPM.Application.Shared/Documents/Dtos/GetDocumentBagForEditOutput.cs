using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BTIT.EPM.Documents.Dtos
{
    public class GetDocumentBagForEditOutput
    {
        public CreateOrEditDocumentBagDto DocumentBag { get; set; }

    }
}