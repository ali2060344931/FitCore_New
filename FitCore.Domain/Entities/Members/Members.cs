using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.Users;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FitCore.Domain.Entities.Members
{
    public class Member : BaseEntity
    {
        [DisplayName("کاربر")]
        [Required(ErrorMessage = "انتخاب کاربر الزامی است.")]
        public long AppUserId { get; set; }
        public AppUser AppUser { get; set; }


        [DisplayName("جنسیت")]
        public Gender Gender { get; set; }

        [DisplayName("تاریخ تولد")]
        public string BirthDate { get; set; }

        [DisplayName("تاریخ شروع عضویت")]
        public string MembershipStartDate { get; set; }

        [DisplayName("تاریخ پایان عضویت")]
        public string MembershipEndDate { get; set; }

        [DisplayName("قد (سانتی‌متر)")]
        [Range(50, 250)]
        public decimal? Height { get; set; }

        public int? ActivityLevelId { get; set; }

        [DisplayName("سطح فعالیت بدنی")]
        public ActivityLevel ActivityLevel { get; set; }

        public int? ExperienceLevelId { get; set; }
        [DisplayName("سطح تجربه تمرینی")]
        public ExperienceLevel ExperienceLevel { get; set; }


        [DisplayName("حساسیت‌های غذایی")]
        [MaxLength(500)]
        public string FoodAllergies { get; set; }


        [DisplayName("بیماری‌ها / شرایط پزشکی")]
        [MaxLength(500)]
        public string MedicalConditions { get; set; }


        [DisplayName("سوابق آسیب‌دیدگی")]
        [MaxLength(500)]
        public string Injuries { get; set; }


        [DisplayName("وضعیت فعالیت عضویت")]
        public bool IsActive { get; set; } = true;

        [DisplayName("توضیحات")]
        [MaxLength(500, ErrorMessage = "توضیحات نباید بیشتر از ۵۰۰ کاراکتر باشد.")]
        public string Description { get; set; }
        public ICollection<MemberBodyMeasurement> memberBodyMeasurements { get; set; }
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }


    /// <summary>
    /// اندازه‌گیری‌های دوره‌ای بدن عضو
    /// </summary>
    public class MemberBodyMeasurement : BaseEntity
    {
        [DisplayName("عضو باشگاه")]
        public long MemberId { get; set; }

        public Member Member { get; set; }


        [DisplayName("تاریخ ثبت اندازه‌گیری")]
        public string RecordDate { get; set; }


        [DisplayName("وزن (کیلوگرم)")]
        [Range(0, 500)]
        public decimal? Weight { get; set; }

        //[DisplayName("قد (سانتی‌متر)")]
        //[Range(50, 250)]
        //public decimal? Height { get; set; }

        [DisplayName("درصد چربی بدن")]
        [Range(0, 100)]
        public decimal? BodyFatPercentage { get; set; }


        [DisplayName("دور کمر (سانتی‌متر)")]
        [Range(0, 300)]
        public decimal? Waist { get; set; }


        [DisplayName("دور باسن (سانتی‌متر)")]
        [Range(0, 300)]
        public decimal? Hip { get; set; }


        [DisplayName("دور سینه (سانتی‌متر)")]
        [Range(0, 300)]
        public decimal? Chest { get; set; }
    }


    /// <summary>
    /// سطح فعالیت بدنی روزانه فرد
    /// </summary>
    public class ActivityLevel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Member> members { get; set; }

    }

    /// <summary>
    /// سطح تجربه تمرینی فرد
    /// </summary>
    public class ExperienceLevel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Member> members { get; set; }

    }
}