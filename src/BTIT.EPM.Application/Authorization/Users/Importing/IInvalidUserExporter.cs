using System.Collections.Generic;
using BTIT.EPM.Authorization.Users.Importing.Dto;
using BTIT.EPM.Dto;

namespace BTIT.EPM.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
