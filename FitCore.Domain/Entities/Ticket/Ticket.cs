using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Users;

using System.Collections.Generic;

namespace FitCore.Domain.Entities.Tickets
{
    public class Ticket : BaseEntity
    {
        public string Title { get; set; }
        public string Message { get; set; } // پیام اولیه عضو

        public TicketStatus Status { get; set; } = TicketStatus.Pending;

        // ارسال کننده (عضو)
        public long SenderUserId { get; set; }
        public AppUser SenderUser { get; set; }

        // دریافت کننده (مدیر باشگاه)
        public long? ReceiverUserId { get; set; }
        public AppUser ReceiverUser { get; set; }

        // برای فیلترینگ داده‌ها بر اساس باشگاه
        public long? GymId { get; set; }
        public Gym Gym { get; set; }


        public ICollection<TicketMessage> Messages { get; set; } = new List<TicketMessage>();
    }

    public enum TicketStatus
    {
        Pending = 1,
        Answered = 2,
        Closed = 3,
        Cleared = 4

    }
}
