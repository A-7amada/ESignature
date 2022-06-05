using Abp.Application.Services.Dto;
using System;

namespace BTIT.EPM.Documents.Dtos
{
    public class GetAllDocumentBagsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string DocumentBagIdFilter { get; set; }

    }
}