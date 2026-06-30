using FitCore.Application.Contexts;
//using FitCore.Application.Interfaces.Contexts;
using FitCore.Application.Interfaces.IGym;
using FitCore.Application.Services.Gyms.Commands.EditGym.FitCore.Application.ViewModels.Gyms;
using FitCore.Application.ViewModels.Gyms;
using FitCore.Common.Dto;

using System.Linq;

namespace FitCore.Application.Services.Gyms.Commands
{
    public class CompleteGymInfoService : ICompleteGymInfoService
    {
        private readonly IDataBaseContext _context;

        public CompleteGymInfoService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(CompleteGymInfoDto dto)
        {
            var gym = _context.Gyms.Find(dto.Id);

            if (gym == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "باشگاه یافت نشد"
                };
            }

            gym.Name = dto.GymName;
            gym.BrandName = dto.BrandName;
            gym.SubDomain = dto.SubDomain;
            gym.PhoneNumber = dto.PhoneNumber;
            gym.Email = dto.Email;
            gym.Website = dto.Website;
            gym.CitiesId = dto.CitiesId;
            gym.Address = dto.Address;
            gym.PostalCode = dto.PostalCode;
            gym.Latitude = dto.Latitude;
            gym.Longitude = dto.Longitude;
            gym.IsActive = dto.IsActive;
            gym.SubscriptionExpireDate = dto.SubscriptionExpireDate;
            gym.MaxMembers = dto.MaxMembers;
            gym.AllowOnlineRegistration = dto.AllowOnlineRegistration;
            gym.OtpExpireSeconds = dto.OtpExpireSeconds;
            gym.MaxOtpRequestPerMinute = dto.MaxOtpRequestPerMinute;

            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "اطلاعات تکمیلی باشگاه ثبت شد"
            };
        }
    }
}