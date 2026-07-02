using System.Collections.Generic;

namespace FitCore.Application.Services.Foods.Queries
{
    public class ResultGetFoodsDto
    {
        public List<FoodDto> Foods { get; set; }

        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }
        public int PageSize { get; set; }
        /// <summary>
        /// آیا این حرکت سراسری (مشترک بین همه باشگاه‌ها) است؟
        /// </summary>
        public bool IsGlobal { get; set; }

        /// <summary>
        /// نام باشگاه صاحب حرکت (در صورتی که سراسری نباشد)
        /// </summary>
        public string GymName { get; set; }


    }


}
