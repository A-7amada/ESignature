using BTIT.EPM.DigitalSignature.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTIT.EPM.Web.Areas.App.Models.DocumentRequests
{
    public class ViewAndSignDocumentViewModel
    {
        public DocumentRequestViewModel DocumentRequest { get; set; }

        public DocumentRequestAuditTrailsForViewModel DocumentRequestAuditTrailsForViewModel { get; set; }

        public ViewAndSignDocumentEmailViewModel ViewAndSignDocumentEmailViewModel { get; set; }

        public List<RecipientDto> Recipients { get; set; }

        public bool ShowSignButton { get; set; }

    }
}
