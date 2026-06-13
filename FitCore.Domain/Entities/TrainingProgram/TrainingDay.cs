using FitCore.Domain.Entities.Commons;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitCore.Domain.Entities.TrainingProgram
{
    /// <summary>
    /// روز تمرینی در برنامه
    /// </summary>
    public class TrainingDay : BaseEntity
    {
        /// <summary>
        /// شناسه برنامه تمرینی
        /// </summary>
        public long TrainingProgramId { get; set; }

        /// <summary>
        /// شماره روز (روز ۱، روز ۲، ...)
        /// </summary>
        public int DayNumber { get; set; }

        /// <summary>
        /// عنوان روز (مثلاً: سینه و سه‌سر)
        /// </summary>
        [MaxLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// نوع روز تمرینی (Push, Pull, Leg, FullBody, Rest, ...)
        /// </summary>
        public int DayTypeId { get; set; }
        public TrainingDayType DayType { get; set; }

        /// <summary>
        /// توضیحات روز
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// مدت زمان تمرین (دقیقه)
        /// </summary>
        public int? DurationMinutes { get; set; }

        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int SortOrder { get; set; }

        // Navigation
        public TrainingProgram TrainingProgram { get; set; }

        /// <summary>
        /// تمرینات این روز
        /// </summary>
        public ICollection<TrainingExerciseItem> ExerciseItems { get; set; }
    }

    /// <summary>
    /// نوع روز تمرینی
    /// </summary>
    public class TrainingDayType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TrainingDay> TrainingDays { get; set; }
    }
}