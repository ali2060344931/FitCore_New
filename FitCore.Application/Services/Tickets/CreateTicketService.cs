using FitCore.Application.Contexts;
using FitCore.Application.Services.Tickets.DTOs;
using FitCore.Application.Services.Tickets.Interfaces;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Tickets;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;

namespace FitCore.Application.Services.Tickets
{
    public class CreateTicketService : ICreateTicketService
    {
        private readonly IDataBaseContext _context;
        public CreateTicketService(IDataBaseContext context) { _context = context; }

        public ResultDto<long> Execute(CreateTicketDto request, long senderUserId)
        {
            var ticket = new Domain.Entities.Tickets.Ticket
            {
                Title = request.Title,
                Message = request.Message,
                SenderUserId = senderUserId
            };
            _context.Tickets.Add(ticket);
            _context.SaveChanges();
            return new ResultDto<long> { Data = ticket.Id, IsSuccess = true, Message = "تیکت با موفقیت ثبت شد" };
        }
    }

    public class ReplyTicketService : IReplyTicketService
    {
        private readonly IDataBaseContext _context;
        public ReplyTicketService(IDataBaseContext context) { _context = context; }

        public ResultDto Execute(ReplyTicketDto request, long replierUserId)
        {
            var message = new Domain.Entities.Tickets.TicketMessage
            {
                TicketId = request.TicketId,
                Text = request.Text,
                SenderUserId = replierUserId
            };
            _context.TicketMessages.Add(message);

            // آپدیت وضعیت تیکت به "پاسخ داده شده"
            var ticket = _context.Tickets.Find(request.TicketId);
            if (ticket != null && ticket.Status != Domain.Entities.Tickets.TicketStatus.Cleared)
            {
                ticket.Status = Domain.Entities.Tickets.TicketStatus.Answered;
            }

            _context.SaveChanges();
            return new ResultDto { IsSuccess = true, Message = "پاسخ ثبت شد" };
        }



        public ResultDto<TicketDetailDto> Execute(long ticketId, long? currentGymId)
        {
            // گرفتن تیکت به همراه چک کردن soft delete
            var ticket = _context.Tickets
                .Include(t => t.SenderUser)
                .Include(t => t.Messages.Where(m => !m.IsRemoved))
                .FirstOrDefault(t => t.Id == ticketId && !t.IsRemoved);

            // بررسی وجود تیکت
            if (ticket == null)
            {
                return new ResultDto<TicketDetailDto> { IsSuccess = false, Message = "تیکت یافت نشد." };
            }

            // --- امنیت چندمستأجری ---
            // اگر کاربر مدیر باشگاه است و تیکت متعلق به باشگاه او نیست، دسترسی ممنوع است
            if (currentGymId.HasValue && ticket.GymId != currentGymId)
            {
                return new ResultDto<TicketDetailDto> { IsSuccess = false, Message = "شما دسترسی به این تیکت را ندارید." };
            }

            // آماده‌سازی خروجی
            var result = new TicketDetailDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Status = ticket.Status.ToString(), // خطای Cleared اینجا برطرف می‌شود
                Messages = ticket.Messages.Select(m => new TicketMessageDto
                {
                    Text = m.Text,
                    SenderName = m.SenderUser.FullName,
                    Date = m.InsertTime.ToString("yyyy/MM/dd - HH:mm"),
                    IsOwner = m.SenderUserId == ticket.SenderUserId
                }).ToList()
            };

            return new ResultDto<TicketDetailDto> { Data = result, IsSuccess = true };
        }
        public class GetGymTicketsService : IGetGymTicketsService
        {
            private readonly IDataBaseContext _context;
            public GetGymTicketsService(IDataBaseContext context) { _context = context; }

            public ResultDto<List<TicketDto>> Execute(long gymId)
            {
                var tickets = _context.Tickets
                    .Include(t => t.SenderUser)
                    .Include(t => t.Gym)
                    .Where(t => t.GymId == gymId && !t.IsRemoved)
                    .OrderByDescending(t => t.Id)
                    .Select(t => new TicketDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        InitialMessage = t.Message,
                        SenderName = t.SenderUser.FullName,
                        GymName = t.Gym.Name,
                        Status = (int)t.Status,
                        Date = t.InsertTime.ToString("yyyy/MM/dd"),
                        UnreadMessagesCount = t.Messages.Count(m => !m.IsRead && m.SenderUserId != t.SenderUserId)
                    }).ToList();

                return new ResultDto<List<TicketDto>> { Data = tickets, IsSuccess = true };
            }
        }

        public class GetAllTicketsService : IGetAllTicketsService
        {
            private readonly IDataBaseContext _context;
            public GetAllTicketsService(IDataBaseContext context) { _context = context; }

            public ResultDto<List<TicketDto>> Execute()
            {
                var tickets = _context.Tickets
                    .Include(t => t.SenderUser)
                    .Include(t => t.Gym)
                    .Where(t => !t.IsRemoved)
                    .OrderByDescending(t => t.Id)
                    .Select(t => new TicketDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        InitialMessage = t.Message,
                        SenderName = t.SenderUser.FullName,
                        GymName = t.Gym.Name ?? "سیستم (بدون باشگاه)",
                        Status = (int)t.Status,
                        Date = t.InsertTime.ToString("yyyy/MM/dd"),
                        UnreadMessagesCount = t.Messages.Count(m => !m.IsRead && m.SenderUserId != t.SenderUserId)
                    }).ToList();

                return new ResultDto<List<TicketDto>> { Data = tickets, IsSuccess = true };
            }
        }
    }
}