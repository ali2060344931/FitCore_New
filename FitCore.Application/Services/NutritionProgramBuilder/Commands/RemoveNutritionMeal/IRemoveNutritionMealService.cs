using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionProgramBuilder.Commands.RemoveNutritionMeal
{
    public interface IRemoveNutritionMealService
    {
        ResultDto Execute(RemoveNutritionMealDto request);
    }
    public class RemoveNutritionMealService : IRemoveNutritionMealService
    {
        private readonly IDataBaseContext _context;

        public RemoveNutritionMealService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RemoveNutritionMealDto request)
        {
            // 1. پیدا کردن وعده همراه با چک کردن وجود آیتم‌های آن
            // استفاده از Any برای پرفورمنس بهتر (نیاز به واکشی لیست غذاها نیست)
            var hasFoodItems = _context.NutritionMealItems.Any(x => x.NutritionMealId == request.Id);

            if (hasFoodItems)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این وعده دارای غذا است! ابتدا باید غذاهای داخل آن را حذف کنید."
                };
            }

            var meal = _context.NutritionMeals.Find(request.Id);

            if (meal == null)
                return new ResultDto { IsSuccess = false, Message = "وعده موردنظر یافت نشد" };

            _context.NutritionMeals.Remove(meal);
            _context.SaveChanges();

            return new ResultDto { IsSuccess = true, Message = "وعده با موفقیت حذف شد" };
        }
    }


    public class RemoveNutritionMealDto
    {
        public long Id { get; set; }
    }
}
