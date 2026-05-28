using FitCore.Common.Roles;

using Microsoft.AspNetCore.Authorization;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Persistence.Security
{
    // گارد برای مدیران ارشد (گزارشات مالی، تنظیمات سیستم)
    public class AuthorizeAdminAttribute : AuthorizeAttribute
    {
        public AuthorizeAdminAttribute() => Roles = $"{UserRoles.SuperAdmin},{UserRoles.Admin}";
    }

    // گارد برای کارکنان (اپراتور، مدیر، ادمین)
    public class AuthorizeStaffAttribute : AuthorizeAttribute
    {
        public AuthorizeStaffAttribute() => Roles = $"{UserRoles.SuperAdmin},{UserRoles.Admin},{UserRoles.Operator}";
    }

    // گارد برای مربیان
    public class AuthorizeTrainerAttribute : AuthorizeAttribute
    {
        public AuthorizeTrainerAttribute() => Roles = $"{UserRoles.SuperAdmin},{UserRoles.Admin},{UserRoles.Trainer}";
    }
}
