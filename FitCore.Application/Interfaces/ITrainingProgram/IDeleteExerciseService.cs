using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.ITrainingProgram
{
    public interface IDeleteExerciseService
    {
        Task<ResultDto> Execute(long exerciseId);
    }
}