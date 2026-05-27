using FitCore.Application.Contexts;
using FitCore.Application.Services.Member.Queries;
using FitCore.Common.Dto;
using FitCore.Common.Roles;
using FitCore.Domain.Entities.Members;
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
        private readonly IDataBaseContext _context;

        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly ILogger<RegisterUserService> _logger;

        public RegisterUserService(
            IDataBaseContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterUserService> logger)
        {
            _context = context;

            _userManager = userManager;

            _signInManager = signInManager;

            _logger = logger;
        }

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

                        Message =
                            "کد تایید نامعتبر یا منقضی شده است"
                    };
                }

                // بررسی انتخاب باشگاه

                if (request.GymId == 0)
                {
                    return new ResultDto()
                    {
                        IsSuccess = false,

                        Message =
                            "باشگاه انتخاب نشده است"
                    };
                }

                // بررسی وجود باشگاه

                var gymExists =
                    await _context.Gyms
                    .AnyAsync(x => x.Id == request.GymId);

                if (!gymExists)
                {
                    return new ResultDto()
                    {
                        IsSuccess = false,

                        Message =
                            "باشگاه انتخاب شده معتبر نیست"
                    };
                }

                // بررسی ثبت نام قبلی در همان باشگاه

                var userExistsInGym =
                    await _userManager.Users
                    .AnyAsync(x =>

                        x.PhoneNumber == request.Mobile &&

                        x.GymId == request.GymId
                    );

                if (userExistsInGym)
                {
                    return new ResultDto()
                    {
                        IsSuccess = false,

                        Message =
                            "شما قبلاً در این باشگاه ثبت نام کرده اید"
                    };
                }

                // ساخت کاربر جدید

                AppUser newUser = new AppUser()
                {
                    FullName = request.FullName,

                    UserName =
                        $"{request.Mobile}_{request.GymId}",

                    PhoneNumber = request.Mobile,

                    IsActive = true,

                    GymId = request.GymId
                };

                // ذخیره User

                var createUser =
                    await _userManager
                    .CreateAsync(
                        newUser,
                        "FitCore@123");

                if (!createUser.Succeeded)
                {
                    string errors = string.Join(
                        "\n",
                        createUser.Errors
                        .Select(x => x.Description)
                    );

                    return new ResultDto()
                    {
                        IsSuccess = false,

                        Message = errors
                    };
                }

                // افزودن Role

                var addRoleResult =
                    await _userManager
                    .AddToRoleAsync(
                        newUser,
                        UserRoles.Member);

                if (!addRoleResult.Succeeded)
                {
                    string errors = string.Join(
                        "\n",
                        addRoleResult.Errors
                        .Select(x => x.Description)
                    );

                    return new ResultDto()
                    {
                        IsSuccess = false,

                        Message = errors
                    };
                }

                // ثبت Member

                //var member = new GetMemberByIdDto()
                //{
                    
                    
                //    UserId = newUser.Id,

                //    GymId = request.GymId,

                //    Mobile = request.Mobile,

                //    RegisterDate = DateTime.Now,

                //    IsActive = true
                //};

                //_context.Members.Add(member);




                // ثبت مصرف OTP

                otp.IsUsed = true;

                // ذخیره نهایی

                await _context.SaveChangesAsync();

                // Login خودکار

                await _signInManager
                    .SignInAsync(
                        newUser,
                        isPersistent: true);

                return new ResultDto()
                {
                    IsSuccess = true,

                    Message =
                        "ثبت نام با موفقیت انجام شد"
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

                    Message =
                        "خطایی در ثبت نام رخ داده است"
                };
            }
        }
    }
}