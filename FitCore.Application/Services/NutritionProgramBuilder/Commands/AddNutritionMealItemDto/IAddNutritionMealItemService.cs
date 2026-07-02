using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.NutritionProgram.Food; // اضافه شدن_using_
using FitCore.Domain.Entities.NutritionProgram.NutritionMealItem;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionProgramBuilder.Commands.AddNutritionMealItemDto
{
    public interface IAddNutritionMealItemService
    {
        ResultDto<long> Execute(AddNutritionMealItemDto request);
    }

    public class AddNutritionMealItemService : IAddNutritionMealItemService
    {
        private readonly IDataBaseContext _context;

        public AddNutritionMealItemService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<long> Execute(AddNutritionMealItemDto request)
        {
            bool isDuplicate = _context.NutritionMealItems
                .Any(x => x.NutritionMealId == request.NutritionMealId && x.FoodId == request.FoodId);

            if (isDuplicate)
            {
                return new ResultDto<long>
                {
                    IsSuccess = false,
                    Message = "این غذا قبلاً در این وعده ثبت شده است."
                };
            }

            if (request.NutritionMealId <= 0) return new ResultDto<long> { IsSuccess = false, Message = "وعده غذایی نامعتبر است" };
            if (request.FoodId <= 0) return new ResultDto<long> { IsSuccess = false, Message = "غذا را انتخاب کنید" };
            if (request.UnitTypeId <= 0) return new ResultDto<long> { IsSuccess = false, Message = "واحد را انتخاب کنید" };
            if (request.Amount <= 0) return new ResultDto<long> { IsSuccess = false, Message = "مقدار باید بیشتر از صفر باشد" };

            var meal = _context.NutritionMeals.FirstOrDefault(x => x.Id == request.NutritionMealId);
            if (meal == null) return new ResultDto<long> { IsSuccess = false, Message = "وعده غذایی یافت نشد" };

            var food = _context.Foods.FirstOrDefault(x => x.Id == request.FoodId);
            if (food == null) return new ResultDto<long> { IsSuccess = false, Message = "غذای انتخاب شده یافت نشد" };

            var unit = _context.NutritionUnitTypes.FirstOrDefault(x => x.Id == request.UnitTypeId);
            if (unit == null) return new ResultDto<long> { IsSuccess = false, Message = "واحد انتخاب شده یافت نشد" };






            // --- منطق محاسبه دقیق با در نظر گرفتن ضریب تبدیل و اولویت باشگاه ---
            decimal conversionFactor = 1; // پیش‌فرض

            if (request.UnitTypeId != food.DefaultUnitId)
            {
                // جستجوی ضریب تبدیل
                // ✅ حذف کلمه Async از انتهای متد
                var unitConversion = _context.FoodUnitConversions
                    .Where(uc => uc.FoodId == request.FoodId && uc.UnitTypeId == request.UnitTypeId)
                    // ترفند اولویت‌بندی: اگر ضریب اختصاصی باشگاه وجود داشته باشد، بالاتر می‌آید
                    .OrderByDescending(uc => uc.GymId)
                    .Take(1)
                    .FirstOrDefault();

                if (unitConversion == null)
                {
                    return new ResultDto<long>
                    {
                        IsSuccess = false,
                        Message = $"ضریب تبدیل برای واحد ({unit.Name}) روی غذا ({food.Title}) تعریف نشده است."
                    };
                }

                conversionFactor = unitConversion.ConversionFactor;
            }
            // -----------------------------------------------------


            // محاسبه معادل مقدار وارد شده در واحد پیش‌فرض غذا
            // مثال: کاربر وارد کرده 2 پیمانه. ضریب تبدیل پیمانه به گرم 180 است.
            // معادل گرم می‌شود: 2 * 180 = 360 گرم
            decimal equivalentAmountInDefaultUnit = request.Amount * conversionFactor;

            // محاسبه نهایی ماکروها (ضرب در مقدار هر واحد پیش‌فرض)
            decimal calculatedCalories = equivalentAmountInDefaultUnit * food.CaloriesPerUnit;
            decimal calculatedProtein = equivalentAmountInDefaultUnit * food.ProteinPerUnit;
            decimal calculatedCarbohydrate = equivalentAmountInDefaultUnit * food.CarbohydratePerUnit;
            decimal calculatedFat = equivalentAmountInDefaultUnit * food.FatPerUnit;
            // -----------------------------------------------------


            var item = new NutritionMealItem
            {
                NutritionMealId = request.NutritionMealId,
                FoodId = request.FoodId,
                Amount = request.Amount,
                UnitTypeId = request.UnitTypeId,
                Description = request.Description,

                // ثبت مقادیر محاسبه شده به عنوان اسنپ‌شات
                Calories = calculatedCalories,
                Protein = calculatedProtein,
                Carbohydrate = calculatedCarbohydrate,
                Fat = calculatedFat
            };

            _context.NutritionMealItems.Add(item);
            _context.SaveChanges();

            return new ResultDto<long>
            {
                IsSuccess = true,
                Message = "آیتم غذایی با موفقیت ثبت شد.\nغذای بعدی را هم میتوانید اضافه نمائید",
                Data = item.Id
            };
        }
    }

    public class AddNutritionMealItemDto
    {
        public long NutritionMealId { get; set; }
        public long FoodId { get; set; }
        public decimal Amount { get; set; }
        public int UnitTypeId { get; set; }
        public string Description { get; set; }
    }
}