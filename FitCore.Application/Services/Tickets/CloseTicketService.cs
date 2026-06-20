using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Tickets;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Tickets
{
    public class CloseTicketService : ICloseTicketService
    {
        private readonly IDataBaseContext _context;
        public CloseTicketService(IDataBaseContext context) => _context = context;

        public async Task<ResultDto> Execute(long ticketId)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
            if (ticket == null) return ResultDto.Failure("تیکت یافت نشد.");
            ticket.Status = TicketStatus.Closed;
            ticket.UpdateTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return ResultDto.Success("تیکت بسته شد.");
        }
    }
}