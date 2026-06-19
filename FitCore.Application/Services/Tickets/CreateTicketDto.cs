using System;
using System.Collections.Generic;

namespace FitCore.Application.Services.Tickets.DTOs
{
    public class CreateTicketDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public class ReplyTicketDto
    {
        public long TicketId { get; set; }
        public string Text { get; set; }
    }

    public class TicketDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string InitialMessage { get; set; }
        public string SenderName { get; set; }
        public string GymName { get; set; }
        public int Status { get; set; }
        public string Date { get; set; }
        public int UnreadMessagesCount { get; set; }
    }

    public class TicketDetailDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public List<TicketMessageDto> Messages { get; set; }
    }

    public class TicketMessageDto
    {
        public string Text { get; set; }
        public string SenderName { get; set; }
        public string Date { get; set; }
        public bool IsOwner { get; set; } // آیا پیام متعلق به کاربر جاری است یا مدیر؟
    }
}