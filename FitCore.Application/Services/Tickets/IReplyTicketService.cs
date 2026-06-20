using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Services.Tickets
{
    // ============ Reply ============

    public interface IReplyTicketService { Task<ResultDto> Execute(ReplyTicketDto dto); }
}