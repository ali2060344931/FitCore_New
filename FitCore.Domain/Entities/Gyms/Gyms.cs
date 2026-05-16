using FitCore.Domain.Entities.Commons;

using System.Collections.Generic;

namespace FitCore.Domain.Entities.Gyms
{
    public class Gym : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }   // برای ورود یا ساب‌دامین

        public string Mobile { get; set; }

        public string Address { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Members.Member> Members {  get; set; }
    }
}