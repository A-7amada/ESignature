using BTIT.EPM.DigitalSignature.Dtos;

using Abp.Extensions;
using System.Collections.Generic;
using System;

namespace BTIT.EPM.Web.Areas.App.Models.DocumentRequests
{
    public class CreateOrEditDocumentRequestModalViewModel
    {
        //public CreateOrEditDocumentRequestDto DocumentRequest { get; set; }

        public bool IsSigningOrdered { get; set; }
        public long? DocumentRequestId { get; set; }
        public Guid BinaryObjectId { get; set; }
        public string BinaryObjectContentType { get; set; }
        public string DocumentTitle { get; set; }
        public string MessageBody { get; set; }
        public List<CreateOrEditRecipientDto> Recipients { get; set; }

        public bool IsEditMode => DocumentRequestId.HasValue;
    }

}