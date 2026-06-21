using FitCore.Application.Contexts;
// فرض میکنیم این اینترفیس را در پروژه Application ساختید تا به سرویس بله دسترسی داشته باشید
using FitCore.Application.Interfaces.ISms;
using FitCore.Common;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Auth
{
    public class SendOtpService
    {
        private readonly IDataBaseContext _context;
        private readonly ISmsService _smsService;
        private readonly IWebHostEnvironment _environment;

        // تزریق سرویس بله
        private readonly IBaleBotService _baleBotService;

        public SendOtpService(
            IDataBaseContext context,
            ISmsService smsService,
            IWebHostEnvironment environment,
            IBaleBotService baleBotService) // اضافه شدن به Constructor
        {
            _context = context;
            _smsService = smsService;
            _environment = environment;
            _baleBotService = baleBotService;
        }

        /// <summary>
        /// برای بخش ثبت نام کاربر
        /// </summary>
        public async Task<ResultDto> Execute(string FullName, string phoneNumber, int gymId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(FullName))
                return new ResultDto { IsSuccess = false, Message = "نـــــام و نام خانوادگی را وارد نمائید" };

            if (string.IsNullOrWhiteSpace(phoneNumber))
                return new ResultDto { IsSuccess = false, Message = "شماره موبایل را وارد کنید" };

            if (gymId == 0)
                return new ResultDto { IsSuccess = false, Message = "هیچ باشگاهی انتخاب نشد" };

            var oneMinuteAgo = DateTime.Now.AddMinutes(-1);
            var requestCount = await _context.UserOtpCodes
                .CountAsync(x => x.PhoneNumber == phoneNumber && x.CreatedAt > oneMinuteAgo, cancellationToken);

            if (requestCount >= 3)
                return new ResultDto { IsSuccess = false, Message = "تعداد درخواست‌های شما بیش از حد مجاز است، لطفاً کمی صبر کنید." };

            var code = OtpGenerator.Generate();

            var otp = new UserOtpCode
            {
                PhoneNumber = phoneNumber,
                Code = code,
                CreatedAt = DateTime.Now,
                ExpireTime = DateTime.Now.AddMinutes(2),
                IsUsed = false
            };

            _context.UserOtpCodes.Add(otp);
            await _context.SaveChangesAsync(cancellationToken);

            // 4. ارسال پیامک
            await _smsService.SendAsync(phoneNumber, $"کد تایید شما: {code}");

            // ==========================================
            // 5. ارسال همزمان به ربات بله (Fire and Forget)
            // ==========================================
            await SendOtpToBaleAsync(phoneNumber, code, cancellationToken);

            return new ResultDto { IsSuccess = true, Message = "کد تایید با موفقیت ارسال شد." };
        }


        /// <summary>
        /// برای بخش ورود کاربر
        /// </summary>
        public async Task<ResultDto<string>> Execute(string phoneNumber, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return new ResultDto<string> { IsSuccess = false, Message = "شماره موبایل را وارد کنید" };

            var oneMinuteAgo = DateTime.Now.AddMinutes(-1);
            var requestCount = await _context.UserOtpCodes
                .CountAsync(x => x.PhoneNumber == phoneNumber && x.CreatedAt > oneMinuteAgo, cancellationToken);

            if (requestCount >= 3)
                return new ResultDto<string> { IsSuccess = false, Message = "تعداد درخواست‌های شما بیش از حد مجاز است، لطفاً کمی صبر کنید." };

            var code = OtpGenerator.Generate();
            var otp = new UserOtpCode
            {
                PhoneNumber = phoneNumber,
                Code = code,
                CreatedAt = DateTime.Now,
                ExpireTime = DateTime.Now.AddMinutes(2),
                IsUsed = false
            };

            _context.UserOtpCodes.Add(otp);
            await _context.SaveChangesAsync(cancellationToken);

            // 4. ارسال پیامک
            await _smsService.SendAsync(phoneNumber, $"کد تایید شما: {code}");

            // ==========================================
            // 5. ارسال همزمان به ربات بله (Fire and Forget)
            // ==========================================
            await SendOtpToBaleAsync(phoneNumber, code, cancellationToken);

            if (_environment.IsDevelopment())
            {
                return new ResultDto<string> { IsSuccess = true, Message = "کد تایید با موفقیت ارسال شد.", Data = code };
            }
            else
            {
                return new ResultDto<string> { IsSuccess = true, Message = "کد تایید با موفقیت ارسال شد.", Data = null };
            }
        }


        // ==========================================
        // متد کمکی برای ارسال به بله
        // ==========================================
        private async Task SendOtpToBaleAsync(string phoneNumber, string code, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);

                // تغییر این خط: ابتدا چک میکنیم که مقدار داشته باشد (HasValue) و بعد بزرگتر از صفر باشد
                if (user != null && user.BaleChatId.HasValue && user.BaleChatId.Value > 0)
                {
                    string message = $"🔐 کد تایید شما: {code}\nاین کد را برای ورود به پنل وارد کنید.";

                    // تغییر این خط: استفاده از user.BaleChatId.Value برای تبدیل long? به long
                    _ = _baleBotService.SendMessageAsync(user.BaleChatId.Value, message);
                }
            }
            catch (Exception ex)
            {
                // نادیده گرفتن خطا
            }
        }
    }
}