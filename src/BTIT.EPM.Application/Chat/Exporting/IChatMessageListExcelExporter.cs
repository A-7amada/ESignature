using System.Collections.Generic;
using Abp;
using BTIT.EPM.Chat.Dto;
using BTIT.EPM.Dto;

namespace BTIT.EPM.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
