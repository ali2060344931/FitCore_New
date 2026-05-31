using System.Collections.Generic;

namespace EndPoint.Site.Areas.Admin.Models
{
    public class PagingPartialViewModel
    {
        public int RowCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string PageQueryKey { get; set; } = "page";
        public string PageSizeQueryKey { get; set; } = "PageSize";
        public List<int> PageSizeOptions { get; set; } = new() { 5, 10, 20, 50, 100, 200 };
    }
}
