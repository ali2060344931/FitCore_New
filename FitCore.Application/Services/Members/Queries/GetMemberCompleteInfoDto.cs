using FitCore.Domain.Entities.Members;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FitCore.Application.Services.Members.Queries
{
    public class GetMemberCompleteInfoDto
    {
        public long Id { get; set; }
        public long AppUserId { get; set; }
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
        
        [Display(Name = "تاریخ پابان عضویت")]
        public string MembershipEndDate { get; set; }

        public int? ActivityLevelId { get; set; }
        public string ActivityLevelName { get; set; }

        public int? ExperienceLevelId { get; set; }
        public string ExperienceLevelName { get; set; }

        public string FoodAllergies { get; set; }
        public string MedicalConditions { get; set; }
        public string Injuries { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "توضیحات")]

        public string Description { get; set; }
        [Display(Name = "قد(سانتی متر)")]

        public decimal? Height { get; set; }

    }


}
