using System.Collections.Generic;
using BTIT.EPM.Documents.Dtos;
using BTIT.EPM.Dto;

namespace BTIT.EPM.Documents.Exporting
{
    public interface IDocumentsExcelExporter
    {
        FileDto ExportToFile(List<GetDocumentForViewDto> documents);
    }
}