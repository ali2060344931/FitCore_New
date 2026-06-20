using FitCore.Domain.Entities.Tickets;

using System.Collections.Generic;

namespace FitCore.Application.Services.Tickets
{
    public class TicketDetailDto
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }
        public long SenderUserId { get; set; }
        public List<TicketMessageDto> Messages { get; set; }
    }
}