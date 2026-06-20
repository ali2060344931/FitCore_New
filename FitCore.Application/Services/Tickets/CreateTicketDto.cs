using FitCore.Domain.Entities.Tickets;

namespace FitCore.Application.Services.Tickets
{
    public class CreateTicketDto
    {
        public long SenderUserId { get; set; }
        public string SenderRole { get; set; }
        public long? GymId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public TicketPriority Priority { get; set; } = TicketPriority.Normal;
    }
}