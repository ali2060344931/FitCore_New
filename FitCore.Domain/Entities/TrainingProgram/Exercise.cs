using FitCore.Domain.Entities.Commons;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitCore.Domain.Entities.TrainingProgram
{
    /// <summary>
    /// حرکت ورزشی (بانک حرکات)
    /// </summary>
    public class Exercise : BaseEntity
    {
        /// <summary>
        /// نام حرکت (فارسی)
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// نام حرکت (انگلیسی)
        /// </summary>
        [MaxLength(200)]
        public string EnglishName { get; set; }

        /// <summary>
        /// توضیحات / نحوه اجرا
        /// </summary>
        [MaxLength(2000)]
        public string Description { get; set; }

        /// <summary>
        /// گروه عضلانی اصلی
        /// </summary>
        public int PrimaryMuscleGroupId { get; set; }
        public MuscleGroup PrimaryMuscleGroup { get; set; }

        /// <summary>
        /// نوع تجهیزات مورد نیاز
        /// </summary>
        public int EquipmentTypeId { get; set; }
        public EquipmentType EquipmentType { get; set; }

        /// <summary>
        /// سطح دشواری
        /// </summary>
        public int DifficultyLevelId { get; set; }
        public ExerciseDifficultyLevel DifficultyLevel { get; set; }

        /// <summary>
        /// آدرس ویدیو آموزشی
        /// </summary>
        [MaxLength(500)]
        public string VideoUrl { get; set; }

        /// <summary>
        /// تصویر حرکت
        /// </summary>
        [MaxLength(500)]
        public string ImagePath { get; set; }

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; }

        // Navigation
        public ICollection<TrainingExerciseItem> ExerciseItems { get; set; }
    }

    /// <summary>
    /// آیتم تمرینی (حرکت در یک روز برنامه)
    /// </summary>
    public class TrainingExerciseItem : BaseEntity
    {
        /// <summary>
        /// شناسه روز تمرینی
        /// </summary>
        public long TrainingDayId { get; set; }

        /// <summary>
        /// شناسه حرکت
        /// </summary>
        public long ExerciseId { get; set; }

        /// <summary>
        /// تعداد ست
        /// </summary>
        public int? Sets { get; set; }

        /// <summary>
        /// تعداد تکرار (به صورت متنی برای انعطاف: مثلاً "8-12" یا "15")
        /// </summary>
        [MaxLength(50)]
        public string Reps { get; set; }

        /// <summary>
        /// وزن توصیه‌شده (کیلوگرم)
        /// </summary>
        public decimal? WeightKg { get; set; }

        /// <summary>
        /// استراحت بین ست‌ها (ثانیه)
        /// </summary>
        public int? RestSeconds { get; set; }

        /// <summary>
        /// توضیحات / نکات مربی
        /// </summary>
        [MaxLength(500)]
        public string CoachNote { get; set; }

        /// <summary>
        /// ترتیب نمایش در روز
        /// </summary>
        public int SortOrder { get; set; }

        // Navigation
        public TrainingDay TrainingDay { get; set; }
        public Exercise Exercise { get; set; }
    }

    /// <summary>
    /// گروه عضلانی
    /// </summary>
    public class MuscleGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
    }

    /// <summary>
    /// نوع تجهیزات
    /// </summary>
    public class EquipmentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
    }

    /// <summary>
    /// سطح دشواری حرکت
    /// </summary>
    public class ExerciseDifficultyLevel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
    }
}