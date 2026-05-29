using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Auth
{
    public class RequestRegisterUserDto
    {
        public string FullName { get; set; }

        public string Mobile { get; set; }

        public string Code { get; set; }

        public string BirthDate { get; set; }
        public bool IsUsed { get; set; }

        public long GymId { get; set; }   // ← این خیلی مهم است

    }
}
