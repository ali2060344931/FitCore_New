using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Foods.Commands.CreateFood
{
    public class CreateFoodDto
    {
        /// <summary>
        /// شناسه باشگاه. مقدار null یعنی حرکت سراسری (مشترک) است
        /// و فقط توسط مدیر کل (SuperAdmin) قابل ثبت است.
        /// </summary>
        public long? GymId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string EnglishTitle { get; set; } = string.Empty;
        public int CategoryTypeId { get; set; }
        public decimal CaloriesPerUnit { get; set; } = 0;
        public decimal ProteinPerUnit { get; set; } = 0;
        public decimal CarbohydratePerUnit { get; set; }= 0;
        public decimal FatPerUnit { get; set; } = 0;
        public int DefaultUnitId { get; set; }
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// آیا این حرکت سراسری (مشترک بین همه باشگاه‌ها) است؟
        /// </summary>
        public bool IsGlobal { get; set; }

        /// <summary>
        /// نام باشگاه صاحب حرکت (در صورتی که سراسری نباشد)
        /// </summary>
        public string GymName { get; set; }

    }
}
