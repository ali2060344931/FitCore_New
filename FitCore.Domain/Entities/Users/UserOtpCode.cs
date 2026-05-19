using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Domain.Entities.Users
{
    /// <summary>
    /// کد اس ام اس کاربری
    /// </summary>
    public class UserOtpCode
    {
        public long Id { get; set; }

        public string PhoneNumber { get; set; }

        public string Code { get; set; }

        public DateTime ExpireTime { get; set; }

        public bool IsUsed { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
