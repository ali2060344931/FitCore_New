using FitCore.Domain.Entities.Provinces;

using System;
using System.ComponentModel.DataAnnotations;
namespace FitCore.Application.Services.Gyms.Commands.EditGym
{
    public class UpdateGymDto
    {
        public long Id { get; set; }

        [Display(Name = "نام باشگاه")]
        [Required(ErrorMessage = "نام باشگاه الزامی است")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Display(Name = "کد یکتای باشگاه")]
        [Required(ErrorMessage = "کد باشگاه الزامی است")]
        [MaxLength(50)]
        public int Code { get; set; }


        [Display(Name = "توضیحات")]
        [MaxLength(1000)]
        public string Description { get; set; }


        [Required(ErrorMessage = "شماره همراه الزامی است")]
        [Display(Name = "شماره موبایل")]
        [MaxLength(20)]
        public string MobileNumber { get; set; }


       /*
        public string SubDomain { get; set; }

        public string BrandName { get; set; }


        public string Logo { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Website { get; set; }

        public string Province { get; set; }

        public Cities Cities { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public bool IsActive { get; set; }

        public DateTime? SubscriptionExpireDate { get; set; }

        public int MaxMembers { get; set; }

        public bool AllowOnlineRegistration { get; set; }

        public int OtpExpireSeconds { get; set; }

        public int MaxOtpRequestPerMinute { get; set; }
      */
    }

}
