using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Foods.Commands.EditFood
{
    public interface IEditFoodService
    {
        Task<FitCore.Common.Dto.ResultDto> Execute(UpdateFoodDto request);
    }
    public class EditFoodService : IEditFoodService
    {
        private readonly IDataBaseContext _context;

        public EditFoodService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Execute(UpdateFoodDto request)
        {
            var food = await _context.Foods.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (food == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "غذا یافت نشد."
                };
            }
            food.Title = request.Title;
            food.EnglishTitle = request.EnglishTitle;
            food.CategoryTypeId = request.CategoryTypeId;
            food.CaloriesPerUnit = request.CaloriesPerUnit;
            food.ProteinPerUnit = request.ProteinPerUnit;
            food.CarbohydratePerUnit = request.CarbohydratePerUnit;
            food.FatPerUnit = request.FatPerUnit;
            food.DefaultUnitId = request.DefaultUnitId;
            food.IsActive = request.IsActive;

            await _context.SaveChangesAsync();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "غذا با موفقیت ویرایش شد."
            };
        }
    }
}
