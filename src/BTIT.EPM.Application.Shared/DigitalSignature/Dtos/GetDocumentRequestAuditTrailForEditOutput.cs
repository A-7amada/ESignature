using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class GetDocumentRequestAuditTrailForEditOutput
    {
		public CreateOrEditDocumentRequestAuditTrailDto DocumentRequestAuditTrail { get; set; }

		public string DocumentRequestDocumentTitle { get; set;}

		public string RecipientFirstName { get; set;}


    }
}