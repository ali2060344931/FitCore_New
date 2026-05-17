using FitCore.Common.Dto;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Users.Commands.LoginUser
{
    public interface ILoginUserService
    {
        Task<ResultDto> Execute(LoginUserRequest request);
    }

    public class LoginUserService : ILoginUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public LoginUserService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ResultDto> Execute(LoginUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "ایمیل و رمز عبور را وارد کنید"
                };
            }

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
                    Message = "حساب کاربری شما غیرفعال است"
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                request.Password,
                lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "رمز عبور اشتباه است"
                };
            }

            // ✅ ساخت Claims سفارشی
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("GymId", user.GymId.ToString())
            };

            // اضافه کردن Role
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(
                claims,
                IdentityConstants.ApplicationScheme);

            var principal = new ClaimsPrincipal(identity);

            await _signInManager.Context.SignInAsync(
                IdentityConstants.ApplicationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = request.RememberMe
                });

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
