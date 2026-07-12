using FitCore.Domain.Entities.Announcements;

namespace FitCore.Application.Services.Announcements.Dashboard.GetDashboardAnnouncements
{
    public class ResultGetDashboardAnnouncementDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string ImageUrl { get; set; }

        public string ButtonText { get; set; }

        public string ButtonUrl { get; set; }

        public AnnouncementType Type { get; set; }

        public AnnouncementPriority Priority { get; set; }

        public bool CanDismiss { get; set; }

        public bool IsPinned { get; set; }

        public bool ShowOnce { get; set; }

        public int? RepeatAfterDays { get; set; }

        public int DisplayOrder { get; set; }

        // ==========================
        // Computed Properties
        // ==========================

        public bool HasImage =>
            !string.IsNullOrWhiteSpace(ImageUrl);

        public bool HasButton =>
            !string.IsNullOrWhiteSpace(ButtonUrl);

        public string CssClass =>
            Type switch
            {
                AnnouncementType.Information => "info",
                AnnouncementType.Warning => "warning",
                AnnouncementType.Success => "success",
                AnnouncementType.Promotion => "primary",
                AnnouncementType.News => "secondary",
                _ => "info"
            };

        public bool HasAction => HasButton;

        public bool HasExpireDate => RepeatAfterDays.HasValue;

    }
}