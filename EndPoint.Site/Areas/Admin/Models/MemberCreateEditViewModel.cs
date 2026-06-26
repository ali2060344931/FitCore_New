using FitCore.Domain.Entities.Members;

using System.ComponentModel.DataAnnotations;

namespace EndPoint.Site.Areas.Admin.Models.Member
{
    public class MemberCreateEditViewModel
    {
        public long Id { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }
        [Display(Name = "تلفن همراه")]
        public string Mobile { get; set; }
        [Display(Name = "جنسیت")]
        public Gender Gender { get; set; }
        [Display(Name = "تاریخ تولد")]
        public string BirthDate { get; set; }
        [Display(Name = "تاریخ شروع عضویت")]
        public string MembershipStartDate { get; set; }

        [Display(Name ="تاریخ پایان عضویت")]
        public string MembershipEndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
