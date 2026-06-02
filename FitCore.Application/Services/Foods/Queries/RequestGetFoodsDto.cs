namespace FitCore.Application.Services.Foods.Queries
{
    public class RequestGetFoodsDto
    {

        public string SearchKey { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

    }
}
