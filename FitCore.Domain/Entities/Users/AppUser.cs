using FitCore.Domain.Entities.Gyms;

using Microsoft.AspNetCore.Identity;

namespace FitCore.Domain.Entities.Users
{
    public class AppUser : IdentityUser<long>
    {
        public string FullName { get; set; }

        public long GymId { get; set; }

        public Gym Gym { get; set; }

        public bool IsActive { get; set; }
    }
}