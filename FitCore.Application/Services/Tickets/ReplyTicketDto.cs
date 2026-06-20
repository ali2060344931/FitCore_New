namespace FitCore.Application.Services.Tickets
{
    public class ReplyTicketDto
    {
        public long TicketId { get; set; }
        public long SenderUserId { get; set; }
        public string SenderRole { get; set; }
        public string Body { get; set; }
        public bool CloseTicket { get; set; }
    }
}