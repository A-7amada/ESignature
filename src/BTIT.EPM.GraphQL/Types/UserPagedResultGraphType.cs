using Abp.Application.Services.Dto;
using GraphQL.Types;
using BTIT.EPM.Dto;

namespace BTIT.EPM.Types
{
    public class UserPagedResultGraphType : ObjectGraphType<PagedResultDto<UserDto>>
    {
        public UserPagedResultGraphType()
        {
            Field(x => x.TotalCount);
            Field(x => x.Items, type: typeof(ListGraphType<UserType>));
        }
    }
}