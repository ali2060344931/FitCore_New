namespace FitCore.Application.Services.Announcements.Commands
{
    public class DashboardAnnouncementDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string ImageUrl { get; set; }

        public string ButtonText { get; set; }

        public string ButtonUrl { get; set; }

        public bool IsPinned { get; set; }

        public int Priority { get; set; }



        // اگر اعلان پین نشده باشد، کاربر می‌تواند آن را ببندد
        public bool CanDismiss => !IsPinned;

        // بررسی وجود تصویر برای اعلان
        public bool HasImage => !string.IsNullOrWhiteSpace(ImageUrl);

        // بررسی وجود دکمه برای اعلان (نیاز به وجود متن و لینک دکمه)
        public bool HasButton => !string.IsNullOrWhiteSpace(ButtonText) && !string.IsNullOrWhiteSpace(ButtonUrl);
    }
}