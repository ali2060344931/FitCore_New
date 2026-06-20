using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Users;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitCore.Domain.Entities.Tickets
{
    public enum TicketStatus
    {
        Open = 1,
        Answered = 2,
        Closed = 3
    }

    public enum TicketPriority
    {
        Low = 1,
        Normal = 2,
        High = 3
    }

    /// <summary>
    /// تیکت پشتیبانی:
    /// عضو → مدیر باشگاه (GymId پر است)
    /// مدیر باشگاه → مدیر سایت (GymId پر است، SenderRole = Admin)
    /// </summary>
    public class Ticket : BaseEntity
    {
        public long SenderUserId { get; set; }
        public AppUser SenderUser { get; set; }

        /// <summary>نقش فرستنده هنگام ارسال: Member یا Admin</summary>
        [MaxLength(20)]
        public string SenderRole { get; set; }

        /// <summary>باشگاهی که این تیکت به آن مربوط است (هم برای عضو، هم برای ادمین)</summary>
        public long? GymId { get; set; }
        public Gym Gym { get; set; }

        [Required, MaxLength(200)]
        public string Subject { get; set; }

        public TicketStatus Status { get; set; } = TicketStatus.Open;
        public TicketPriority Priority { get; set; } = TicketPriority.Normal;

        public List<TicketMessage> Messages { get; set; }
    }

    public class TicketMessage : BaseEntity
    {
        public long TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public long SenderUserId { get; set; }
        public AppUser SenderUser { get; set; }

        [MaxLength(20)]
        public string SenderRole { get; set; } // Member, Admin, SuperAdmin

        [Required]
        public string Body { get; set; }

        [MaxLength(500)]
        public string AttachmentPath { get; set; }
    }
}