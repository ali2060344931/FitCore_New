using System;

namespace FitCore.Application.Services.Announcements.Queries.GetAnnouncements
{
    public class RequestGetAnnouncementsDto
    {
        public string SearchKey { get; set; }

        public bool? IsActive { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}
