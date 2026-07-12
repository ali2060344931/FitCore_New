namespace FitCore.Application.Services.Announcements.Dashboard.DismissAnnouncement
{
    public class RequestDismissAnnouncementDto
    {
        public long AnnouncementId { get; set; }

        public long UserId { get; set; }
    }
}