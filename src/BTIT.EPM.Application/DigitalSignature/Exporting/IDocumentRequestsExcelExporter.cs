using System.Collections.Generic;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.Dto;

namespace BTIT.EPM.DigitalSignature.Exporting
{
    public interface IDocumentRequestsExcelExporter
    {
        FileDto ExportToFile(List<GetDocumentRequestForViewDto> documentRequests);
    }
}