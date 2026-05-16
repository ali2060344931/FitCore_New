using FitCore.Application.Services.Users.Queries.GetUsers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Users.Queries.GetUsersRoles
{
    public interface IGetUsersRoles
    {
        ReslutGetUserDto Execute(RequestGetUserDto request);
    }

    public class RequestGetUserRolesDto()
    {
        public long UserId {  get; set; }

    }
}
