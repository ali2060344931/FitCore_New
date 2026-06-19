namespace FitCore.Application.Services.Foods.Queries
{
    public class FoodDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string EnglishTitle { get; set; } = string.Empty;
        public int CategoryTypeId { get; set; }
        public string CategoryTypeName { get; set; } = string.Empty;
        public decimal CaloriesPerUnit { get; set; }
        public decimal ProteinPerUnit { get; set; }
        public decimal CarbohydratePerUnit { get; set; }
        public decimal FatPerUnit { get; set; }
        public int DefaultUnitId { get; set; }
        public string DefaultUnitName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
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
