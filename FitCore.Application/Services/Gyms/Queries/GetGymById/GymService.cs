using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.IGym;
using FitCore.Application.Services.Gyms.Commands.AddGym;
using FitCore.Domain.Entities.Gyms;

using System.Collections.Generic;
using System.Linq;
namespace FitCore.Application.Services.Gyms.Commands
{
    public class GetGymByIdService : IGetGymByIdService
    {
        private readonly IDataBaseContext _context;

        public GetGymByIdService(IDataBaseContext context)
        {
            _context = context;
        }

        public GymDto GetById(string code)
        {
            var x = _context.Gyms.FirstOrDefault(x => x.Code == code);

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
                //ProvinceID = x.ProvincesId,
                CitiesId = x.CitiesId,
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

    }

}
