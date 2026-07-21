using FitCore.Application.Contexts;
using FitCore.Application.Services.Auth.Dto;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Auth
{
    public class RegisterManagerService
    {
        private readonly IDataBaseContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<RegisterManagerService> _logger;

        public RegisterManagerService(
            IDataBaseContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterManagerService> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<ResultDto> Execute(RequestRegisterManagerDto request)
        {
            try
            {
                request.Mobile = request.Mobile?.Trim();
                request.Code = request.Code?.Trim();
                request.FullName = request.FullName?.Trim();
                request.GymName = request.GymName?.Trim();

                // 1. بررسی OTP
                var otp = await _context.UserOtpCodes
                    .FirstOrDefaultAsync(x => x.PhoneNumber == request.Mobile && x.Code == request.Code && x.IsUsed == false);

                if (otp == null) return new ResultDto { IsSuccess = false, Message = "کد تایید نامعتبر یا منقضی شده است" };

                // 2. ایجاد باشگاه جدید (مطابق با Entity شما)
                // 2. ایجاد باشگاه جدید (مطابق با Entity شما)

                //string uniqueGymCode = string.Empty;
                //bool isUnique = false;
                //Random rnd = new Random();
                //int maxRetries = 10; // برای جلوگیری از لوپ بی‌نهایت در صورت پر بودن دیتابیس

                //// تولید کد یکتا با چک کردن در دیتابیس
                //while (!isUnique && maxRetries > 0)
                //{
                //    // تغییر از 4 رقمی به 6 رقمی برای جلوگیری از تکراری شدن
                //    uniqueGymCode = rnd.Next(100000, 999999).ToString();

                //    // آیا این کد قبلاً در دیتابیس ثبت شده است؟
                //    var exists = await _context.Gyms.AnyAsync(g => g.Code == uniqueGymCode);

                //    if (!exists)
                //    {
                //        isUnique = true; // کد یافت شد، از حلقه خارج می‌شویم
                //    }

                //    maxRetries--;
                //}

                //// اگر به هر دلیلی بعد از 10 بار هم کد یکتا پیدا نشد (بسیار نادر)، از زمان حال سیستم استفاده می‌کنیم
                //if (!isUnique)
                //{
                //    uniqueGymCode = DateTime.Now.ToString("HHmmss"); // مثلا 145230
                //}

                var newGym = new Gym
                {
                    Name = request.GymName,
                    CitiesId = request.CitiesId,
                    MobileNumber = request.Mobile,
                    IsActive = false
                };

                _context.Gyms.Add(newGym);
                await _context.SaveChangesAsync();


                // 3. ساخت کاربر مدیر
                AppUser newUser = new AppUser()
                {
                    FullName = request.FullName,
                    UserName = $"{request.Mobile}_{newGym.Id}",
                    PhoneNumber = request.Mobile,
                    IsActive = true,
                    GymId = newGym.Id
                };

                var createUser = await _userManager.CreateAsync(newUser, "FitCore@123");
                if (!createUser.Succeeded)
                {
                    string errors = string.Join("\n", createUser.Errors.Select(x => x.Description));
                    return new ResultDto { IsSuccess = false, Message = errors };
                }

                // 4. اختصاص نقش Admin (یا نام نقشی که برای مدیر باشگاه تعریف کرده‌اید)
                var addRoleResult = await _userManager.AddToRoleAsync(newUser, "Admin");
                if (!addRoleResult.Succeeded)
                {
                    string errors = string.Join("\n", addRoleResult.Errors.Select(x => x.Description));
                    return new ResultDto { IsSuccess = false, Message = errors };
                }

                // 5. اتمام عملیات
                otp.IsUsed = true;
                await _context.SaveChangesAsync();

                await _signInManager.SignInAsync(newUser, isPersistent: true);

                return new ResultDto { IsSuccess = true, Message = "ثبت نام مدیر باشگاه انجام شد. منتظر تایید نهایی ادمین سیستم باشید." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error In RegisterManagerService");
                return new ResultDto { IsSuccess = false, Message = "خطایی در ثبت نام رخ داده است" };
            }
        }
    }
}