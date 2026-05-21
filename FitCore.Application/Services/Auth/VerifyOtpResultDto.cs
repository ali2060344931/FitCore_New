using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Auth
{
    public class VerifyOtpResultDto : ResultDto
    {
        public bool NeedGymSelection { get; set; }
        public string LoginToken { get; set; }
        public List<GymItemDto> Gyms { get; set; } = new();
    }
}
