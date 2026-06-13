using FitCore.Application.Services.TrainingProgramBuilder.Commands.EditTrainingExercise;
using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.ITrainingProgram
{
    public interface IEditTrainingExerciseService
    {
        Task<ResultDto<RequestEditTrainingExerciseDto>> Execute(RequestEditTrainingExerciseDto request);
    }
}