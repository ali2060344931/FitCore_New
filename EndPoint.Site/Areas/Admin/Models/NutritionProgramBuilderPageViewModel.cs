using FitCore.Application.Services.NutritionProgramBuilder.Queries;

using System.Collections.Generic;

namespace EndPoint.Site.Areas.Admin.Models
{
    public class NutritionProgramBuilderPageViewModel
    {
        public ProgramBuilderDto Program { get; set; }
        public ProgramBuilderDto ResultGetFoodsDto { get; set; }

        public BuilderLookupDto Lookups { get; set; } = new();

        // استفاده از مسیر کامل برای جلوگیری از هرگونه تداخل فضای نام
        public FitCore.Domain.Entities.Members.Member MemberDetails { get; set; }

        public int? MemberAge { get; set; }
    }
}