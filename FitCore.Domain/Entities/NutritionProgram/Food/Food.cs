using FitCore.Domain.Entities.Commons;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Domain.Entities.NutritionProgram.Food
{
    /// <summary>
    /// ماده غذایی
    /// </summary>
    public class Food: BaseEntity
    {
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// عنوان انگلیسی
        /// </summary>
        public string EnglishTitle { get; set; }
        /// <summary>
        /// شناسه دسته‌بندی
        /// </summary>
        public int CategoryTypeId { get; set; }

        /// <summary>
        /// دسته‌بندی ماده غذایی
        /// </summary>
        public FoodCategoryType CategoryType { get; set; }
        /// <summary>
        /// کالری به‌ازای هر واحد
        /// </summary>
        public decimal CaloriesPerUnit { get; set; }
        /// <summary>
        /// پروتئین به‌ازای هر واحد
        /// </summary>
        public decimal ProteinPerUnit { get; set; }
        /// <summary>
        /// کربوهیدرات به‌ازای هر واحد
        /// </summary>
        public decimal CarbohydratePerUnit { get; set; }
        /// <summary>
        /// چربی به‌ازای هر واحد
        /// </summary>
        public decimal FatPerUnit { get; set; }
        /// <summary>
        /// شناسه واحد پیش‌فرض
        /// </summary>
        public int DefaultUnitId { get; set; }
        /// <summary>
        /// واحد پیش‌فرض
        /// </summary>
        public NutritionUnitType DefaultUnit { get; set; }
        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// نوع واحد اندازه‌گیری
    /// </summary>
    public class NutritionUnitType
    {
        public int Id {  get; set; }
        /// <summary>
        /// نام واحد
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// مواد غذایی (مرتبط با این واحد)
        /// </summary>
        public ICollection<Food> food { get; set; }
        /// <summary>
        /// آیتم‌های وعده غذایی (مرتبط با این واحد)
        /// </summary>
        public ICollection<NutritionMealItem.NutritionMealItem> NutritionMealItems { get; set; }
    }
    /// <summary>
    /// نوع دسته‌بندی مواد غذایی
    /// </summary>
    public class FoodCategoryType
    {
        public int Id { get; set; }
        /// <summary>
        /// نام دسته‌بندی
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// مواد غذایی این دسته
        /// </summary>
        public ICollection<Food> food { get; set; }
    }
}
