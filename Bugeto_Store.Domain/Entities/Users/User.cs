using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugeto_Store.Domain.Entities.Users
{
    /// <summary>
    /// لیست کاربران
    /// </summary>
    public class User
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime RemoveTime { get; set; }
        public ICollection<UserInRole > UserInRoles { get; set; }

    }
}
