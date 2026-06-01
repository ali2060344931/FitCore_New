using FitCore.Application.Services.NutritionProgramBuilder.Queries;

using System.Collections.Generic;

namespace EndPoint.Site.Areas.Admin.Models
{

    public class NutritionProgramBuilderPageViewModel
    {
        public ProgramBuilderDto Program { get; set; }

        public BuilderLookupDto Lookups { get; set; } = new();
    }
    /*
    public class NutritionProgramBuilderPageViewModel
    {
        public ProgramBuilderDto Program { get; set; } = new();
        public NutritionProgramBuilderLookupsVm Lookups { get; set; } = new();
    }

    public class NutritionProgramBuilderLookupsVm
    {
        public List<LookupItemVm> Foods { get; set; } = new();
        public List<LookupItemVm> Units { get; set; } = new();
    }

    public class LookupItemVm
    {
        public long Id { get; set; }
        public string Title { get; set; }
    }
    */
}
