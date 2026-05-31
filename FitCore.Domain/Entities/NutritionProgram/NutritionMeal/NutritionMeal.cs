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
        /// <summary>
        /// شناسه روزِ برنامه غذایی
        /// </summary>
        public long NutritionProgramDayId { get; set; }

        public int MealTypeId { get; set; }
        /// <summary>
        /// نوع وعده
        /// </summary>
        public MealType MealType { get; set; }
        /// <summary>
        /// عنوان وعده
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// ساعت وعده
        /// </summary>
        public TimeSpan? MealTime { get; set; }
        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int SortOrder { get; set; }

        // Nutrition Summary
        /// <summary>
        /// مجموع کالری
        /// </summary>
        public decimal TotalCalories { get; set; }
        /// <summary>
        /// مجموع پروتئین
        /// </summary>
        public decimal TotalProtein { get; set; }
        /// <summary>
        /// مجموع کربوهیدرات
        /// </summary>
        public decimal TotalCarbohydrate { get; set; }
        /// <summary>
        /// مجموع چربی
        /// </summary>
        public decimal TotalFat { get; set; }

        // Navigation
        /// <summary>
        /// روزِ برنامه غذایی
        /// </summary>
        public NutritionProgramDay.NutritionProgramDay NutritionProgramDay { get; set; }
        /// <summary>
        /// آیتم‌های وعده (اقلام غذایی)
        /// </summary>
        public ICollection<NutritionMealItem.NutritionMealItem> Items { get; set; }
    }

    /// <summary>
    /// نوع وعده
    /// </summary>
    public class MealType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<NutritionMeal> nutritionMeals { get; set; }

    }
}
