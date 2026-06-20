using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.ITrainingProgram
{
    public interface IDeleteTrainingProgramService
    {
        Task<ResultDto> Execute(long trainingProgramId);
    }

}