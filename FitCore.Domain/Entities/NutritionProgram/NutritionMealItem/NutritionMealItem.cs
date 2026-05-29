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
    /// «یک ماده غذایی خاص» در یک وعده
    /// </summary>
    public class NutritionMealItem:BaseEntity
    {

        public long NutritionMealId { get; set; }

        public long FoodId { get; set; }

        public decimal Amount { get; set; }

        
        public int UnitTypeId { get; set; }
        public NutritionUnitType UnitType { get; set; }

        public string Description { get; set; }

        // Snapshot Values

        public decimal Calories { get; set; }

        public decimal Protein { get; set; }

        public decimal Carbohydrate { get; set; }

        public decimal Fat { get; set; }

        // Navigation

        public NutritionMeal.NutritionMeal NutritionMeal { get; set; }

        public Food.Food Food { get; set; }
    }
}
