using System.Collections.Generic;
using BTIT.EPM.Authorization.Users.Dto;
using BTIT.EPM.Dto;

namespace BTIT.EPM.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}