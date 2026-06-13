using FitCore.Application.Services.TrainingProgramBuilder.Commands.AddTrainingDay;
using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.ITrainingProgram
{
    public interface IAddTrainingDayService
    {
        Task<ResultDto<RequestAddTrainingDayDto>> Execute(RequestAddTrainingDayDto request);
    }
}