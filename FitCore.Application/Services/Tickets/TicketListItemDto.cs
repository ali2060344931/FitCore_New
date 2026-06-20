using FitCore.Domain.Entities.Tickets;

using System;

namespace FitCore.Application.Services.Tickets
{
    // ============ DTOs ============

    public class TicketListItemDto
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public string SenderName { get; set; }
        public string GymName { get; set; }
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }
        public int MessagesCount { get; set; }
        public DateTime LastActivity { get; set; }
        public bool HasUnreadFromOther { get; set; }
    }
}