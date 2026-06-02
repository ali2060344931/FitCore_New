using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.NutritionProgram.Food;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Foods.Commands.CreateFood
{
    public interface IAddFoodService
    {
        Task<FitCore.Common.Dto.ResultDto> Execute(CreateFoodDto request);
    }
    public class AddFoodService : IAddFoodService
    {
        private readonly IDataBaseContext _context;

        public AddFoodService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Execute(CreateFoodDto request)
        {


            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "نـــام غذا را وارد نمائید"
                };
            }

            if (request.CategoryTypeId==0)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "دسته بندی(گروه) را انتخاب نمائید"
                };
            }


            if (request.DefaultUnitId==0)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "واحد اندازگیری غذا را انتخاب نمائید"
                };
            }


            var foodlist =_context.Foods.Any(c=>c.Title == request.Title && c.CategoryTypeId==request.CategoryTypeId);
            if(foodlist)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "غذای مورد نظر قبلا در این گروه ثبت گردید"
                };

            }
            var food = new Food
            {
                Title = request.Title,
                EnglishTitle = request.EnglishTitle,
                CategoryTypeId = request.CategoryTypeId,
                CaloriesPerUnit = request.CaloriesPerUnit,
                ProteinPerUnit = request.ProteinPerUnit,
                CarbohydratePerUnit = request.CarbohydratePerUnit,
                FatPerUnit = request.FatPerUnit,
                DefaultUnitId = request.DefaultUnitId,
                IsActive = request.IsActive
            };

            await _context.Foods.AddAsync(food);
            await _context.SaveChangesAsync();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "غذا با موفقیت ثبت شد. "+request.Title
            };
        }
    }
}
