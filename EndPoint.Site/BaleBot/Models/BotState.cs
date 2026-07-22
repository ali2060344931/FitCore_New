using FitCore.Domain.Entities.Members;

namespace EndPoint.Site.BaleBot.Models
{
    public class BotState
    {
        public string Step { get; set; }
        public string RegType { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public long? GymId { get; set; }
        public string FullName { get; set; }
        public string GymName { get; set; }
        public long? SelectedUserId { get; set; }
        public long? SelectedGymId { get; set; }

        public Gender? Gender { get; set; }
        public int? BirthYear { get; set; }
        public int? BirthMonth { get; set; }
        public int? BirthDay { get; set; }

        // ===== حذف شوند =====
        // public int? BirthYearRangeStart { get; set; }
        // public int? BirthYearRangeEnd { get; set; }

        // ===== جدید (پیشنهاد 3 و تغییر فلو) =====
        /// <summary>
        /// شماره موبایل استاندارد شده برای استفاده بعد از دریافت قد و وزن
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// قد (سانتی‌متر)
        /// </summary>
        public decimal? Height { get; set; }

        /// <summary>
        /// وزن (کیلوگرم)
        /// </summary>
        public decimal? Weight { get; set; }
    }
}