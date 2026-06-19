using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.Users;

using System.ComponentModel.DataAnnotations;

namespace FitCore.Domain.Entities.Tickets
{
    public class TicketMessage : BaseEntity
    {
        public long TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public string Text { get; set; }

        public long SenderUserId { get; set; }
        public AppUser SenderUser { get; set; }

        public bool IsRead { get; set; } = false;
    }
}