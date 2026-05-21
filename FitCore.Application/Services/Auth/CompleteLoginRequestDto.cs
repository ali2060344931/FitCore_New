using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Auth
{
    public class CompleteLoginRequestDto
    {
        public string LoginToken { get; set; }
        public long GymId { get; set; }
    }
}
