using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Users;

using System;
using System.Collections.Generic;

namespace FitCore.Domain.Entities.NutritionProgram.NutritionProgram
{
    /// <summary>
    /// برنامه غذایی
    /// </summary>
    public class NutritionProgram : BaseEntity
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
        /// توضیحات
        /// </summary>
        public string Description { get; set; }

        public int ProgramTypeId { get; set; }
        /// <summary>
        /// نوع برنامه غذایی
        /// </summary>
        public NutritionProgramType ProgramType { get; set; }

        public int GoalTypeId { get; set; }
        /// <summary>
        /// هدف برنامه
        /// </summary>
        public GoalType GoalType { get; set; }
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


        // Navigation
        /// <summary>
        /// باشگاه
        /// </summary>
        public Gym Gym { get; set; }
        /// <summary>
        /// عضو
        /// </summary>
        public Member Member { get; set; }

        /// <summary>
        /// کاربر ایجادکننده
        /// </summary>
        public AppUser CreatedByUser { get; set; }
        /// <summary>
        /// روزهای برنامه
        /// </summary>
        public ICollection<NutritionProgramDay.NutritionProgramDay> Days { get; set; }
    }

    /// <summary>
    /// نوع برنامه غذایی
    /// </summary>
    public class NutritionProgramType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<NutritionProgram> nutritionPrograms { get; set; }

    }
    /// <summary>
    /// هدف برنامه
    /// </summary>
    public class GoalType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<NutritionProgram> nutritionPrograms { get; set; }
    }
}
