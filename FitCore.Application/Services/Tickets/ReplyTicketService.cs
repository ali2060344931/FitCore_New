using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Common.Roles;
using FitCore.Domain.Entities.Tickets;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Tickets
{
    public class ReplyTicketService : IReplyTicketService
    {
        private readonly IDataBaseContext _context;
        public ReplyTicketService(IDataBaseContext context) => _context = context;

        public async Task<ResultDto> Execute(ReplyTicketDto dto)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == dto.TicketId && !t.IsRemoved);
            if (ticket == null) return ResultDto.Failure("تیکت یافت نشد.");
            if (string.IsNullOrWhiteSpace(dto.Body)) return ResultDto.Failure("متن پیام نمی‌تواند خالی باشد.");

            await _context.TicketMessages.AddAsync(new TicketMessage
            {
                TicketId = dto.TicketId,
                SenderUserId = dto.SenderUserId,
                SenderRole = dto.SenderRole,
                Body = dto.Body
            });

            ticket.Status = dto.CloseTicket
                ? TicketStatus.Closed
                : (dto.SenderRole == UserRoles.Member ? TicketStatus.Open : TicketStatus.Answered);
            ticket.UpdateTime = DateTime.Now;

            await _context.SaveChangesAsync();
            return ResultDto.Success("پاسخ شما ثبت شد.");
        }
    }
}