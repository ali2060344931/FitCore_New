using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Interfaces.INutritionProgram;
using FitCore.Application.Services.NutritionPrograms.Commands.AddNutritionProgram;
using FitCore.Application.Services.NutritionPrograms.Queries.GetNutritionProgram;

namespace FitCore.Application.Services.NutritionPrograms.NutritionProgramsFacad
{
    public class NutritionProgramFacad : INutritionProgramFacad
    {
        private readonly IDataBaseContext _context;

        public NutritionProgramFacad(
            IDataBaseContext context)
        {
            _context = context;
        }


        private IGetNutritionProgramsService _getNutritionProgramsService;
        public IGetNutritionProgramsService GetNutritionProgramsService =>
            _getNutritionProgramsService ??=
            new GetNutritionProgramsService(_context);
       
        
        
        private IAddNutritionProgramService  _addNutritionProgramService;
        public IAddNutritionProgramService AddNutritionProgramService =>
            _addNutritionProgramService ??=
            new AddNutritionProgramService(_context);

    }
}
