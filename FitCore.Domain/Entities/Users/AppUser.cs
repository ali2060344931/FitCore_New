using FitCore.Domain.Entities.Gyms;

using Microsoft.AspNetCore.Identity;

namespace FitCore.Domain.Entities.Users
{
    public class AppUser : IdentityUser<long>
    {
        // هر کاربر متعلق به یک باشگاه
        public long? GymId { get; set; }

        public Gym Gym { get; set; }

        // نام کامل
        public string FullName { get; set; }

        // فعال یا غیرفعال
        public bool IsActive { get; set; } = true;
    }
}