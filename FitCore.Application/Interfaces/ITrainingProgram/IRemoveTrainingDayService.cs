using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.ITrainingProgram
{
    public interface IRemoveTrainingDayService
    {
        Task<ResultDto> Execute(long trainingDayId);
    }
}