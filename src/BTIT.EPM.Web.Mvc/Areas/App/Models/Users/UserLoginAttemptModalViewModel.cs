using System.Collections.Generic;
using BTIT.EPM.Authorization.Users.Dto;

namespace BTIT.EPM.Web.Areas.App.Models.Users
{
    public class UserLoginAttemptModalViewModel
    {
        public List<UserLoginAttemptDto> LoginAttempts { get; set; }
    }
}