using FitCore.Application.Contexts;

using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Tickets
{
    public class GetTicketDetailService : IGetTicketDetailService
    {
        private readonly IDataBaseContext _context;
        public GetTicketDetailService(IDataBaseContext context) => _context = context;

        public async Task<TicketDetailDto> Execute(long ticketId)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Messages).ThenInclude(m => m.SenderUser)
                .FirstOrDefaultAsync(t => t.Id == ticketId && !t.IsRemoved);

            if (ticket == null) return null;

            return new TicketDetailDto
            {
                Id = ticket.Id,
                Subject = ticket.Subject,
                Status = ticket.Status,
                Priority = ticket.Priority,
                SenderUserId = ticket.SenderUserId,
                Messages = ticket.Messages
                    .Where(m => !m.IsRemoved)
                    .OrderBy(m => m.InsertTime)
                    .Select(m => new TicketMessageDto
                    {
                        Id = m.Id,
                        SenderUserId = m.SenderUserId,
                        SenderRole = m.SenderRole,
                        SenderName = m.SenderUser.FullName,
                        Body = m.Body,
                        AttachmentPath = m.AttachmentPath,
                        InsertTime = m.InsertTime
                    }).ToList()
            };
        }
    }
}