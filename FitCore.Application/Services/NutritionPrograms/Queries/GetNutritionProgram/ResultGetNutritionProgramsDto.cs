using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionPrograms.Queries.GetNutritionProgram
{
    public class ResultGetNutritionProgramsDto
    {
        public List<GetNutritionProgramsDto> NutritionPrograms { get; set; }

        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }
        public int Row { get; set; }
    }


    public class GetNutritionProgramsDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string GoalType { get; set; }
        
        public string ProgramType { get; set; }

        public string MemberName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public bool IsActive { get; set; }
    }

    public class RequestGetNutritionProgramsDto
    {
        public string SearchKey { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public long AppUserId { get; set; }
    }

}
