using FitCore.Domain.Entities.Provinces;

using System;
using System.ComponentModel.DataAnnotations;
namespace FitCore.Application.Services.Gyms.Commands.EditGym
{
    public class UpdateGymDto
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public string SubDomain { get; set; }

        public string BrandName { get; set; }

        public string Description { get; set; }

        public string Logo { get; set; }

        public string PhoneNumber { get; set; }

        public string MobileNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Website { get; set; }

        public string Province { get; set; }

        public Cities Cities { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public bool IsActive { get; set; }

        public DateTime? SubscriptionExpireDate { get; set; }

        public int MaxMembers { get; set; }

        public bool AllowOnlineRegistration { get; set; }

        public int OtpExpireSeconds { get; set; }

        public int MaxOtpRequestPerMinute { get; set; }
    }

}
