using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.IGym;
using FitCore.Application.ViewModels.Gyms;
using FitCore.Common.Dto;
using FitCore.Common.Roles;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Gyms.Commands.AddGym
{
    public class AddGymService : IAddGymService
    {
        private readonly IDataBaseContext _context;

        private readonly UserManager<AppUser> _userManager;

        public AddGymService(
            IDataBaseContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;

            _userManager = userManager;
        }

        public async Task<ResultDto> Execute(CreateGymDto request)
        {
            try
            {
                request.Name =
                    request.Name?.Trim();

                request.Code =request.Code;

                request.MobileNumber =
                    request.MobileNumber?.Trim();

                request.FullName =
                    request.FullName?.Trim();

                // بررسی تکراری بودن کد باشگاه
                var gymCodeExists =
                    await _context.Gyms
                    .AnyAsync(x => x.Code == request.Code);

                if (gymCodeExists)
                {
                    return new ResultDto()
                    {
                        IsSuccess = false,

                        Message =
                            "کد باشگاه تکراری است"
                    };
                }

                // بررسی تکراری بودن موبایل مدیر

                var userExists =
                    await _userManager.Users
                    .AnyAsync(x =>
                        x.PhoneNumber ==
                        request.MobileNumber);

                if (userExists)
                {
                    return new ResultDto()
                    {
                        IsSuccess = false,

                        Message =
                            "این شماره موبایل قبلاً ثبت شده است"
                    };
                }


                // ایجاد باشگاه

                var gym = new Gym()
                {
                    Name = request.Name,

                    Code = request.Code,

                    Description =
                        request.Description,

                    MobileNumber =
                        request.MobileNumber,

                    IsActive = true,

                };

                _context.Gyms.Add(gym);

                await _context.SaveChangesAsync(default);

                // ایجاد مدیر باشگاه

                var adminUser = new AppUser()
                {
                    FullName = request.FullName,

                    UserName =
                        $"{request.MobileNumber}_{gym.Id}",

                    PhoneNumber =
                        request.MobileNumber,

                    IsActive = true,

                    GymId = gym.Id
                };

                // ثبت User

                var createUserResult =
                    await _userManager
                    .CreateAsync(
                        adminUser,
                        "FitCore@123");

                if (!createUserResult.Succeeded)
                {
                    string errors = string.Join(
                        "\n",
                        createUserResult.Errors
                        .Select(x => x.Description));

                    return new ResultDto()
                    {
                        IsSuccess = false,

                        Message = errors
                    };
                }

                // ثبت Role

                var roleResult =
                    await _userManager
                    .AddToRoleAsync(
                        adminUser,
                        UserRoles.Admin);

                if (!roleResult.Succeeded)
                {
                    string errors = string.Join(
                        "\n",
                        roleResult.Errors
                        .Select(x => x.Description));

                    return new ResultDto()
                    {
                        IsSuccess = false,

                        Message = errors
                    };
                }

                return new ResultDto()
                {
                    IsSuccess = true,

                    Message =
                        "باشگاه و مدیر باشگاه با موفقیت ثبت شدند"
                };
            }
            catch (Exception ex)
            {
                return new ResultDto()
                {
                    IsSuccess = false,

                    Message =
                        ex.Message
                };
            }
        }
    }
}