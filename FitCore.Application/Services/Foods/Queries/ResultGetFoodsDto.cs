using FitCore.Application.Services.NutritionProgramBuilder.Queries;

using System.Collections.Generic;

namespace FitCore.Application.Services.Foods.Queries
{
    public class ResultGetFoodsDto
    {
        public List<FoodDto> Foods { get; set; }

        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }
        public int PageSize { get; set; }


    }
}
