using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Users;

using System.Collections.Generic;

namespace FitCore.Domain.Entities.Gyms
{
    public class Gym : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; } // Unique

        public string SubDomain { get; set; }

        public bool IsActive { get; set; }


        public ICollection<AppUser> Users { get; set; }

        public ICollection<Member> Members { get; set; }
    }
}