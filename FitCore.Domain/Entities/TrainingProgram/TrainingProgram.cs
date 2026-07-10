using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Users;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitCore.Domain.Entities.TrainingProgram
{
    /// <summary>
    /// برنامه تمرینی
    /// </summary>
    public class TrainingProgram : BaseEntity
    {
        /// <summary>
        /// شناسه باشگاه
        /// </summary>
        public long? GymId { get; set; }

        /// <summary>
        /// شناسه عضو
        /// </summary>
        public long MemberId { get; set; }

        /// <summary>
        /// شناسه کاربر ایجادکننده
        /// </summary>
        public long CreatedByUserId { get; set; }

        /// <summary>
        /// عنوان برنامه
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// نوع برنامه تمرینی (قدرتی، استقامتی، ...)
        /// </summary>
        public int TrainingProgramTypeId { get; set; }
        public TrainingProgramType TrainingProgramType { get; set; }

        /// <summary>
        /// هدف تمرینی (حجم، کات، کاهش وزن، ...)
        /// </summary>
        public int TrainingGoalTypeId { get; set; }
        public TrainingGoalType TrainingGoalType { get; set; }

        /// <summary>
        /// تعداد جلسات در هفته
        /// </summary>
        public int? SessionsPerWeek { get; set; }

        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; }

        // Navigation Properties
        public Gym Gym { get; set; }
        public Member Member { get; set; }
        public AppUser CreatedByUser { get; set; }
        public bool IsSeen { get; set; } = false;
        public DateTime? SeenAt { get; set; }

        /// <summary>
        /// روزهای برنامه تمرینی
        /// </summary>
        public ICollection<TrainingDay> Days { get; set; }

        public bool Equals(TrainingProgram other)
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// نوع برنامه تمرینی
    /// </summary>
    public class TrainingProgramType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TrainingProgram> TrainingPrograms { get; set; }
    }

    /// <summary>
    /// هدف تمرینی
    /// </summary>
    public class TrainingGoalType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TrainingProgram> TrainingPrograms { get; set; }
    }
}