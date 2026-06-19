using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Foods.Commands.EditFood
{
    public class UpdateFoodDto
    {
        /// <summary>
        /// شناسه باشگاه. مقدار null یعنی حرکت سراسری (مشترک) است
        /// و فقط توسط مدیر کل (SuperAdmin) قابل ثبت است.
        /// </summary>
        public long? GymId { get; set; }

        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string EnglishTitle { get; set; } = string.Empty;
        public int CategoryTypeId { get; set; }
        public decimal CaloriesPerUnit { get; set; }
        public decimal ProteinPerUnit { get; set; }
        public decimal CarbohydratePerUnit { get; set; }
        public decimal FatPerUnit { get; set; }
        public int DefaultUnitId { get; set; }
        public bool IsActive { get; set; }

    }
}
