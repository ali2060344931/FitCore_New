using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Common.Roles;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Auth
{
    public class RegisterUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IDataBaseContext _db;

        public RegisterUserService(
            UserManager<AppUser> userManager,
            IDataBaseContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<ResultDto> Execute(RequestRegisterUserDto request)
        {
            // ۱. بررسی برابری پسوردها در لایه سرویس
            if (request.Password != request.RePassword)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "رمز عبور و تکرار آن مطابقت ندارند."
                };
            }

            // ۲. بررسی وجود کاربر (کد قبلی شما)
            var exists = await _userManager.Users.AnyAsync(u => u.Mobile == request.Mobile);
            if (exists)
                return new ResultDto { IsSuccess = false, Message = "این شماره قبلاً ثبت شده است." };

            // ۳. ساخت کاربر با Identity
            var user = new AppUser
            {
                FullName = request.FullName,
                UserName = request.Mobile,
                Mobile = request.Mobile,
                GymId = request.GymId,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");
                return new ResultDto { IsSuccess = true, Message = "خوش آمدید! ثبت‌نام انجام شد." };
            }

            return new ResultDto
            {
                IsSuccess = false,
                Message = string.Join("\n", result.Errors.Select(e => e.Description))
            };
        }
    }

    /*

    public async Task<ResultDto> Execute(
        RequestRegisterUserDto request)
    {
        try
        {
            // Trim

            request.Mobile =
                request.Mobile?.Trim();

            request.Code =
                request.Code?.Trim();

            request.FullName =
                request.FullName?.Trim();

            // بررسی OTP

            var otp = await _context.UserOtpCodes
                .FirstOrDefaultAsync(x =>
                    x.PhoneNumber == request.Mobile &&
                    x.Code == request.Code &&
                    x.IsUsed == false
                );

            if (otp == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "کد تایید نامعتبر یا منقضی شده است"
                };
            }

            // بررسی انتخاب باشگاه

            if (request.GymId == 0)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "باشگاه انتخاب نشده است"
                };
            }

            // بررسی وجود باشگاه

            var gymExists = await _context.Gyms
                .AnyAsync(x => x.Id == request.GymId);

            if (!gymExists)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "باشگاه انتخاب شده معتبر نیست"
                };
            }

            // بررسی ثبت نام قبلی در همان باشگاه

            var userExistsInGym = await _userManager.Users
                .AnyAsync(x =>
                    x.PhoneNumber == request.Mobile &&
                    x.GymId == request.GymId
                );

            if (userExistsInGym)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "شما قبلاً در این باشگاه ثبت نام کرده اید"
                };
            }

            // ساخت کاربر جدید

            AppUser newUser = new AppUser()
            {
                FullName = request.FullName,
                UserName = $"{request.Mobile}_{request.GymId}",
                PhoneNumber = request.Mobile,
                IsActive = true,
                GymId = request.GymId
            };

            // ذخیره User

            var createUser = await _userManager
                .CreateAsync(newUser, "FitCore@123");

            if (!createUser.Succeeded)
            {
                string errors = string.Join(
                    "\n",
                    createUser.Errors.Select(x => x.Description)
                );

                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = errors
                };
            }

            // افزودن Role

            var addRoleResult = await _userManager
                .AddToRoleAsync(newUser, UserRoles.Member);

            if (!addRoleResult.Succeeded)
            {
                string errors = string.Join(
                    "\n",
                    addRoleResult.Errors.Select(x => x.Description)
                );

                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = errors
                };
            }




            // ایجاد رکورد Member

            var member = new Domain.Entities.Members.Member()
            {
                AppUserId = newUser.Id,
                IsActive = true,
            };
            await _context.Members.AddAsync(member);



            // ثبت مصرف OTP

            otp.IsUsed = true;

            // ذخیره نهایی

            await _context.SaveChangesAsync();

            // Login خودکار

            await _signInManager.SignInAsync(
                newUser,
                isPersistent: true);

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ثبت نام با موفقیت انجام شد"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error In RegisterUserService");

            return new ResultDto()
            {
                IsSuccess = false,
                Message = "خطایی در ثبت نام رخ داده است"
            };
        }
    }
    */
}

