using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.IGym;
using FitCore.Application.Services.Gyms.Commands.AddGym;
using FitCore.Application.Services.Gyms.Commands.EditGym;
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
        /*
        public List<GymDto> GetAll()
        {
            return _context.Gyms.Select(x => new GymDto
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                SubDomain = x.SubDomain,
                BrandName = x.BrandName,
                Description = x.Description,
                Logo = x.Logo,
                PhoneNumber = x.PhoneNumber,
                MobileNumber = x.MobileNumber,
                Email = x.Email,
                Website = x.Website,
                Province = x.Province,
                City = x.City,
                Address = x.Address,
                PostalCode = x.PostalCode,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                IsActive = x.IsActive,
                SubscriptionExpireDate = x.SubscriptionExpireDate,
                MaxMembers = x.MaxMembers,
                AllowOnlineRegistration = x.AllowOnlineRegistration,
                OtpExpireSeconds = x.OtpExpireSeconds,
                MaxOtpRequestPerMinute = x.MaxOtpRequestPerMinute
            }).ToList();
        }

        public GymDto GetById(long id)
        {
            var x = _context.Gyms.Find(id);

            if (x == null) return null;

            return new GymDto
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                SubDomain = x.SubDomain,
                BrandName = x.BrandName,
                Description = x.Description,
                Logo = x.Logo,
                PhoneNumber = x.PhoneNumber,
                MobileNumber = x.MobileNumber,
                Email = x.Email,
                Website = x.Website,
                Province = x.Province,
                City = x.City,
                Address = x.Address,
                PostalCode = x.PostalCode,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                IsActive = x.IsActive,
                SubscriptionExpireDate = x.SubscriptionExpireDate,
                MaxMembers = x.MaxMembers,
                AllowOnlineRegistration = x.AllowOnlineRegistration,
                OtpExpireSeconds = x.OtpExpireSeconds,
                MaxOtpRequestPerMinute = x.MaxOtpRequestPerMinute
            };
        }

        public void Create(CreateGymDto dto)
        {
            var gym = new Gym
            {
                Name = dto.Name,
                Code = dto.Code,
                SubDomain = dto.SubDomain,
                BrandName = dto.BrandName,
                Description = dto.Description,
                Logo = dto.Logo,
                PhoneNumber = dto.PhoneNumber,
                MobileNumber = dto.MobileNumber,
                Email = dto.Email,
                Website = dto.Website,
                Province = dto.Province,
                City = dto.City,
                Address = dto.Address,
                PostalCode = dto.PostalCode,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                IsActive = dto.IsActive,
                SubscriptionExpireDate = dto.SubscriptionExpireDate,
                MaxMembers = dto.MaxMembers,
                AllowOnlineRegistration = dto.AllowOnlineRegistration,
                OtpExpireSeconds = dto.OtpExpireSeconds,
                MaxOtpRequestPerMinute = dto.MaxOtpRequestPerMinute
            };

            _context.Gyms.Add(gym);
            _context.SaveChanges();
        }
        */
        public void Update(UpdateGymDto dto)
        {
            var gym = _context.Gyms.Find(dto.Id);

            if (gym == null) return;

            gym.Name = dto.Name;
            gym.Code = dto.Code;
            gym.SubDomain = dto.SubDomain;
            gym.BrandName = dto.BrandName;
            gym.Description = dto.Description;
            gym.Logo = dto.Logo;
            gym.PhoneNumber = dto.PhoneNumber;
            gym.MobileNumber = dto.MobileNumber;
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

            _context.SaveChanges();
        }

        /*
        public void Delete(long id)
        {
            var gym = _context.Gyms.Find(id);
            if (gym == null) return;

            _context.Gyms.Remove(gym);
            _context.SaveChanges();
        }
        */
    }

}
