using FitCore.Domain.Entities.Members;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FitCore.Application.Services.Member.Queries
{
    public class GetMemberByIdDto
    {
        public long Id { get; set; }

        [DisplayName("نام و نام خانوادگی")]
        [Required(ErrorMessage = "نام و نام خانوادگی الزامی است.")]
        [MaxLength(100, ErrorMessage = "نام و نام خانوادگی نباید بیشتر از ۱5۰ کاراکتر باشد.")]
        public string FullName { get; set; }


        [DisplayName("شماره موبایل")]
        [Required(ErrorMessage = "شماره موبایل الزامی است.")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "فرمت شماره موبایل صحیح نیست. (مثال: 09123456789)")]
        public string Mobile { get; set; }

        [DisplayName("جنسیت")]
        //[Required(ErrorMessage = "انتخاب جنسیت الزامی است.")]

        //public int? GenderId { get; set; }
        public Gender Gender { get; set; }

        [DisplayName("تاریخ تولد")]
        // اگر تاریخ تولد قرار است اجباری باشد، Required را اضافه کنید
        // [Required(ErrorMessage = "تاریخ تولد الزامی است.")]
        public string BirthDate { get; set; }
    }



}
