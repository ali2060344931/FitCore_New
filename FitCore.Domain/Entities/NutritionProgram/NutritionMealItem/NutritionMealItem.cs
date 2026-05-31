using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.NutritionProgram.Food;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Domain.Entities.NutritionProgram.NutritionMealItem
{
    /// <summary>
    ///آیتم وعده غذایی
    /// </summary>
    public class NutritionMealItem:BaseEntity
    {
        /// <summary>
        /// شناسه وعده غذایی
        /// </summary>
        public long NutritionMealId { get; set; }
        /// <summary>
        /// شناسه ماده غذایی
        /// </summary>
        public long FoodId { get; set; }
        /// <summary>
        /// مقدار
        /// </summary>
        public decimal Amount { get; set; }

        public int UnitTypeId { get; set; }
        /// <summary>
        /// نوع واحد اندازه‌گیری
        /// </summary>
        public NutritionUnitType UnitType { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }

        // Snapshot Values
        /// <summary>
        /// کالری
        /// </summary>
        public decimal Calories { get; set; }
        /// <summary>
        /// پروتئین
        /// </summary>
        public decimal Protein { get; set; }
        /// <summary>
        /// کربوهیدرات
        /// </summary>
        public decimal Carbohydrate { get; set; }
        /// <summary>
        /// چربی
        /// </summary>
        public decimal Fat { get; set; }

        // Navigation
        /// <summary>
        /// وعده غذایی
        /// </summary>
        public NutritionMeal.NutritionMeal NutritionMeal { get; set; }
        /// <summary>
        /// ماده غذایی
        /// </summary>
        public Food.Food Food { get; set; }
    }
}
