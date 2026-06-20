using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgram;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;

namespace FitCore.Application.Services.NutritionProgramBuilder.Commands.RemoveNutritionDay
{
    public interface IRemoveNutritionAllDayService
    {
        ResultDto Execute(RemoveAllNutritionDaysDto request);
    }

    public class RemoveNutritionAllDayService : IRemoveNutritionAllDayService
    {
        private readonly IDataBaseContext _context;

        public RemoveNutritionAllDayService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RemoveAllNutritionDaysDto request)
        {
            // دریافت تمام روزهای فعال برنامه به همراه وعده‌ها و آیتم‌های غذایی
            var days = _context.NutritionProgramDays
                .Include(d => d.Meals)
                    .ThenInclude(m => m.Items)
                .Where(d => d.NutritionProgramId == request.NutritionProgramId && !d.IsRemoved)
                .ToList();

            if (!days.Any())
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "هیچ روز فعالی برای این برنامه یافت نشد"
                };
            }

            var now = DateTime.Now;

            foreach (var day in days)
            {
                // Soft Delete کردن روز
                day.IsRemoved = true;
                day.RemoveTime = now;

                // Soft Delete کردن تمام وعده‌های این روز
                if (day.Meals != null)
                {
                    foreach (var meal in day.Meals.Where(m => !m.IsRemoved))
                    {
                        meal.IsRemoved = true;
                        meal.RemoveTime = now;

                        // Soft Delete کردن تمام آیتم‌های غذایی این وعده
                        if (meal.Items != null)
                        {
                            foreach (var item in meal.Items.Where(i => !i.IsRemoved))
                            {
                                item.IsRemoved = true;
                                item.RemoveTime = now;
                            }
                        }
                    }
                }
            }

            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = $"تمامی {days.Count} روز به همراه تمام وعده‌ها و غذاهای آن‌ها با موفقیت حذف شدند"
            };
        }
    }
    public class RemoveAllNutritionDaysDto
    {
        public long NutritionProgramId { get; set; }
    }
}