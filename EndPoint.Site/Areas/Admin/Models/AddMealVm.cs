using System;

namespace EndPoint.Site.Areas.Admin.Models
{
    public class AddMealVm
    {
        public long NutritionProgramDayId { get; set; }  // MUST be set
        public int MealTypeId { get; set; }              // from dropdown
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan? MealTime { get; set; }
        public int SortOrder { get; set; }
    }
}
