using FitCore.Application.Services.Exercises.Commands.AddExercise;
using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.ITrainingProgram
{
    public interface IAddExerciseService
    {
        Task<ResultDto<RequestAddExerciseDto>> Execute(RequestAddExerciseDto request);
    }
}