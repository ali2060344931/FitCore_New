using FitCore.Application.Services.NutritionProgramBuilder.Queries;

using System.Collections.Generic;

namespace EndPoint.Site.Areas.Admin.Models
{

    public class NutritionProgramBuilderPageViewModel
    {
        public ProgramBuilderDto Program { get; set; }
        public ProgramBuilderDto ResultGetFoodsDto { get; set; }

        public BuilderLookupDto Lookups { get; set; } = new();
    }
}
