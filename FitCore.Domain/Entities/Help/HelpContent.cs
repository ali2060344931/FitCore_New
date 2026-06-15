using FitCore.Domain.Entities.Commons;

namespace FitCore.Domain.Entities.Help
{
    public class HelpContent : BaseEntity
    {
        public string HelpKey { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
    }
}
