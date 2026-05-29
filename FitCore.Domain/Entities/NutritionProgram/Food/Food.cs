using FitCore.Domain.Entities.Commons;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Domain.Entities.NutritionProgram.Food
{
    /// <summary>
    /// ماده غذایی مرجع
    /// </summary>
    public class Food: BaseEntity
    {

        public string Title { get; set; }

        public string EnglishTitle { get; set; }

        public int CategoryTypeId { get; set; }
        public FoodCategoryType CategoryType { get; set; }

        public decimal CaloriesPerUnit { get; set; }

        public decimal ProteinPerUnit { get; set; }

        public decimal CarbohydratePerUnit { get; set; }

        public decimal FatPerUnit { get; set; }

        public int DefaultUnitId { get; set; }
        public NutritionUnitType DefaultUnit { get; set; }

        public bool IsActive { get; set; }
    }


    public class NutritionUnitType
    {
        public int Id {  get; set; }
        public string Name { get; set; }

        public  ICollection<Food> food { get; set; }
        public  ICollection<NutritionMealItem.NutritionMealItem> NutritionMealItems { get; set; }
    }
    public class FoodCategoryType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Food> food { get; set; }
    }
}
