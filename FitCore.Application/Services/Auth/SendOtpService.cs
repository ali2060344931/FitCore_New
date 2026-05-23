using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.ISms;
using FitCore.Common;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Users;

using Microsoft.EntityFrameworkCore; // اضافه شده برای استفاده از AnyAsync

using System;
using System.Threading;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Auth
{
    public class SendOtpService
    {
        private readonly IDataBaseContext _context;
        private readonly ISmsService _smsService;

        public SendOtpService(IDataBaseContext context, ISmsService smsService)
        {
            _context = context;
            _smsService = smsService;
        }

        /// <summary>
        /// برای بخش ثبت نام کاربر
        /// </summary>
        /// <param name="FullName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="gymId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultDto> Execute(string FullName, string phoneNumber,int gymId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(FullName))
                return new ResultDto { IsSuccess = false, Message = "نـــــام و نام خانوادگی را وارد نمائید" };
            
            
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return new ResultDto { IsSuccess = false, Message = "شماره موبایل را وارد کنید" };
            
            
            if (gymId==0)
                return new ResultDto { IsSuccess = false, Message = "هیچ باشگاهی انتخاب نشد" };

            // 2. بررسی Rate Limit (3 بار در دقیقه)
            var oneMinuteAgo = DateTime.Now.AddMinutes(-1);

            var requestCount = await _context.UserOtpCodes
                .CountAsync(x => x.PhoneNumber == phoneNumber && x.CreatedAt > oneMinuteAgo, cancellationToken);

            if (requestCount >= 3)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "تعداد درخواست‌های شما بیش از حد مجاز است، لطفاً کمی صبر کنید."
                };
            }

            // 3. تولید و ذخیره کد OTP
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

            return new ResultDto
            {
                IsSuccess = true,
                Message = "کد تایید با موفقیت ارسال شد."
            };
        }
        /// <summary>
        /// برای بخش ورود کاربر
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultDto> Execute( string phoneNumber, CancellationToken cancellationToken = default)
        {
            
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return new ResultDto { IsSuccess = false, Message = "شماره موبایل را وارد کنید" };
            
            
            // 2. بررسی Rate Limit (3 بار در دقیقه)
            var oneMinuteAgo = DateTime.Now.AddMinutes(-1);

            var requestCount = await _context.UserOtpCodes
                .CountAsync(x => x.PhoneNumber == phoneNumber && x.CreatedAt > oneMinuteAgo, cancellationToken);

            if (requestCount >= 3)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "تعداد درخواست‌های شما بیش از حد مجاز است، لطفاً کمی صبر کنید."
                };
            }

            // 3. تولید و ذخیره کد OTP
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

            return new ResultDto
            {
                IsSuccess = true,
                Message = "کد تایید با موفقیت ارسال شد."
            };
        }
    }
}
