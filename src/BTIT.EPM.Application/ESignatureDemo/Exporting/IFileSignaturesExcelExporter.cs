using System.Collections.Generic;
using BTIT.EPM.ESignatureDemo.Dtos;
using BTIT.EPM.Dto;

namespace BTIT.EPM.ESignatureDemo.Exporting
{
    public interface IFileSignaturesExcelExporter
    {
        FileDto ExportToFile(List<GetFileSignatureForViewDto> fileSignatures);
    }
}