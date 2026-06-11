using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Auth
{
    public class RequestRegisterUserDto
    {
        public string FullName { get; set; }
        public string Mobile { get; set; }

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "تکرار رمز عبور الزامی است")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "رمز عبور و تکرار آن برابر نیستند")]
        public string RePassword { get; set; } // فیلد جدید

        public string Code { get; set; } // کد OTP
        public long GymId { get; set; }
    }
}
