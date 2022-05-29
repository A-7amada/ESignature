using Abp.Application.Services.Dto;
using System;

namespace BTIT.EPM.Documents.Dtos
{
    public class GetAllDocumentsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string FileNameFilter { get; set; }

		public string ExtensionFilter { get; set; }

		public int? MaxSizeFilter { get; set; }
		public int? MinSizeFilter { get; set; }

		public string ContentTypeFilter { get; set; }

		public int IsActiveFilter { get; set; }


		 public string BinaryObjectTenantIdFilter { get; set; }

		 		 public string DocumentRequestDocumentTitleFilter { get; set; }

		 
    }
}