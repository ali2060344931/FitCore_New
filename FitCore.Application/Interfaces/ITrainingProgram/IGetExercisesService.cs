using FitCore.Application.Services.Exercises.Queries;
using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.ITrainingProgram
{
    public interface IGetExercisesService
    {
        Task<ResultDto<GetExercisesResultDto>> Execute(GetExercisesRequestDto request);
    }
}