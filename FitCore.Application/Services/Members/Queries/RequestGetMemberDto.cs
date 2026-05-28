namespace FitCore.Application.Services.Member.Queries
{
    public class RequestGetMemberDto
    {
        public string SearchKey { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public long AppUserId { get; set; }
    }
}
