using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace BTIT.EPM.DigitalSignature.Dtos
{
    public class GetRecipientForEditOutput
    {
		public CreateOrEditRecipientDto Recipient { get; set; }

		public string UserName { get; set;}

		public string DocumentRequestDocumentTitle { get; set;}


    }
}