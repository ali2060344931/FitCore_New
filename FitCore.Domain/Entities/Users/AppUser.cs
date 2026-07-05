using FitCore.Domain.Entities.Members;

using Microsoft.AspNetCore.Identity;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FitCore.Domain.Entities.Users
{
    public class AppUser : IdentityUser<long>
    {

        [DisplayName("نام و نام خانوادگی")]
        [Required(ErrorMessage = "نام و نام خانوادگی الزامی است.")]
        [MaxLength(100, ErrorMessage = "نام نباید بیشتر از 150 کاراکتر باشد.")]
        public string FullName { get; set; }

        [DisplayName("شماره موبایل")]
        //[Required(ErrorMessage = "شماره موبایل الزامی است.")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "فرمت شماره موبایل صحیح نیست. (مثال: 09123456789)")]
        public string Mobile { get; set; }


        public long? GymId { get; set; }

        public Gyms.Gym Gym { get; set; }

        public bool IsActive { get; set; }
        public Member Member { get; set; }

        public long? BaleChatId { get; set; }

    }
}