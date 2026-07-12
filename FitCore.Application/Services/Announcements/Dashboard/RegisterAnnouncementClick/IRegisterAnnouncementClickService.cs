using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Dashboard.RegisterAnnouncementClick
{
    public interface IRegisterAnnouncementClickService
    {
        Task<ResultDto> Execute(
            RequestRegisterAnnouncementClickDto request);
    }
}
