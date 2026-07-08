using FitCore.Application.Contexts;
using FitCore.Application.Services;
using FitCore.Application.Services.Members.Commands;
using FitCore.Common.Dto;
using FitCore.Common.Roles;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    private readonly IFileCompressionService _fileService;
    private readonly UserManager<AppUser> _userManager;

    public AddNewMemberService(
        IDataBaseContext context,
        UserManager<AppUser> userManager,
        IFileCompressionService fileService
        )
    {
        _context = context;
        _fileService = fileService;
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



            // ===== پردازش و فشرده‌سازی فایل‌ها =====

            var memberProfileImageUrl = _context.Members.Where(c => c.AppUserId == request.AppUserId).FirstOrDefault();

            request.ProfileImageUrl = await _fileService.ReplaceImageAsync(request.ProfileImageFile, memberProfileImageUrl.ProfileImageUrl, (long) gymId,StorageFolder.Members);
            request.VideoUrl = await _fileService.ReplaceVideoAsync(request.VideoFile, memberProfileImageUrl.VideoUrl, (long)gymId, StorageFolder.Members);
            request.BodyImageUrl1 = await _fileService.ReplaceImageAsync(request.BodyImageFile1, memberProfileImageUrl.BodyImageUrl1, (long)gymId, StorageFolder.Members);
            request.BodyImageUrl2 = await _fileService.ReplaceImageAsync(request.BodyImageFile2, memberProfileImageUrl.BodyImageUrl2, (long)gymId, StorageFolder.Members);
            request.BodyImageUrl3 = await _fileService.ReplaceImageAsync(request.BodyImageFile3, memberProfileImageUrl.BodyImageUrl3, (long)gymId, StorageFolder.Members);
            // =========================================


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
                MembershipStartDate = request.MembershipStartDate,
                MembershipEndDate = request.MembershipEndDate,
                IsActive = request.IsActive,


                // ذخیره مسیرها در دیتابیس
                ProfileImageUrl = request.ProfileImageUrl,
                VideoUrl = request.VideoUrl,
                BodyImageUrl1 = request.BodyImageUrl1,
                BodyImageUrl2 = request.BodyImageUrl2,
                BodyImageUrl3 = request.BodyImageUrl3
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

    public bool IsActive { get; set; }


    // در DTO ها اضافه کنید:
    // فایل‌های ورودی از فرم
    public IFormFile ProfileImageFile { get; set; }
    public IFormFile VideoFile { get; set; }
    public IFormFile BodyImageFile1 { get; set; }
    public IFormFile BodyImageFile2 { get; set; }
    public IFormFile BodyImageFile3 { get; set; }

    // مسیرهایی که بعد از فشرده‌سازی پر می‌شوند
    public string ProfileImageUrl { get; set; }
    public string VideoUrl { get; set; }
    public string BodyImageUrl1 { get; set; }
    public string BodyImageUrl2 { get; set; }
    public string BodyImageUrl3 { get; set; }
}


