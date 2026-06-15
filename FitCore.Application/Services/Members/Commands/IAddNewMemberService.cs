using FitCore.Application.Contexts;
using FitCore.Application.Services.Members.Commands;
using FitCore.Common.Dto;
using FitCore.Common.Roles;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Commands
{
    public interface IAddNewMemberService
    {
        Task<ResultDto> Execute(RequestAddNewMemberDto request);
    }
}


public class AddNewMemberService : IAddNewMemberService
{
    private readonly IDataBaseContext _context;

    private readonly UserManager<AppUser> _userManager;

    public AddNewMemberService(
        IDataBaseContext context,
        UserManager<AppUser> userManager)
    {
        _context = context;

        _userManager = userManager;
    }

    public async Task<ResultDto> Execute(
        RequestAddNewMemberDto request)
    {
        try
        {
            //==================================================
            // پیدا کردن مدیر باشگاه
            //==================================================

            var currentUser =
                await _userManager.Users
                .FirstOrDefaultAsync(x =>
                    x.Id == request.AppUserId);

            if (currentUser == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }

            //==================================================
            // GymId مدیر
            //==================================================

            var gymId = currentUser.GymId;

            if (gymId == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "باشگاه مدیر مشخص نیست"
                };
            }

            //==================================================
            // Trim
            //==================================================

            request.FullName =
                request.FullName?.Trim();

            request.Mobile =
                request.Mobile?.Trim();

            //==================================================
            // بررسی ثبت قبلی
            //==================================================

            var userExists =
                await _userManager.Users
                .AnyAsync(x =>

                    x.PhoneNumber == request.Mobile &&

                    x.GymId == gymId
                );

            if (userExists)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message =
                        "این عضو قبلاً در باشگاه ثبت شده است"
                };
            }

            //==================================================
            // ایجاد User
            //==================================================

            AppUser newUser = new AppUser()
            {
                FullName = request.FullName,

                UserName =
                    $"{request.Mobile}_{gymId}",

                PhoneNumber =
                    request.Mobile,

                IsActive = true,

                GymId = gymId
            };

            //==================================================
            // ذخیره User
            //==================================================

            var createUser =
                await _userManager
                .CreateAsync(
                    newUser,
                    "FitCore@123"
                );

            if (!createUser.Succeeded)
            {
                string errors = string.Join(
                    "\n",
                    createUser.Errors
                    .Select(x => x.Description));

                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = errors
                };
            }

            //==================================================
            // ثبت Role
            //==================================================

            var addRole =
                await _userManager
                .AddToRoleAsync(
                    newUser,
                    UserRoles.Member);

            if (!addRole.Succeeded)
            {
                string errors = string.Join(
                    "\n",
                    addRole.Errors
                    .Select(x => x.Description));

                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = errors
                };
            }

            //==================================================
            // ثبت Member
            //==================================================

            Member member = new Member()
            {
                AppUserId = newUser.Id,

                Gender = request.Gender,

                BirthDate = request.BirthDate,
                MembershipStartDate=request.MembershipStartDate,
                MembershipEndDate=request.MembershipEndDate,
                IsActive = true
            };

            await _context.Members.AddAsync(member);

            //==================================================
            // ذخیره نهایی
            //==================================================

            await _context.SaveChangesAsync();

            //==================================================
            // پایان
            //==================================================

            return new ResultDto()
            {
                IsSuccess = true,
                Message =
                    "عضو جدید با موفقیت ثبت شد"
            };
        }
        catch (Exception ex)
        {
            return new ResultDto()
            {
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }
}
//========================================================
// DTO
//========================================================
public class RequestAddNewMemberDto
{
    public long AppUserId { get; set; }

    [DisplayName("نام و نام خانوادگی")]
    [Required(ErrorMessage = "نام و نام خانوادگی الزامی است")]
    public string FullName { get; set; }

    [DisplayName("موبایل")]
    [Required(ErrorMessage = "شماره موبایل الزامی است")]
    public string Mobile { get; set; }

    [DisplayName("جنسیت")]
    public Gender Gender { get; set; }

    [DisplayName("تاریخ تولد")]
    public string BirthDate { get; set; }


    [DisplayName("تاریخ شروع عضویت")]
    public string MembershipStartDate { get; set; }

    [DisplayName("تاریخ پایان عضویت")]
    public string MembershipEndDate { get; set; }




}


