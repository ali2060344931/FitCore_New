using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Users;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using MemberEntity = FitCore.Domain.Entities.Members.Member;

namespace FitCore.Application.Services.Members.Commands
{
    public interface IAddNewMemberService
    {
        ResultDto Execute(RequestAddNewMemberDto request);
    }

    public class AddNewMemberService : IAddNewMemberService
    {
        private readonly IDataBaseContext _context;

        public AddNewMemberService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestAddNewMemberDto request)
        {
            MemberEntity member = new MemberEntity()
            {
                AppUserId = request.AppUserId,

                //FirstName = request.FirstName,

                //LastName = request.LastName,

                //Mobile = request.Mobile,

                Gender = request.Gender,

                BirthDate = request.BirthDate,

                IsActive = true
            };

            _context.Members.Add(member);

            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "عضو جدید با موفقیت ثبت شد"
            };
        }
    }

    public class RequestAddNewMemberDto
    {
        //============

        [DisplayName("کاربر")]
        [Required(ErrorMessage = "انتخاب کاربر الزامی است.")]
        [ForeignKey("AppUserId")]
        public long AppUserId { get; set; }
        public AppUser AppUser { get; set; }



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
        [Required(ErrorMessage = "انتخاب جنسیت الزامی است.")]
        
        public Gender Gender { get; set; }

        [DisplayName("تاریخ تولد")]
        // اگر تاریخ تولد قرار است اجباری باشد، Required را اضافه کنید
        // [Required(ErrorMessage = "تاریخ تولد الزامی است.")]
        public string BirthDate { get; set; }






    }
}
