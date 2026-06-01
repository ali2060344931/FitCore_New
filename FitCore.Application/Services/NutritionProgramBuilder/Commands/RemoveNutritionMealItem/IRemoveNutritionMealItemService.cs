using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionProgramBuilder.Commands.RemoveNutritionMealItem
{
    public interface IRemoveNutritionMealItemService
    {
        ResultDto Execute(RemoveNutritionMealItemDto request);
    }

    public class RemoveNutritionMealItemService : IRemoveNutritionMealItemService
    {
        private readonly IDataBaseContext _context;

        public RemoveNutritionMealItemService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RemoveNutritionMealItemDto request)
        {
            try
            {
                var item = _context.NutritionMealItems.Find(request.Id);

                if (item == null)
                {
                    return new ResultDto
                    {
                        IsSuccess = false,
                        Message = "آیتم موردنظر یافت نشد"
                    };
                }

                _context.NutritionMealItems.Remove(item);
                _context.SaveChanges();

                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "غذای وعده با موفقیت حذف شد"
                };
            }
            catch (Exception)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "خطا در حذف آیتم غذا"
                };
            }
        }
    }
    public class RemoveNutritionMealItemDto
    {
        public long Id { get; set; }
    }
}
