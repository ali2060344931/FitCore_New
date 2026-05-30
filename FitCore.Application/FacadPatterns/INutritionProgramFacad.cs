using FitCore.Application.Interfaces.INutritionProgram;

namespace FitCore.Application.FacadPatterns
{
    public interface INutritionProgramFacad
    {
        IAddNutritionProgramService AddNutritionProgramService { get; }

        IGetNutritionProgramsService GetNutritionProgramsService { get; }


        //IGetNutritionProgramByIdService GetNutritionProgramByIdService { get; }


    }
}
