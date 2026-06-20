using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Tickets;

using System.Threading.Tasks;

namespace FitCore.Application.Services.Tickets
{
    public class CreateTicketService : ICreateTicketService
    {
        private readonly IDataBaseContext _context;
        public CreateTicketService(IDataBaseContext context) => _context = context;

        public async Task<ResultDto<long>> Execute(CreateTicketDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Subject) || string.IsNullOrWhiteSpace(dto.Body))
                return ResultDto<long>.Failure("موضوع و متن پیام الزامی است.");

            var ticket = new Ticket
            {
                SenderUserId = dto.SenderUserId,
                SenderRole = dto.SenderRole,
                GymId = dto.GymId,
                Subject = dto.Subject,
                Priority = dto.Priority,
                Status = TicketStatus.Open
            };

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            await _context.TicketMessages.AddAsync(new TicketMessage
            {
                TicketId = ticket.Id,
                SenderUserId = dto.SenderUserId,
                SenderRole = dto.SenderRole,
                Body = dto.Body
            });
            await _context.SaveChangesAsync();

            return ResultDto<long>.Success(ticket.Id, "تیکت با موفقیت ارسال شد.");
        }
    }
}