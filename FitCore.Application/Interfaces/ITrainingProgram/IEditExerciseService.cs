using FitCore.Application.Services.Exercises.Commands.EditExercise;
using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.ITrainingProgram
{
    public interface IEditExerciseService
    {
        Task<ResultDto<RequestEditExerciseDto>> Execute(RequestEditExerciseDto request);
    }
}