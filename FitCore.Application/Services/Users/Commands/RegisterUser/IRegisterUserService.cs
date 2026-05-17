using FitCore.Application.Services.Users.Commands.LoginUser;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Users;

using FluentValidation;

using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static FitCore.Application.Services.Users.Commands.RegisterUser.RegisterUserService;

namespace FitCore.Application.Services.Users.Commands.RegisterUser
{
    public interface IRegisterUserService
    {
        Task<ResultDto> Execute(RegisterUserRequest request);
    }

    public partial class RegisterUserService : IRegisterUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterUserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResultDto> Execute(RegisterUserRequest request)
        {
            var userExist = await _userManager.FindByEmailAsync(request.Email);

            if (userExist != null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این ایمیل قبلاً ثبت شده است"
                };
            }

            var user = new AppUser
            {
                Email = request.Email,
                UserName = request.Email,
                FullName = request.FullName,
                GymId = request.GymId,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                //return new ResultDto
                //{
                //    IsSuccess = false,
                //    Message = "خطا در ثبت کاربر"
                //};
                if (!result.Succeeded)
                {
                    return new ResultDto
                    {
                        IsSuccess = false,
                        Message = string.Join(" | ", result.Errors.Select(x => x.Description))
                    };
                }
            }

            return new ResultDto
            {
                IsSuccess = true,
                Message = "ثبت نام با موفقیت انجام شد"
            };
        }

        public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
        {
            public RegisterUserValidator()
            {
                RuleFor(x => x.FullName)
                    .NotEmpty()
                    .WithMessage("نام الزامی است");

                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .WithMessage("ایمیل معتبر نیست");

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .MinimumLength(6)
                    .WithMessage("رمز عبور باید حداقل 6 کاراکتر باشد");

                RuleFor(x => x.RePassword)
                    .Equal(x => x.Password)
                    .WithMessage("رمز عبور مطابقت ندارد");
            }


        }

    }
}
