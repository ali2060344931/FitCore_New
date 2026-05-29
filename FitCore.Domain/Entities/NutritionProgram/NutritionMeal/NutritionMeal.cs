using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgram;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Domain.Entities.NutritionProgram.NutritionMeal
{
    /// <summary>
    /// وعده غذایی
    /// </summary>
    public class NutritionMeal:BaseEntity
    {

        public long NutritionProgramDayId { get; set; }

        public int MealTypeId { get; set; }
        public MealType MealType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TimeSpan? MealTime { get; set; }

        public int SortOrder { get; set; }

        // Nutrition Summary

        public decimal TotalCalories { get; set; }

        public decimal TotalProtein { get; set; }

        public decimal TotalCarbohydrate { get; set; }

        public decimal TotalFat { get; set; }

        // Navigation

        public NutritionProgramDay.NutritionProgramDay NutritionProgramDay { get; set; }

        public ICollection<NutritionMealItem.NutritionMealItem> Items { get; set; }
    }


    public class MealType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<NutritionMeal> nutritionMeals { get; set; }

    }
}
