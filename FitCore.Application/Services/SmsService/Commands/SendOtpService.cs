using FitCore.Application.Interfaces;
using FitCore.Application.Interfaces.Contexts;
using FitCore.Common;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Users;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FitCore.Application.Services.SmsService.Commands
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

        public async Task<ResultDto> Execute(string phoneNumber, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return new ResultDto { IsSuccess = false, Message = "شماره موبایل را وارد کنید" };

            // ✅ Rate Limit (3 بار در دقیقه)
            var oneMinuteAgo = DateTime.Now.AddMinutes(-1);

            var requestCount = _context.UserOtpCodes
                .Count(x => x.PhoneNumber == phoneNumber && x.CreatedAt > oneMinuteAgo);

            if (requestCount >= 3)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "لطفاً کمی صبر کنید"
                };
            }

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

            await _smsService.SendAsync(phoneNumber, $"کد تایید شما: {code}");

            return new ResultDto
            {
                IsSuccess = true,
                Message = "کد ارسال شد"
            };
        }
    }
}
