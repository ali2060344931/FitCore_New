using FitCore.Domain.Entities.Gyms;

using System.ComponentModel.DataAnnotations;

namespace FitCore.Application.Services.Auth
{
    public class RequestRegisterUserDto
    {
        public string FullName { get; set; }
        public string Mobile { get; set; }

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string RePassword { get; set; } // .......فیلد جدید

        [Required(ErrorMessage = "تکرار رمز عبور الزامی است")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "رمز عبور و تکرار آن برابر نیستند")]

        public string Code { get; set; } // کد OTP
        public long GymId { get; set; }
        public Gym gym { get; set; }
    }
}
