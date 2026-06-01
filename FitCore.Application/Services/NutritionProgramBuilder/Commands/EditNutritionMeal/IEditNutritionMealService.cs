using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionProgramBuilder.Commands.EditNutritionMeal
{
    public interface IEditNutritionMealService
    {
        ResultDto Execute(EditNutritionMealDto request);
    }


    public class EditNutritionMealService : IEditNutritionMealService
    {
        private readonly IDataBaseContext _context;

        public EditNutritionMealService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(EditNutritionMealDto request)
        {
            var meal = _context.NutritionMeals.Find(request.Id);

            if (meal == null)
                return new ResultDto { IsSuccess = false, Message = "وعده یافت نشد" };

            meal.Title = request.Title;
            // meal.Description = request.Description;

            _context.SaveChanges();

            return new ResultDto { IsSuccess = true, Message = "وعده با موفقیت ویرایش شد" };
        }
    }

    public class EditNutritionMealDto
    {
        public long Id { get; set; }
        public string Title { get; set; } // مثلا: صبحانه، ناهار
        // اگر فیلدهای دیگری مثل Description یا Time دارید اینجا اضافه کنید
    }
}
