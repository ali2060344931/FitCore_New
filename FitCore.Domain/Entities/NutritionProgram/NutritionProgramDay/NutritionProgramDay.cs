using FitCore.Domain.Entities.Commons;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Domain.Entities.NutritionProgram.NutritionProgramDay
{
    /// <summary>
    /// جزئیات تغذیه‌ای
    /// </summary>
    public class NutritionProgramDay: BaseEntity
    {
        /// <summary>
        /// شناسه برنامه غذایی
        /// </summary>
        public long NutritionProgramId { get; set; }
        /// <summary>
        /// شماره روز
        /// </summary>
        public int DayNumber { get; set; }
        /// <summary>
        /// عنوان روز
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// برنامه غذایی
        /// </summary>
        public NutritionProgram.NutritionProgram NutritionProgram { get; set; }
        /// <summary>
        /// وعده‌های غذایی
        /// </summary>
        public ICollection<NutritionMeal.NutritionMeal> Meals { get; set; }
    }
}
