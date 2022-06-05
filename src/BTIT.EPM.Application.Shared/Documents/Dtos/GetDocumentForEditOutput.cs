using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BTIT.EPM.Documents.Dtos
{
    public class GetDocumentForEditOutput
    {
        public CreateDocumentInputDto Document { get; set; }
        public string DocumentBagDocumentBagId { get; set; }

        public string BinaryObjectDescription { get; set; }

        public string BinaryObjectTenantId { get; set;}

		public string DocumentRequestDocumentTitle { get; set;}


    }
}