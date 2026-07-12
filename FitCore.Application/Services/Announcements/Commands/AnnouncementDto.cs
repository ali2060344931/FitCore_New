namespace FitCore.Application.Services.Announcements.Common
{
    public class AnnouncementDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string ImageUrl { get; set; }

        public string ButtonText { get; set; }

        public string ButtonUrl { get; set; }

        public int Type { get; set; }

        public int Priority { get; set; }

        public bool IsPinned { get; set; }
    }
}