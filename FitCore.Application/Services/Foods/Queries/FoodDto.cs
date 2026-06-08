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
    }
}
