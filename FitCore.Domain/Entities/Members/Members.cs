using FitCore.Domain.Entities.Commons;

using System;
using System.Reflection;

namespace FitCore.Domain.Entities.Members
{
    public class Member : BaseEntity
    {
        public long GymId { get; set; }
        public Gyms.Gym Gym { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Mobile { get; set; }

        public Gender Gender { get; set; }

        public DateTime BirthDate { get; set; }

        // تاریخ شروع عضویت
        public DateTime MembershipStartDate { get; set; }

        // تاریخ پایان عضویت
        public DateTime? MembershipEndDate { get; set; }

        // قد (برای تحلیل بدن)
        public float? Height { get; set; }

        // وزن
        public float? Weight { get; set; }

        // وضعیت فعال بودن عضویت
        public bool IsActive { get; set; } = true;

        // توضیحات
        public string? Description { get; set; }
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}