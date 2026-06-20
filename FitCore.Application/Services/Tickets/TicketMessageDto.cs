using System;

namespace FitCore.Application.Services.Tickets
{
    public class TicketMessageDto
    {
        public long Id { get; set; }
        public long SenderUserId { get; set; }
        public string SenderRole { get; set; }
        public string SenderName { get; set; }
        public string Body { get; set; }
        public string AttachmentPath { get; set; }
        public DateTime InsertTime { get; set; }
    }
}