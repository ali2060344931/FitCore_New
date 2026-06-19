using FitCore.Application.Services.NutritionProgramBuilder.Queries;

using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;

namespace EndPoint.Site.Areas.Admin.Models.Foods
{
    public class FoodCreateEditViewModel
    {
        public long? Id { get; set; }
        /// <summary>
        /// شناسه باشگاه. مقدار null یعنی حرکت سراسری (مشترک) است
        /// و فقط توسط مدیر کل (SuperAdmin) قابل ثبت است.
        /// </summary>
        public long? GymId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string EnglishTitle { get; set; } = string.Empty;
        public int CategoryTypeId { get; set; }
        public decimal CaloriesPerUnit { get; set; }
        public decimal ProteinPerUnit { get; set; }
        public decimal CarbohydratePerUnit { get; set; }
        public decimal FatPerUnit { get; set; }
        public int DefaultUnitId { get; set; }
        public bool IsActive { get; set; } = true;

        public List<SelectListItem> CategoryTypes { get; set; } = new();
        public List<SelectListItem> DefaultUnits { get; set; } = new();
        public BuilderLookupDto Lookups { get; set; } = new();
        /// <summary>
        /// آیا این حرکت سراسری باشد (در همه باشگاه‌ها قابل استفاده)؟
        /// فقط توسط مدیر کل قابل انتخاب است.
        /// اگر false باشد، فقط در باشگاه خود مدیر ثبت می‌شود.
        /// </summary>
        public bool IsGlobal { get; set; } = false;

    }
}
