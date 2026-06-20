using System.Threading.Tasks;

namespace FitCore.Application.Services.Tickets
{
    // ============ Get Detail ============

    public interface IGetTicketDetailService { Task<TicketDetailDto> Execute(long ticketId); }
}