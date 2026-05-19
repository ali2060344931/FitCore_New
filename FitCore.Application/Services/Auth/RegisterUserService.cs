using FitCore.Application.Interfaces.Contexts;
using FitCore.Application.Services.Auth;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks; // اضافه شود

public class RegisterUserService
{
    private readonly IDataBaseContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<RegisterUserService> _logger; // اضافه شد

    public RegisterUserService(
        IDataBaseContext context,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ILogger<RegisterUserService> logger) // تزریق ILogger
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<ResultDto> Execute(RequestRegisterUserDto request, CancellationToken cancellationToken=default)
    {
        try
        {
            request.Mobile = request.Mobile.Trim();
            request.Code = request.Code.Trim();

            // 1) بررسی کد تایید
            var otp = await _context.UserOtpCodes
                .FirstOrDefaultAsync(x =>
                    x.PhoneNumber == request.Mobile &&
                    x.Code == request.Code &&
                    x.IsUsed == false);

            if (otp == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "کد تایید نامعتبر یا منقضی شده است"
                };
            }

            // 2) بررسی وجود کاربر
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.PhoneNumber == request.Mobile);

            if (user != null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "این شماره موبایل قبلاً ثبت شده است"
                };
            }

            // 3) ✔ بررسی معتبر بودن GymId در همینجا
            var gymExists = await _context.Gyms.AnyAsync(x => x.Id == request.GymId);

            if (!gymExists)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "باشگاه انتخاب‌شده معتبر نیست"
                };
            }

            // 4) ✔ ایجاد کاربر جدید
            AppUser newUser = new AppUser()
            {
                FullName = request.FullName,
                UserName = request.Mobile,
                PhoneNumber = request.Mobile,
                IsActive = true,
                GymId = request.GymId  // مقدار درست اینجا می‌نشیند
            };

            // ساخت کاربر
            var createUser = await _userManager.CreateAsync(newUser, "FitCore@123");

            if (!createUser.Succeeded)
            {
                string errors = string.Join("\n", createUser.Errors.Select(e => e.Description));

                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = errors
                };
            }

            // 5) ثبت مصرف OTP
            otp.IsUsed = true;
            await _context.SaveChangesAsync(cancellationToken);

            // 6) ورود خودکار
            await _signInManager.SignInAsync(newUser, isPersistent: true);

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ثبت نام با موفقیت انجام شد"
            };
        }
        catch (Exception ex)
        {
            return new ResultDto()
            {
                IsSuccess = false,
                Message = $"خطای غیرمنتظره: {ex.Message}"
            };
        }
    }
}
