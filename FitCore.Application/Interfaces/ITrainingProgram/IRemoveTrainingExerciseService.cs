using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.ITrainingProgram
{
    public interface IRemoveTrainingExerciseService
    {
        Task<ResultDto> Execute(long trainingExerciseItemId);
    }
}