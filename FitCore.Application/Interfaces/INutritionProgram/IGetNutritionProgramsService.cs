using FitCore.Application.Services.NutritionPrograms.Queries.GetNutritionProgram;

using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.INutritionProgram
{
    public interface IGetNutritionProgramsService
    {
        Task<ResultGetNutritionProgramsDto> Execute(RequestGetNutritionProgramsDto request);
    }
}
