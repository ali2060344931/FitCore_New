using FitCore.Application.Services.TrainingPrograms.Commands.EditTrainingProgram;
using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.ITrainingProgram
{
    public interface IEditTrainingProgramService
    {
        Task<ResultDto<RequestEditTrainingProgramDto>> Execute(RequestEditTrainingProgramDto request);
    }
}