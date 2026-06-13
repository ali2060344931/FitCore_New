using FitCore.Application.Services.TrainingPrograms.Queries.GetTrainingPrograms;
using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.ITrainingProgram
{
    public interface IGetTrainingProgramsService
    {
        Task<ResultDto<GetTrainingProgramsResultDto>> Execute(GetTrainingProgramsRequestDto request);
    }
}