using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Users;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace FitCore.Domain.Entities.Members
{
    public class Member : BaseEntity
    {
        [DisplayName("کاربر")]
        [Required(ErrorMessage = "انتخاب کاربر الزامی است.")]
        //[ForeignKey("AppUserId")]
        public long AppUserId { get; set; }
        public AppUser AppUser    { get; set; }

        //public long GymId { get; set; }
        //public Gyms.Gyms Gym { get; set; }

        //[DisplayName("نام")]
        //[Required(ErrorMessage = "نام الزامی است.")]
        //[MaxLength(100, ErrorMessage = "نام نباید بیشتر از ۱۰۰ کاراکتر باشد.")]
        //public string FirstName { get; set; }

        //[DisplayName("نام خانوادگی")]
        //[Required(ErrorMessage = "نام خانوادگی الزامی است.")]
        //[MaxLength(100, ErrorMessage = "نام خانوادگی نباید بیشتر از ۱۰۰ کاراکتر باشد.")]
        //public string LastName { get; set; }

        //[DisplayName("شماره موبایل")]
        //[Required(ErrorMessage = "شماره موبایل الزامی است.")]
        //[RegularExpression(@"^09\d{9}$", ErrorMessage = "فرمت شماره موبایل صحیح نیست. (مثال: 09123456789)")]
        //public string Mobile { get; set; }

        [DisplayName("جنسیت")]
        //[Required(ErrorMessage = "انتخاب جنسیت الزامی است.")]
        public Gender Gender { get; set; }

        [DisplayName("تاریخ تولد")]
        // اگر تاریخ تولد قرار است اجباری باشد، Required را اضافه کنید
        // [Required(ErrorMessage = "تاریخ تولد الزامی است.")]
        public string BirthDate { get; set; }

        [DisplayName("تاریخ شروع عضویت")]
        //[Required(ErrorMessage = "تاریخ شروع عضویت الزامی است.")]
        public string MembershipStartDate { get; set; }

        [DisplayName("تاریخ پایان عضویت")]
        //[Required(ErrorMessage = "تاریخ پایان عضویت الزامی است.")]
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