using FitCore.Application.Services.TrainingPrograms.Queries.GetTrainingProgramById;
using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.ITrainingProgram
{
    public interface IGetTrainingProgramByIdService
    {
        Task<ResultDto<TrainingProgramDetailsDto>> Execute(long trainingProgramId);
    }
}