using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Auth
{
    public class VerifyOtpService
    {
        private readonly IDataBaseContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILoginTokenStore _loginTokenStore;

        public VerifyOtpService(
            IDataBaseContext db,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILoginTokenStore loginTokenStore)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _loginTokenStore = loginTokenStore;
        }

        public async Task<VerifyOtpResultDto> Execute(string mobile, string code, CancellationToken cancellationToken = default)
        {
            mobile = mobile?.Trim();
            code = code?.Trim();

            // 1) اعتبارسنجی OTP (طبق ساختار خودت)
            var otp = await _db.UserOtpCodes
                .FirstOrDefaultAsync(x => x.PhoneNumber == mobile && x.Code == code && x.IsUsed == false, cancellationToken=default);

            if (otp == null)
            {
                return new VerifyOtpResultDto
                {
                    IsSuccess = false,
                    Message = "کد تایید نامعتبر یا منقضی شده است"
                };
            }

            otp.IsUsed = true;
            otp.CreatedAt = DateTime.Now;

            await _db.SaveChangesAsync(cancellationToken);

            // (اختیاری) اگر expire داری اینجا چک کن

            // 2) پیدا کردن تمام اکانت‌های این موبایل
            var accounts = await _userManager.Users
                .Where(x => x.PhoneNumber == mobile && x.IsActive)
                .Select(x => new { x.Id, x.GymId })
                .ToListAsync(cancellationToken);

            if (accounts.Count == 0)
            {
                return new VerifyOtpResultDto
                {
                    IsSuccess = false,
                    Message = "کاربری با این شماره یافت نشد"
                };
            }

            // 3) اگر فقط یک باشگاه: ورود مستقیم
            if (accounts.Count == 1)
            {
                var user = await _userManager.FindByIdAsync(accounts[0].Id.ToString());
                await _signInManager.SignInAsync(user, isPersistent: true);

                // مصرف OTP
                otp.IsUsed = true;
                await _db.SaveChangesAsync(cancellationToken);

                return new VerifyOtpResultDto
                {
                    IsSuccess = true,
                    Message = "ورود با موفقیت انجام شد",
                    NeedGymSelection = false
                };
            }

            // 4) چند باشگاه: لیست باشگاه‌ها را بده + یک توکن کوتاه‌عمر
            var gymIds = accounts.Select(x => x.GymId).Distinct().ToList();

            var gyms = await _db.Gyms
                .Where(g => gymIds.Contains(g.Id) && g.IsActive)
                .Select(g => new GymItemDto { Id = g.Id, Name = g.Name })
                .ToListAsync(cancellationToken);

            // توکن انتخاب باشگاه (2 دقیقه)
            var token = await _loginTokenStore.CreateAsync(mobile, TimeSpan.FromMinutes(2), cancellationToken);

            // توجه: OTP را اینجا مصرف نمی‌کنیم تا بعد از انتخاب باشگاه مصرف شود
            // یا می‌توانی همینجا مصرف کنی؛ ولی آن‌وقت اگر کاربر انتخاب را انجام نداد، OTP سوخته می‌شود.

            return new VerifyOtpResultDto
            {
                IsSuccess = true,
                Message = "چند باشگاه برای این شماره یافت شد. لطفاً باشگاه را انتخاب کنید.",
                NeedGymSelection = true,
                LoginToken = token,
                Gyms = gyms
            };
        }

        public async Task<ResultDto> CompleteLogin(string loginToken, long gymId, CancellationToken cancellationToken = default)
        {
            loginToken = loginToken?.Trim();

            var mobile = await _loginTokenStore.GetMobileAsync(loginToken, cancellationToken);
            if (string.IsNullOrWhiteSpace(mobile))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "توکن ورود معتبر نیست یا منقضی شده است. دوباره تلاش کنید."
                };
            }

            // پیدا کردن کاربر مربوط به همان باشگاه
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.PhoneNumber == mobile && x.GymId == gymId && x.IsActive, cancellationToken);

            if (user == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "اکانت این شماره در باشگاه انتخاب‌شده یافت نشد."
                };
            }

            await _signInManager.SignInAsync(user, isPersistent: true);

            // توکن یکبارمصرف
            await _loginTokenStore.RemoveAsync(loginToken, cancellationToken);

            // اگر می‌خواهی OTP را هم اینجا مصرف کنی:
            // (نیازمند نگه داشتن otp در مرحله قبل یا چک دوباره‌اش با code؛ برای سادگی فعلاً اینجا مصرف نکردیم)

            return new ResultDto
            {
                IsSuccess = true,
                Message = "ورود با موفقیت انجام شد"
            };
        }
    }
}
