using System.ComponentModel.DataAnnotations;

namespace EndPoint.Site.Areas.Admin.Models
{
    public class HelpContentCreateEditViewModel
    {
        public long Id { get; set; }

        [Display(Name = "کلید راهنما")]
        [Required]
        public string HelpKey { get; set; }

        [Display(Name = "عنوان")]
        [Required]
        public string Title { get; set; }

        [Display(Name = "متن راهنما")]
        public string Content { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
    }
}
