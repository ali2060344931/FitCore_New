using FitCore.Common.Dto;

using System.Threading;
using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.IGym
{
    public interface IDeleteGymService
    {
        //Task<ResultDto> Execute(long id);
        ResultDto Execute(long UserId);
    }
}