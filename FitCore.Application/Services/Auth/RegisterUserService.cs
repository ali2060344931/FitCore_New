using FitCore.Application.Interfaces.Contexts;
using FitCore.Application.Services.Auth;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
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

    public async Task<ResultDto> Execute(RequestRegisterUserDto request)
    {
        try
        {
            request.Mobile = request.Mobile.Trim();
            request.Code = request.Code.Trim();

            // بررسی کد تایید
            var otp =await _context.UserOtpCodes
                .FirstOrDefaultAsync(x =>
                    x.PhoneNumber == request.Mobile &&
                    x.Code == request.Code &&
                    x.IsUsed == false /*&& x.ExpireTime > DateTime.Now*/);

            if (otp == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "کد تایید نامعتبر یا منقضی شده است"
                };
            }

            // بررسی وجود کاربر
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
            
            // ایجاد کاربر جدید
            AppUser newUser = new AppUser()
            {
                FullName = request.FullName,
                UserName = request.Mobile,
                PhoneNumber = request.Mobile,
                IsActive = true,
                GymId = 2
            };

            // ✅ ساخت کاربر در Identity با try-catch دقیق
            IdentityResult createUser;

            try
            {
                createUser = await _userManager.CreateAsync(newUser, "FitCore@123");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception در CreateAsync");

                var innerMessage = ex.InnerException?.Message ?? "No inner exception";

                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = $"خطای داخلی سرور: {ex.Message} | Inner: {innerMessage}"
                };
            }

            if (!createUser.Succeeded)
            {
                string errors = string.Join("\n", createUser.Errors.Select(e => e.Description));
                _logger.LogError("❌ Identity Errors: {Errors}", errors);

                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = errors
                };
            }

            // ثبت مصرف OTP
            otp.IsUsed = true;
            _context.SaveChanges();

            // ورود خودکار
            await _signInManager.SignInAsync(newUser, isPersistent: true);

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ثبت نام با موفقیت انجام شد"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Exception کلی در RegisterUserService");

            return new ResultDto()
            {
                IsSuccess = false,
                Message = $"خطای غیرمنتظره: {ex.Message}"
            };
        }
    }
}
