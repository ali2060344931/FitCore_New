using FitCore.Common.Roles;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Common
{
    public static class AuthorizationExtensions
    {
        public static bool IsSuperAdmin(this ClaimsPrincipal user) => user.IsInRole(UserRoles.SuperAdmin);

        public static bool IsAdmin(this ClaimsPrincipal user) => user.IsInRole(UserRoles.SuperAdmin) || user.IsInRole(UserRoles.Admin);


        public static bool IsStaff(this ClaimsPrincipal user) => user.IsAdmin() || user.IsInRole(UserRoles.Operator);
    }
}
