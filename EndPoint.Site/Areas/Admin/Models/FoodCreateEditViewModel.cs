using FitCore.Application.Services.NutritionProgramBuilder.Queries;

using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;

namespace EndPoint.Site.Areas.Admin.Models.Foods
{
    public class FoodCreateEditViewModel
    {
        public long? Id { get; set; }
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

    }
}
