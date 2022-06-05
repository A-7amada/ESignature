using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BTIT.EPM.Documents.Dtos
{
    public class CreateOrEditDocumentBagDto : EntityDto<long?>
    {

        public string DocumentBagId { get; set; }

    }
}