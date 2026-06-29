using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EndPoint.Site.Areas.Admin.Models
{
    // کلاس برای هر ردیف از اعضا
    // اینوم وضعیت ارسال
    public enum MemberFilterType
    {
        [Display(Name = "فقط اعضای فعال")]
        Active = 1,

        [Display(Name = "فقط اعضای منقضی")]
        Expired = 2,

        [Display(Name = "همه اعضا (فعال و منقضی)")]
        All = 3
    }

    public class MemberSelectItem
    {
        public long Id { get; set; }
        public long BaleChatId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsSelected { get; set; } = false;
        public bool IsExpired { get; set; } = false;
    }

    public class SendBaleBulkMessageViewModel
    {
        [DisplayName("متن پیام")]
        public string MessageText { get; set; }

        public List<MemberSelectItem> Members { get; set; } = new List<MemberSelectItem>();

        // فیلد جایگزین شده: انتخاب نوع گیرندگان
        [DisplayName("نوع گیرندگان پیام")]
        public MemberFilterType FilterType { get; set; } = MemberFilterType.Active;

        public int SentCount { get; set; }
        public int FailedCount { get; set; }
        public bool IsSubmitted { get; set; }
    }
}
