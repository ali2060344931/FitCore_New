using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionProgramBuilder.Commands.EditNutritionMealItem
{
    public interface IEditNutritionMealItemService
    {
        ResultDto Execute(EditNutritionMealItemDto request);
    }
    public class EditNutritionMealItemService : IEditNutritionMealItemService
    {
        private readonly IDataBaseContext _context;

        public EditNutritionMealItemService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(EditNutritionMealItemDto request)
        {
            var item = _context.NutritionMealItems.Find(request.Id);

            if (item == null)
                return new ResultDto { IsSuccess = false, Message = "آیتم یافت نشد" };

            item.FoodId = request.FoodId;
            item.Amount = request.Amount;
            item.UnitTypeId = request.UnitTypeId;
            item.Description= request.Description;
            _context.SaveChanges();

            return new ResultDto { IsSuccess = true, Message = "با موفقیت ویرایش شد" };
        }
    }

    public class EditNutritionMealItemDto
    {
        public long Id { get; set; }
        public long FoodId { get; set; }
        public decimal Amount { get; set; }
        public int UnitTypeId { get; set; }
        public string Description { get; set; }
    }
}
