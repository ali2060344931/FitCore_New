using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Gyms.Commands.EditGym
{
    using System;
    using System.ComponentModel.DataAnnotations;

    namespace FitCore.Application.ViewModels.Gyms
    {
        public class CompleteGymInfoDto
        {
            public long Id { get; set; }

            [Display(Name = "نام برند")]
            public string BrandName { get; set; }

            [Display(Name = "زیردامنه")]
            public string SubDomain { get; set; }

            [Display(Name = "تلفن ثابت")]
            public string PhoneNumber { get; set; }

            [Display(Name = "ایمیل")]
            public string Email { get; set; }

            [Display(Name = "وب سایت")]
            public string Website { get; set; }

            [Display(Name = "شهر")]
            public int? CitiesId { get; set; }

            [Display(Name = "آدرس")]
            public string Address { get; set; }

            [Display(Name = "کد پستی")]
            public string PostalCode { get; set; }

            [Display(Name = "عرض جغرافیایی")]
            public double? Latitude { get; set; }

            [Display(Name = "طول جغرافیایی")]
            public double? Longitude { get; set; }

            [Display(Name = "فعال")]
            public bool IsActive { get; set; }

            [Display(Name = "تاریخ انقضا")]
            public String SubscriptionExpireDate { get; set; }

            [Display(Name = "حداکثر اعضا")]
            public int MaxMembers { get; set; }

            [Display(Name = "ثبت نام آنلاین")]
            public bool AllowOnlineRegistration { get; set; }

            [Display(Name = "اعتبار OTP")]
            public int OtpExpireSeconds { get; set; }

            [Display(Name = "حداکثر درخواست OTP")]
            public int MaxOtpRequestPerMinute { get; set; }
        }
    }
}
