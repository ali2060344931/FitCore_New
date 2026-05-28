using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Members;

using Microsoft.AspNetCore.Identity;

using System.Collections.Generic;

namespace FitCore.Domain.Entities.Users
{
    public class AppUser : IdentityUser<long>
    {
        public string FullName { get; set; }

        public long? GymId { get; set; }

        public Gyms.Gyms Gym { get; set; }

        public bool IsActive { get; set; }
        public ICollection<Member> Members { get; set; } = new List<Member>();
    }
}