using FitCore.Application.Interfaces.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Auth
{
    public class VerifyOtpService
    {
        private readonly IDataBaseContext _context;
        private readonly UserManager<AppUser> _userManager;
        // ✅ اضافه کردن SignInManager
        private readonly SignInManager<AppUser> _signInManager;

        public VerifyOtpService(
            IDataBaseContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ResultDto> Execute(
            string mobile,
            string code, CancellationToken cancellationToken = default)
        {
            try
            {
                mobile = mobile.Trim();
                code = code.Trim();

                var otp = await _context.UserOtpCodes
                    .FirstOrDefaultAsync(x => x.PhoneNumber == mobile && x.Code == code && x.IsUsed == false, cancellationToken);

                if (otp == null)
                {
                    return new ResultDto { IsSuccess = false, Message = "کد تأیید نامعتبر یا منقضی شده است" };
                }

                var user = await _userManager.Users
                    .FirstOrDefaultAsync(x => x.PhoneNumber == mobile, cancellationToken);

                if (user == null)
                {
                    return new ResultDto { IsSuccess = false, Message = "کاربری با این شماره یافت نشد" };
                }

                if (!user.IsActive)
                {
                    return new ResultDto { IsSuccess = false, Message = "حساب شما غیرفعال است" };
                }

                // مصرف کردن کد
                otp.IsUsed = true;
                await _context.SaveChangesAsync(cancellationToken);

                // ✅ لاگین استاندارد با Identity
                // این متد خودش تمام کوکی‌ها و Claimهای لازم (UserName, Id و ...) را می‌سازد
                await _signInManager.SignInAsync(user, isPersistent: true);

                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "ورود با موفقیت انجام شد"
                };
            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = $"خطای داخلی سرور: {ex.Message}"
                };
            }
        }
    }
}
