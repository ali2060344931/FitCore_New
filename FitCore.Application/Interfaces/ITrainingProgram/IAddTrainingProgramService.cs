using FitCore.Application.Services.TrainingPrograms.Commands.AddTrainingProgram;
using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.ITrainingProgram
{
    public interface IAddTrainingProgramService
    {
        Task<ResultDto<RequestAddTrainingProgramDto>> Execute(RequestAddTrainingProgramDto request);
    }
}