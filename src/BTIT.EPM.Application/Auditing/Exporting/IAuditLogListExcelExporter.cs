using System.Collections.Generic;
using BTIT.EPM.Auditing.Dto;
using BTIT.EPM.Dto;

namespace BTIT.EPM.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
