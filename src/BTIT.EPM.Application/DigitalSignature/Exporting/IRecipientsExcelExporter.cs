using System.Collections.Generic;
using BTIT.EPM.DigitalSignature.Dtos;
using BTIT.EPM.Dto;

namespace BTIT.EPM.DigitalSignature.Exporting
{
    public interface IRecipientsExcelExporter
    {
        FileDto ExportToFile(List<GetRecipientForViewDto> recipients);
    }
}