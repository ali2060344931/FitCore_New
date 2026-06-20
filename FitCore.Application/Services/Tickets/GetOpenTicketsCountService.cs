using FitCore.Application.Contexts;
using FitCore.Common.Roles;
using FitCore.Domain.Entities.Tickets;

using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace FitCore.Application.Services.Tickets
{
    public class GetOpenTicketsCountService : IGetOpenTicketsCountService
    {
        private readonly IDataBaseContext _context;
        public GetOpenTicketsCountService(IDataBaseContext context) => _context = context;

        public async Task<int> ForAdmin(long gymId) =>
            await _context.Tickets.CountAsync(t => !t.IsRemoved && t.GymId == gymId &&
                t.SenderRole == UserRoles.Member && t.Status == TicketStatus.Open);

        public async Task<int> ForSuperAdmin() =>
            await _context.Tickets.CountAsync(t => !t.IsRemoved &&
                t.SenderRole == UserRoles.Admin && t.Status == TicketStatus.Open);
    }
}