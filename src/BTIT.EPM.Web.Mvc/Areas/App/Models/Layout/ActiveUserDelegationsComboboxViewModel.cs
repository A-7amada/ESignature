using System.Collections.Generic;
using BTIT.EPM.Authorization.Delegation;
using BTIT.EPM.Authorization.Users.Delegation.Dto;

namespace BTIT.EPM.Web.Areas.App.Models.Layout
{
    public class ActiveUserDelegationsComboboxViewModel
    {
        public IUserDelegationConfiguration UserDelegationConfiguration { get; set; }
        
        public List<UserDelegationDto> UserDelegations { get; set; }
    }
}
