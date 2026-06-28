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
        /// <summary>
        /// شناسه کاربر (AppUser.Id) انتخاب‌شده برای جلسه فعلی ربات
        /// </summary>
        public long? SelectedUserId { get; set; }
        /// <summary>
        /// شناسه باشگاه انتخاب‌شده برای جلسه فعلی ربات
        /// </summary>
        public long? SelectedGymId { get; set; }
    }
}
