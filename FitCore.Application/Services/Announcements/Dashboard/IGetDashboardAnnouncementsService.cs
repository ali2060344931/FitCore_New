using FitCore.Application.Services.Announcements.Dashboard.GetDashboardAnnouncements;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Dashboard
{
    public interface IGetDashboardAnnouncementsService
    {
        Task<ResultDto<ResultGetDashboardAnnouncementDto>> Execute(
            RequestGetDashboardAnnouncementsDto request);
    }
}
