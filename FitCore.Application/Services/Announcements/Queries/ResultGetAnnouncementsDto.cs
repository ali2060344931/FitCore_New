using FitCore.Domain.Entities.Announcements;

using System;
using System.Collections.Generic;

namespace FitCore.Application.Services.Announcements.Queries.GetAnnouncements
{
    public class ResultGetAnnouncementsDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public AnnouncementType Type { get; set; }

        public AnnouncementPriority Priority { get; set; }

        public bool IsActive { get; set; }

        public bool IsPinned { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class ResultGetAnnouncementsListDto
    {
        public List<ResultGetAnnouncementsDto> Announcements { get; set; }

        public int RowCount { get; set; }
    }
}