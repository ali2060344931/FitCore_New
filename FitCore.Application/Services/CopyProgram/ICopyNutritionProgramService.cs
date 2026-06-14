using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.NutritionProgram.NutritionMeal;
using FitCore.Domain.Entities.NutritionProgram.NutritionMealItem;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgramDay;

using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.CopyPrograms
{
    public interface ICopyNutritionProgramService
    {
        Task<ResultDto<long>> Execute(CopyNutritionProgramDto request);
    }

    public class CopyNutritionProgramDto
    {
        /// <summary>
        /// شناسه برنامه غذایی مبدأ (که می‌خواهیم کپی شود)
        /// </summary>
        [Required]
        public long SourceProgramId { get; set; }

        /// <summary>
        /// شناسه عضو مقصد (که برنامه برای او کپی می‌شود)
        /// </summary>
        [Required]
        public long TargetMemberId { get; set; }

        /// <summary>
        /// شناسه کاربری که عملیات کپی را انجام می‌دهد
        /// </summary>
        public long CreatedByUserId { get; set; }

        /// <summary>
        /// تاریخ شروع جدید برای برنامه کپی‌شده
        /// </summary>
        [Required]
        public string NewStartDate { get; set; }

        /// <summary>
        /// تاریخ پایان جدید برای برنامه کپی‌شده
        /// </summary>
        [Required]
        public string NewEndDate { get; set; }
    }

    public class CopyNutritionProgramService : ICopyNutritionProgramService
    {
        private readonly IDataBaseContext _context;

        public CopyNutritionProgramService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<long>> Execute(CopyNutritionProgramDto request)
        {
            //====================================
            // بارگذاری کامل برنامه مبدأ با تمام جزئیات
            //====================================

            var source =
                await _context.NutritionPrograms
                .Include(p => p.Days)
                    .ThenInclude(d => d.Meals)
                        .ThenInclude(m => m.Items)
                .FirstOrDefaultAsync(p =>
                    p.Id == request.SourceProgramId &&
                    !p.IsRemoved);

            if (source == null)
            {
                return new ResultDto<long>
                {
                    IsSuccess = false,
                    Message = "برنامه غذایی مبدأ یافت نشد",
                    Data = 0
                };
            }

            //====================================
            // بررسی عضو مقصد
            //====================================

            var targetMember =
                await _context.Members
                .FirstOrDefaultAsync(m =>
                    m.Id == request.TargetMemberId);

            if (targetMember == null)
            {
                return new ResultDto<long>
                {
                    IsSuccess = false,
                    Message = "عضو مقصد یافت نشد",
                    Data = 0
                };
            }

            //====================================
            // ساخت برنامه جدید (کپی عمیق)
            //====================================

            var newProgram =
                new FitCore.Domain.Entities.NutritionProgram.NutritionProgram.NutritionProgram
                {
                    MemberId = request.TargetMemberId,
                    GymId = source.GymId,
                    CreatedByUserId = request.CreatedByUserId,
                    ProgramTypeId = source.ProgramTypeId,
                    GoalTypeId = source.GoalTypeId,
                    Description = source.Description,
                    StartDate = request.NewStartDate,
                    EndDate = request.NewEndDate,
                    IsActive = true,
                    InsertTime = DateTime.Now,
                };

            await _context.NutritionPrograms.AddAsync(newProgram);
            await _context.SaveChangesAsync();  // برای دریافت Id جدید

            //====================================
            // کپی روزها
            //====================================

            if (source.Days != null)
            {
                foreach (var sourceDay in source.Days.Where(d => !d.IsRemoved).OrderBy(d => d.DayNumber))
                {
                    var newDay = new NutritionProgramDay
                    {
                        NutritionProgramId = newProgram.Id,
                        DayNumber = sourceDay.DayNumber,
                        Title = sourceDay.Title,
                        InsertTime = DateTime.Now,
                    };

                    await _context.NutritionProgramDays.AddAsync(newDay);
                    await _context.SaveChangesAsync();

                    //====================================
                    // کپی وعده‌های هر روز
                    //====================================

                    if (sourceDay.Meals == null) continue;

                    foreach (var sourceMeal in sourceDay.Meals.Where(m => !m.IsRemoved).OrderBy(m => m.SortOrder))
                    {
                        var newMeal = new NutritionMeal
                        {
                            NutritionProgramDayId = newDay.Id,
                            MealTypeId = sourceMeal.MealTypeId,
                            Title = sourceMeal.Title,
                            Description = sourceMeal.Description,
                            MealTime = sourceMeal.MealTime,
                            SortOrder = sourceMeal.SortOrder,
                            TotalCalories = sourceMeal.TotalCalories,
                            TotalProtein = sourceMeal.TotalProtein,
                            TotalCarbohydrate = sourceMeal.TotalCarbohydrate,
                            TotalFat = sourceMeal.TotalFat,
                            InsertTime = DateTime.Now,
                        };

                        await _context.NutritionMeals.AddAsync(newMeal);
                        await _context.SaveChangesAsync();

                        //====================================
                        // کپی آیتم‌های هر وعده
                        //====================================

                        if (sourceMeal.Items == null) continue;

                        foreach (var sourceItem in sourceMeal.Items.Where(i => !i.IsRemoved))
                        {
                            var newItem = new NutritionMealItem
                            {
                                NutritionMealId = newMeal.Id,
                                FoodId = sourceItem.FoodId,
                                Amount = sourceItem.Amount,
                                UnitTypeId = sourceItem.UnitTypeId,
                                Description = sourceItem.Description,
                                Calories = sourceItem.Calories,
                                Protein = sourceItem.Protein,
                                Carbohydrate = sourceItem.Carbohydrate,
                                Fat = sourceItem.Fat,
                                InsertTime = DateTime.Now,
                            };

                            await _context.NutritionMealItems.AddAsync(newItem);
                        }

                        await _context.SaveChangesAsync();
                    }
                }
            }

            return new ResultDto<long>
            {
                IsSuccess = true,
                Message = $"برنامه غذایی با موفقیت برای عضو مقصد کپی شد ({source.Days?.Count ?? 0} روز)",
                Data = newProgram.Id
            };
        }
    }
}