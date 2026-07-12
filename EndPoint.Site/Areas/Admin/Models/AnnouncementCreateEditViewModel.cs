using FitCore.Domain.Entities.Announcements;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EndPoint.Site.Areas.Admin.Models.Announcement
{
    public class AnnouncementCreateEditViewModel
    {
        public long Id { get; set; }

        [DisplayName("عنوان")]
        [Required(ErrorMessage = "عنوان الزامی است")]
        public string Title { get; set; }

        [DisplayName("متن اطلاعیه")]
        [Required(ErrorMessage = "متن اطلاعیه الزامی است")]
        public string Message { get; set; }

        public string ImageUrl { get; set; }

        public IFormFile ImageFile { get; set; }

        [DisplayName("متن دکمه")]
        public string ButtonText { get; set; }

        [DisplayName("آدرس دکمه")]
        public string ButtonUrl { get; set; }

        public AnnouncementType Type { get; set; }

        public AnnouncementPriority Priority { get; set; }

        public bool IsActive { get; set; }

        public bool IsPinned { get; set; }

        public bool ShowOnce { get; set; }

        public bool IsForAllRoles { get; set; }

        public bool IsForAllGyms { get; set; }

        public bool CanDismiss { get; set; }

        public int? RepeatAfterDays { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<long> SelectedRoleIds { get; set; } = new();

        public List<long> SelectedGymIds { get; set; } = new();

        public List<SelectListItem> Roles { get; set; } = new();

        public List<SelectListItem> Gyms { get; set; } = new();
    }
}