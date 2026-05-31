using FitCore.Application.Interfaces.INutritionProgram;
using FitCore.Application.Services.NutritionPrograms.Commands.DeleteNutritionProgram;

namespace FitCore.Application.FacadPatterns
{
    public interface INutritionProgramFacad
    {
        IAddNutritionProgramService AddNutritionProgramService { get; }

        IGetNutritionProgramsService GetNutritionProgramsService { get; }


        IDeleteNutritionProgramService DeleteNutritionProgramService { get; }


    }
}
