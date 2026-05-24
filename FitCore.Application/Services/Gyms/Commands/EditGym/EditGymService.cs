using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.IGym;
using FitCore.Application.Services.Gyms.Commands.AddGym;
using FitCore.Application.Services.Gyms.Commands.EditGym;
using FitCore.Application.ViewModels.Gyms;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Gyms;

using System.Collections.Generic;
using System.Linq;
namespace FitCore.Application.Services.Gyms.Commands
{
    public class EditGymService : IEditGymService
    {
        private readonly IDataBaseContext _context;

        public EditGymService(IDataBaseContext context)
        {
            _context = context;
        }
        
        //public void Execute(UpdateGymDto dto)
 public ResultDto Execute(UpdateGymDto dto)
        {
            var gym = _context.Gyms.Find(dto.Id);

            if (gym == null)
                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "باشگاهی یافت نشد"
                };


            gym.Name = dto.Name;
            gym.Code = dto.Code;
            gym.MobileNumber = dto.MobileNumber;
            gym.Description = dto.Description;


            /*
            gym.SubDomain = dto.SubDomain;
            gym.BrandName = dto.BrandName;
            gym.Logo = dto.Logo;
            gym.PhoneNumber = dto.MobileNumber;
            gym.Email = dto.Email;
            gym.Website = dto.Website;
            gym.Province = dto.Province;
            gym.Cities = dto.Cities;
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
            */
            _context.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true,
                Message = "باشگاه با موفقیت ثبت شد"
            };

        }

    }

}
