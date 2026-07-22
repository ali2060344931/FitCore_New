using FitCore.Application.Services.TrainingPrograms.Queries.GetTrainingProgramById;

using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EndPoint.Site.Areas.Admin.Models
{
    public class TrainingProgramBuilderPageViewModel
    {
        public TrainingProgramDetailsDto Program { get; set; }

        public List<SelectListItem> DayTypes { get; set; }





        //public List<SelectListItem> Exercises { get; set; }
        public List<ExerciseLookupDto> Exercises { get; set; } = new();




        public FitCore.Domain.Entities.Members.Member MemberDetails { get; set; }
        public int? MemberAge { get; set; }

    }

    public class ExerciseCreateEditViewModel
    {
        public long Id { get; set; }

        [DisplayName("نام حرکت")]
        [Required(ErrorMessage = "نام حرکت الزامی است")]
        public string Name { get; set; }

        [DisplayName("نام انگلیسی")]
        public string EnglishName { get; set; }

        [DisplayName("توضیحات / نحوه اجرا")]
        public string Description { get; set; }

        [DisplayName("گروه عضلانی اصلی")]
        [Required(ErrorMessage = "گروه عضلانی الزامی است")]
        public int PrimaryMuscleGroupId { get; set; }

        [DisplayName("نوع تجهیزات")]
        [Required(ErrorMessage = "نوع تجهیزات الزامی است")]
        public int EquipmentTypeId { get; set; }

        [DisplayName("سطح دشواری")]
        [Required(ErrorMessage = "سطح دشواری الزامی است")]
        public int DifficultyLevelId { get; set; }

        [DisplayName("آدرس ویدیو")]
        public string VideoUrl { get; set; }

        [DisplayName("تصویر")]
        public string ImagePath { get; set; }

        public bool IsActive { get; set; } = true;

        /// <summary>
        /// آیا این حرکت سراسری باشد (در همه باشگاه‌ها قابل استفاده)؟
        /// فقط توسط مدیر کل قابل انتخاب است.
        /// اگر false باشد، فقط در باشگاه خود مدیر ثبت می‌شود.
        /// </summary>
        public bool IsGlobal { get; set; } = false;
    }

    public class ExerciseLookupDto
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public int? MuscleGroupId { get; set; }

        /// <summary>
        /// اگر true باشد یعنی حرکت عمومی (سراسری) است
        /// </summary>
        public bool IsGlobal { get; set; }
    }
}