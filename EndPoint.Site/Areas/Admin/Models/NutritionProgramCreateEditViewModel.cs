using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EndPoint.Site.Areas.Admin.Models
{
    public class NutritionProgramCreateEditViewModel
    {
        public long Id { get; set; }

        public long MemberId { get; set; }

        [DisplayName("نوع برنامه")]
        [Required(ErrorMessage = "نوع برنامه الزامی است")]
        public int ProgramTypeId { get; set; }

        [DisplayName("هدف برنامه")]
        [Required(ErrorMessage = "هدف برنامه الزامی است")]
        public int GoalTypeId { get; set; }

        [DisplayName("عنوان برنامه")]
        [Required(ErrorMessage = "عنوان برنامه الزامی است")]
        public string Title { get; set; }

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
