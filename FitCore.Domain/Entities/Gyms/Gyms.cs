using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Provinces;
using FitCore.Domain.Entities.Users;
using FitCore.Domain.Entities.Provinces;
using System;
using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

namespace FitCore.Domain.Entities.Gyms
{

    public class Gyms : BaseEntity
    {
        [Display(Name = "نام باشگاه")]
        [Required(ErrorMessage = "نام باشگاه الزامی است")]
        [MaxLength(200)]
        public string Name { get; set; }



        [Display(Name = "کد یکتای باشگاه")]
        [Required(ErrorMessage = "کد باشگاه الزامی است")]
        [MaxLength(50)]
        public string Code { get; set; }



        [Display(Name = "زیردامنه")]
        [MaxLength(100)]
        public string SubDomain { get; set; }



        [Display(Name = "نام برند")]
        [MaxLength(200)]
        public string BrandName { get; set; }



        [Display(Name = "توضیحات")]
        [MaxLength(1000)]
        public string Description { get; set; }



        [Display(Name = "لوگو")]
        public string Logo { get; set; }



        [Display(Name = "تلفن ثابت")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }



        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "تلفن همراه الزامی است")]
        [MaxLength(20)]
        public string MobileNumber { get; set; }



        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
        public string Email { get; set; }



        [Display(Name = "وب سایت")]
        public string Website { get; set; }



        [Display(Name = "استان")]
        [MaxLength(100)]
        public int? ProvincesId { get; set; }

        public Province Provinces { get; set; }


        [Display(Name = "شهر")]
        [MaxLength(100)]
        public int? CitiesId { get; set; }
        public City Cities { get; set; } 


        [Display(Name = "آدرس")]
        [MaxLength(500)]
        public string Address { get; set; }



        [Display(Name = "کد پستی")]
        [MaxLength(20)]
        public string PostalCode { get; set; }



        [Display(Name = "عرض جغرافیایی")]
        public double? Latitude { get; set; }



        [Display(Name = "طول جغرافیایی")]
        public double? Longitude { get; set; }



        [Display(Name = "فعال")]
        public bool IsActive { get; set; }



        [Display(Name = "تاریخ انقضای اشتراک")]
        public DateTime? SubscriptionExpireDate { get; set; }



        [Display(Name = "حداکثر تعداد اعضا")]
        public int MaxMembers { get; set; }



        [Display(Name = "اجازه ثبت نام آنلاین")]
        public bool AllowOnlineRegistration { get; set; }



        [Display(Name = "زمان اعتبار کد تایید (ثانیه)")]
        public int OtpExpireSeconds { get; set; }



        [Display(Name = "حداکثر درخواست کد در دقیقه")]
        public int MaxOtpRequestPerMinute { get; set; }



        [Display(Name = "کاربران باشگاه")]
        public ICollection<AppUser> Users { get; set; }



        [Display(Name = "اعضای باشگاه")]
        public ICollection<Member> Members { get; set; }
    }
}