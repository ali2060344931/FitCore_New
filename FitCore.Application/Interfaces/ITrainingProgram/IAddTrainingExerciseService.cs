using FitCore.Application.Services.TrainingProgramBuilder.Commands.AddTrainingExercise;
using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.ITrainingProgram
{
    public interface IAddTrainingExerciseService
    {
        Task<ResultDto<RequestAddTrainingExerciseDto>> Execute(RequestAddTrainingExerciseDto request);
    }
}