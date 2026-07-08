using System.Collections.Generic;

namespace FitCore.Application.Services.Member.Queries
{
    public class ResultGetMembersDto
    {
        public List<ResultGetMemberDto> Members { get; set; }

        public int CurrentPage { get; set; }

        public int RowCount { get; set; }

        public int PageSize { get; set; }
        public int Rows { get; set; }

        public string ProfileImageUrl { get; set; }

    }
}
