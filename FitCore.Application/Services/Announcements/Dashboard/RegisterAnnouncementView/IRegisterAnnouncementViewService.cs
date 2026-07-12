using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Dashboard.RegisterAnnouncementView
{
    public interface IRegisterAnnouncementViewService
    {
        Task<ResultDto> Execute(
            RequestRegisterAnnouncementViewDto request);
    }
}
