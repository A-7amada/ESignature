using System.Collections.Generic;
using BTIT.EPM.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace BTIT.EPM.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
