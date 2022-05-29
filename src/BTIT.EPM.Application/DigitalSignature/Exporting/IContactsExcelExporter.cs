using System.Collections.Generic;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.Dto;

namespace BTIT.EPM.DigitalSignature.Exporting
{
    public interface IContactsExcelExporter
    {
        FileDto ExportToFile(List<GetContactForViewDto> contacts);
    }
}