using FitCore.Common.Dto;

using System.Threading.Tasks;

namespace FitCore.Application.Services.Tickets
{
    // ============ Close ============

    public interface ICloseTicketService { Task<ResultDto> Execute(long ticketId); }
}