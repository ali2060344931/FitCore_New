using System.ComponentModel.DataAnnotations;

namespace FitCore.Application.Services.Auth.Dto
{
    public class RequestRegisterManagerDto
    {
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Code { get; set; } // کد پیامک OTP
        public string GymName { get; set; }

        // مطابق با فیلد existing در کلاس Gym شما
        public int CitiesId { get; set; }
    }
}