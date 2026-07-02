namespace FitCore.Application.Services.Foods.Queries
{
    public class RequestGetFoodsDto
    {

        public string SearchKey { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        /// <summary>
        /// شناسه باشگاه کاربر جاری (مدیر باشگاه).
        /// اگر IsAdmin = true باشد، این مقدار نادیده گرفته می‌شود.
        /// </summary>
        public long? GymId { get; set; }

        /// <summary>
        /// آیا کاربر جاری مدیر کل (SuperAdmin) است؟
        /// در این صورت همه حرکات (همه باشگاه‌ها + سراسری) قابل مشاهده است.
        /// </summary>
        public bool IsAdmin { get; set; }


        /// <summary>
        /// فیلتر بر اساس دسته‌بندی غذایی
        /// </summary>
        public int? CategoryTypeId { get; set; }

        /// <summary>
        /// فیلتر مالکیت: true = عمومی، false = متعلق به باشگاه، null = همه
        /// </summary>
        public bool? IsGlobalFilter { get; set; }


    }
}
