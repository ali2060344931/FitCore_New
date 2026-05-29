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

        public long NutritionProgramId { get; set; }

        public int DayNumber { get; set; }

        public string Title { get; set; }

        public NutritionProgram.NutritionProgram NutritionProgram { get; set; }

        public ICollection<NutritionMeal.NutritionMeal> Meals { get; set; }
    }
}
