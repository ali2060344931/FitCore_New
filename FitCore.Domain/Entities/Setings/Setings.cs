namespace FitCore.Domain.Entities.Setings
{
    public class Setings
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string TextFilde { get; set; }
        public int NumberFilde { get; set; }
        public bool BoolFilde { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public long? SuperAdminChatId { get; set; }

    }
}
