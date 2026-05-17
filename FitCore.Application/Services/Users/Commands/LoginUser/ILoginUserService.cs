using FitCore.Common.Dto;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Users.Commands.LoginUser
{
    public interface ILoginUserService
    {
        Task<ResultDto> Execute(LoginUserRequest request);
    }


    public class LoginUserService : ILoginUserService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public LoginUserService(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<ResultDto> Execute(LoginUserRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کاربری با این ایمیل یافت نشد"
                };
            }

            if (!user.IsActive)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "حساب کاربری غیرفعال است"
                };
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                request.Password,
                request.RememberMe,
                lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "ایمیل یا رمز عبور اشتباه است"
                };
            }

            return new ResultDto
            {
                IsSuccess = true,
                Message = "ورود با موفقیت انجام شد"
            };
        }
    }


    public class LoginUserRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
