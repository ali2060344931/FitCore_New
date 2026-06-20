using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Services.Tickets
{
    // ============ Create ============

    public interface ICreateTicketService { Task<ResultDto<long>> Execute(CreateTicketDto dto); }
}