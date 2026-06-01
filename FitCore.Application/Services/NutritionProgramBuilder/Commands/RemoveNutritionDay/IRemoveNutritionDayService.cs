using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionProgramBuilder.Commands.RemoveNutritionDay
{
    public interface IRemoveNutritionDayService
    {
        ResultDto Execute(RemoveNutritionDayDto request);
    }

    public class RemoveNutritionDayService : IRemoveNutritionDayService
    {
        private readonly IDataBaseContext _context;

        public RemoveNutritionDayService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RemoveNutritionDayDto request)
        {
            var day = _context.NutritionProgramDays.Find(request.Id);

            if (day == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "روز موردنظر یافت نشد"
                };
            }

            var hasMeals = _context.NutritionMeals.Any(x => x.NutritionProgramDayId == request.Id);

            if (hasMeals)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این روز دارای وعده است. ابتدا وعده‌های این روز را حذف کنید."
                };
            }

            _context.NutritionProgramDays.Remove(day);
            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "روز با موفقیت حذف شد"
            };
        }
    }
    public class RemoveNutritionDayDto
    {
        public long Id { get; set; }
    }
}
