using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;

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
            // دریافت روز به همراه وعده‌ها و آیتم‌های غذایی
            var day = _context.NutritionProgramDays
                .Include(d => d.Meals)
                    .ThenInclude(m => m.Items)
                .FirstOrDefault(d => d.Id == request.Id);

            if (day == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "روز موردنظر یافت نشد"
                };
            }

            var now = DateTime.Now;

            // Soft Delete کردن روز
            day.IsRemoved = true;
            day.RemoveTime = now;

            // Soft Delete کردن تمام وعده‌های این روز
            if (day.Meals != null)
            {
                foreach (var meal in day.Meals)
                {
                    meal.IsRemoved = true;
                    meal.RemoveTime = now;

                    // Soft Delete کردن تمام آیتم‌های غذایی این وعده
                    if (meal.Items != null)
                    {
                        foreach (var item in meal.Items)
                        {
                            item.IsRemoved = true;
                            item.RemoveTime = now;
                        }
                    }
                }
            }

            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "روز به همراه تمام وعده‌های آن با موفقیت حذف شد"
            };
        }
    }

    public class RemoveNutritionDayDto
    {
        public long Id { get; set; }
    }
}