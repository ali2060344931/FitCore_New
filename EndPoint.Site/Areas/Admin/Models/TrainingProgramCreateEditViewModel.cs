using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EndPoint.Site.Areas.Admin.Models
{
    public class TrainingProgramCreateEditViewModel
    {
        public long Id { get; set; }

        public long MemberId { get; set; }

        [DisplayName("عنوان برنامه")]
        [Required(ErrorMessage = "عنوان برنامه الزامی است")]
        public string Title { get; set; }

        [DisplayName("نوع برنامه تمرینی")]
        [Required(ErrorMessage = "نوع برنامه تمرینی الزامی است")]
        public int TrainingProgramTypeId { get; set; }

        [DisplayName("هدف تمرینی")]
        [Required(ErrorMessage = "هدف تمرینی الزامی است")]
        public int TrainingGoalTypeId { get; set; }

        [DisplayName("تعداد جلسات در هفته")]
        public int? SessionsPerWeek { get; set; }

        [DisplayName("توضیحات")]
        public string Description { get; set; }

        [DisplayName("تاریخ شروع")]
        [Required(ErrorMessage = "تاریخ شروع برنامه الزامی است")]
        public string StartDate { get; set; }

        [DisplayName("تاریخ پایان")]
        [Required(ErrorMessage = "تاریخ پایان برنامه الزامی است")]
        public string EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}