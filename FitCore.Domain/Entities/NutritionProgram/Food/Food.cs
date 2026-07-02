using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.Gyms;

using System.Collections.Generic;

namespace FitCore.Domain.Entities.NutritionProgram.Food
{
    /// <summary>
    /// ماده غذایی
    /// </summary>
    public class Food : BaseEntity
    {
        /// <summary>
        /// شناسه باشگاه صاحب این غذا.
        /// مقدار null به معنای غذا سراسری (مشترک بین همه باشگاه‌ها) است
        /// که توسط مدیر کل (SuperAdmin) ثبت می‌شود.
        /// </summary>
        public long? GymId { get; set; }
        public Gym Gym { get; set; }

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


        /// <summary>
        /// لیست واحدهای قابل تبدیل برای این ماده غذایی به همراه ضریب آن‌ها
        /// </summary>
        public ICollection<FoodUnitConversion> UnitConversions { get; set; }
    }

    /// <summary>
    /// نوع واحد اندازه‌گیری
    /// </summary>
    public class NutritionUnitType
    {
        public int Id { get; set; }
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



    /// <summary>
    /// جدول واسط برای تعریف ضریب تبدیل واحد اندازه‌گیری مواد غذایی
    /// </summary>
    public class FoodUnitConversion : BaseEntity
    {
        /// <summary>
        /// شناسه ماده غذایی
        /// </summary>
        public long FoodId { get; set; }
        /// <summary>
        /// ماده غذایی
        /// </summary>
        public Food Food { get; set; }

        /// <summary>
        /// شناسه واحد اندازه‌گیری (واحدی که می‌خواهیم به واحد پیش‌فرض تبدیل شود)
        /// </summary>
        public int UnitTypeId { get; set; }
        /// <summary>
        /// نوع واحد اندازه‌گیری
        /// </summary>
        public NutritionUnitType UnitType { get; set; }

        /// <summary>
        /// ضریب تبدیل: نشان می‌دهد 1 عدد از این واحد (UnitTypeId) معادل چند واحد از واحد پیش‌فرض غذا (DefaultUnitId) است.
        /// مثال: واحد پیش‌فرض گرم است. واحد انتخابی "پیمانه" است. 1 پیمانه برنج = 180 گرم. پس مقدار این فیلد 180 است.
        /// </summary>
        public decimal ConversionFactor { get; set; }


        /// <summary>
        /// شناسه باشگاه. اگر نال باشد یعنی ضریب سراسری است، اگر مقدار داشته باشد مخصوص همان باشگاه است.
        /// </summary>
        public long? GymId { get; set; }
        public Gym Gym { get; set; }

    }
}
