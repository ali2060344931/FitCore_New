using FitCore.Application.Services.TrainingProgramBuilder.Commands.EditTrainingDay;
using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.ITrainingProgram
{
    public interface IEditTrainingDayService
    {
        Task<ResultDto<RequestEditTrainingDayDto>> Execute(RequestEditTrainingDayDto request);
    }
}