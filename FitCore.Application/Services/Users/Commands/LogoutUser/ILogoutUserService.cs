using FitCore.Common.Dto;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Users.Commands.LogoutUser
{
    public  interface ILogoutUserService
    {
        Task<ResultDto> Execute();
    }

    public class LogoutUserService : ILogoutUserService
    {
        private readonly SignInManager<AppUser> _signInManager;

        public LogoutUserService(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }


        public async Task<ResultDto> Execute()
        {
            await _signInManager.SignOutAsync();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "خروج با موفقیت انجام شد"
            };
        }
    }
}
