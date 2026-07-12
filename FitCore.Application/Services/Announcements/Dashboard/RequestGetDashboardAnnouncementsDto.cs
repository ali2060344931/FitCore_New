using System.Collections.Generic;

namespace FitCore.Application.Services.Announcements.Dashboard.GetDashboardAnnouncements
{
    public class RequestGetDashboardAnnouncementsDto
    {
        public long UserId { get; set; }

        public long? GymId { get; set; }

        public List<long> RoleIds { get; set; } = new();

    }
}