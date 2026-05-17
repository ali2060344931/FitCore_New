using FitCore.Application.Services.Users.Commands.LoginUser;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Users;

using FluentValidation;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly SignInManager<AppUser> _signInManager;

        public RegisterUserService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ResultDto> Execute(RegisterUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
                return new ResultDto { IsSuccess = false, Message = "نام و نام خانوادگی را وارد کنید" };

            if (string.IsNullOrWhiteSpace(request.Email))
                return new ResultDto { IsSuccess = false, Message = "ایمیل را وارد کنید" };

            if (string.IsNullOrWhiteSpace(request.Password))
                return new ResultDto { IsSuccess = false, Message = "رمز عبور را وارد کنید" };

            if (request.Password != request.RePassword)
                return new ResultDto { IsSuccess = false, Message = "رمز عبور و تکرار آن یکسان نیست" };

            var userExist = await _userManager.FindByEmailAsync(request.Email);

            if (userExist != null)
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این ایمیل قبلاً ثبت شده است"
                };

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
                var errors = result.Errors.Select(x => MapIdentityError(x.Description)).ToList();

                return new ResultDto
                {
                    IsSuccess = false,
                    Message = string.Join(" - ", errors)
                };
            }

            return new ResultDto
            {
                IsSuccess = true,
                Message = "ثبت نام با موفقیت انجام شد"
            };
        }


        private string MapIdentityError(string error)
        {
            if (error.Contains("Passwords must have at least one non alphanumeric character"))
                return "رمز عبور باید حداقل یک کاراکتر خاص مانند !@@#$%^&* داشته باشد";

            if (error.Contains("Passwords must have at least one lowercase"))
                return "رمز عبور باید حداقل یک حرف کوچک انگلیسی داشته باشد";

            if (error.Contains("Passwords must have at least one uppercase"))
                return "رمز عبور باید حداقل یک حرف بزرگ انگلیسی داشته باشد";

            if (error.Contains("Passwords must have at least one digit"))
                return "رمز عبور باید حداقل یک عدد داشته باشد";

            if (error.Contains("Email"))
                return "ایمیل وارد شده معتبر نیست";

            return error;
        }

        
    }
}
