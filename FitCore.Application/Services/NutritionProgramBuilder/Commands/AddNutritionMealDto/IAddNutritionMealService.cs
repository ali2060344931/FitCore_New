using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.NutritionProgram.NutritionMeal;

using System;
using System.Linq;

namespace FitCore.Application.Services.NutritionProgramBuilder.Commands.AddNutritionMealDto
{
    public interface IAddNutritionMealService
    {
        ResultDto<long> Execute(AddNutritionMealDto request);
    }
    public class AddNutritionMealService : IAddNutritionMealService
    {
        private readonly IDataBaseContext _context;

        public AddNutritionMealService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<long> Execute(AddNutritionMealDto request)
        {
            if (request.NutritionProgramDayId <= 0)
                return new ResultDto<long> { IsSuccess = false, Message = "روز برنامه نامعتبر است" };

            //if (string.IsNullOrWhiteSpace(request.Title))
            //    return new ResultDto<long> { IsSuccess = false, Message = "عنوان وعده را وارد کنید" };

            if (request.MealTypeId <= 0)
                return new ResultDto<long> { IsSuccess = false, Message = "نوع وعده را انتخاب کنید" };

            var dayExists = _context.NutritionProgramDays.Any(x => x.Id == request.NutritionProgramDayId);
            if (!dayExists)
                return new ResultDto<long> { IsSuccess = false, Message = "روز برنامه یافت نشد" };


            var mealTypeExists = _context.MealTypes.Any(x => x.Id == request.MealTypeId);
            if (!mealTypeExists)
                return new ResultDto<long> { IsSuccess = false, Message = "نوع وعده نامعتبر است" };

            //بررسی تکراری بودن وعده در یک روز
            var CheackRepid = _context.NutritionMeals.Any(c => c.MealTypeId == request.MealTypeId && c.NutritionProgramDayId == request.NutritionProgramDayId);
            if (CheackRepid)
                return new ResultDto<long> { IsSuccess = false, Message = "وعده مورد نظر قبل در این روز ثبت گردید" };

            var meal = new NutritionMeal
            {
                NutritionProgramDayId = request.NutritionProgramDayId,
                //Title = request.Title.Trim(),
                MealTypeId = request.MealTypeId,
                //MealTime= TimeSpan.Parse("08:30")
            };

            _context.NutritionMeals.Add(meal);
            _context.SaveChanges();

            return new ResultDto<long>
            {
                IsSuccess = true,
                Message = "وعده جدید با موفقیت ثبت شد",
                Data = meal.Id
            };
        }
    }

    public class AddNutritionMealDto
    {
        public long NutritionProgramDayId { get; set; }
        public string Title { get; set; }
        public int MealTypeId { get; set; }

    }
}
