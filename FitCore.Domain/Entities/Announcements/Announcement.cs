using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace FitCore.Domain.Entities.Announcements
{
    public class Announcement : BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        [MaxLength(500)]
        public string ImageUrl { get; set; }

        [MaxLength(100)]
        public string ButtonText { get; set; }

        [MaxLength(500)]
        public string ButtonUrl { get; set; }

        public AnnouncementType Type { get; set; }

        public AnnouncementPriority Priority { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public bool ShowOnce { get; set; }

        public bool IsPinned { get; set; }

        public int DisplayOrder { get; set; }

        /// <summary>
        /// اگر true باشد برای تمام نقش‌ها نمایش داده می‌شود.
        /// </summary>
        public bool IsForAllRoles { get; set; }

        /// <summary>
        /// اگر true باشد برای تمام باشگاه‌ها نمایش داده می‌شود.
        /// </summary>
        public bool IsForAllGyms { get; set; }


        /// <summary>
        /// آیا کاربر اجازه بستن این اطلاعیه را دارد؟
        /// </summary>
        public bool CanDismiss { get; set; } = true;

        /// <summary>
        /// بعد از چند روز دوباره نمایش داده شود.
        /// اگر null باشد فقط یکبار نمایش داده می‌شود.
        /// </summary>
        public int? RepeatAfterDays { get; set; }


        public virtual ICollection<AnnouncementRole> Roles { get; set; } = new List<AnnouncementRole>();

        public virtual ICollection<AnnouncementGym> Gyms { get; set; } = new List<AnnouncementGym>();

        public virtual ICollection<AnnouncementView> Views { get; set; } = new List<AnnouncementView>();
    }


    public class AnnouncementRole : BaseEntity
    {
        public long AnnouncementId { get; set; }

        public virtual Announcement Announcement { get; set; }

        public long RoleId { get; set; }
    }


    public class AnnouncementGym : BaseEntity
    {
        public long AnnouncementId { get; set; }

        public Announcement Announcement { get; set; }

        public long GymId { get; set; }

        public Gym Gym { get; set; }
    }

    public class AnnouncementView : BaseEntity
    {
        public long AnnouncementId { get; set; }

        public Announcement Announcement { get; set; }

        public long UserId { get; set; }

        public AppUser User { get; set; }

        public DateTime ViewedAt { get; set; } = DateTime.Now;

        public bool IsClicked { get; set; }

        public DateTime? DismissedAt { get; set; }
    }

    public enum AnnouncementType
    {
        Information = 1,
        Promotion = 2,
        Warning = 3,
        Success = 4,
        News = 5
    }
    public enum AnnouncementPriority
    {
        Low = 1,
        Normal = 2,
        High = 3,
        Critical = 4
    }
}