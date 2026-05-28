using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.Users;

using System;
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
        // اگر تاریخ تولد قرار است اجباری باشد، Required را اضافه کنید
        public string BirthDate { get; set; }

        [DisplayName("تاریخ شروع عضویت")]
        public string MembershipStartDate { get; set; }

        [DisplayName("تاریخ پایان عضویت")]
        public string MembershipEndDate { get; set; }

        [DisplayName("قد (cm)")]
        [Range(0, 300, ErrorMessage = "قد باید بین ۰ تا ۳۰۰ سانتی‌متر باشد.")]
        public decimal? Height { get; set; }

        [DisplayName("وزن (kg)")]
        [Range(0, 500, ErrorMessage = "وزن باید بین ۰ تا ۵۰۰ کیلوگرم باشد.")]
        public decimal? Weight { get; set; }

        [DisplayName("وضعیت عضویت")]
        public bool IsActive { get; set; } = true;

        [DisplayName("توضیحات")]
        [MaxLength(500, ErrorMessage = "توضیحات نباید بیشتر از ۵۰۰ کاراکتر باشد.")]
        public string Description { get; set; }

    }
    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}