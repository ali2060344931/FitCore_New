using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.NutritionProgram.NutritionMealItem;

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
            if (request.NutritionMealId <= 0)
            {
                return new ResultDto<long>
                {
                    IsSuccess = false,
                    Message = "وعده غذایی نامعتبر است"
                };
            }

            if (request.FoodId <= 0)
            {
                return new ResultDto<long>
                {
                    IsSuccess = false,
                    Message = "غذا را انتخاب کنید"
                };
            }

            if (request.UnitTypeId <= 0)
            {
                return new ResultDto<long>
                {
                    IsSuccess = false,
                    Message = "واحد را انتخاب کنید"
                };
            }

            if (request.Amount <= 0)
            {
                return new ResultDto<long>
                {
                    IsSuccess = false,
                    Message = "مقدار باید بیشتر از صفر باشد"
                };
            }

            var meal = _context.NutritionMeals
                .FirstOrDefault(x => x.Id == request.NutritionMealId);

            if (meal == null)
            {
                return new ResultDto<long>
                {
                    IsSuccess = false,
                    Message = "وعده غذایی یافت نشد"
                };
            }

            var food = _context.Foods.FirstOrDefault(x => x.Id == request.FoodId);
            if (food == null)
            {
                return new ResultDto<long>
                {
                    IsSuccess = false,
                    Message = "غذای انتخاب شده یافت نشد"
                };
            }

            var unit = _context.NutritionUnitTypes.FirstOrDefault(x => x.Id == request.UnitTypeId);
            if (unit == null)
            {
                return new ResultDto<long>
                {
                    IsSuccess = false,
                    Message = "واحد انتخاب شده یافت نشد"
                };
            }

            var item = new NutritionMealItem
            {
                NutritionMealId = request.NutritionMealId,
                FoodId = request.FoodId,
                Amount = request.Amount,
                UnitTypeId = request.UnitTypeId
            };

            _context.NutritionMealItems.Add(item);
            _context.SaveChanges();

            return new ResultDto<long>
            {
                IsSuccess = true,
                Message = "آیتم غذایی با موفقیت ثبت شد",
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
    }
}
