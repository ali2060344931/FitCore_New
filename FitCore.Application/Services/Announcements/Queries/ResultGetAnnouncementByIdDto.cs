using FitCore.Domain.Entities.Announcements;

using System;
using System.Collections.Generic;

namespace FitCore.Application.Services.Announcements.Queries.GetAnnouncementById
{
    public class ResultGetAnnouncementByIdDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string ImageUrl { get; set; }

        public string ButtonText { get; set; }

        public string ButtonUrl { get; set; }

        public AnnouncementType Type { get; set; }

        public AnnouncementPriority Priority { get; set; }

        public bool IsActive { get; set; }

        public bool ShowOnce { get; set; }

        public bool IsPinned { get; set; }

        public bool IsForAllRoles { get; set; }

        public bool IsForAllGyms { get; set; }

        public bool CanDismiss { get; set; }

        public int? RepeatAfterDays { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<long> RoleIds { get; set; }

        public List<long> GymIds { get; set; }
    }
}