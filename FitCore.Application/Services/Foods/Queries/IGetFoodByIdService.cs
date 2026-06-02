using FitCore.Application.Contexts;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Foods.Queries
{
    public interface IGetFoodByIdService
    {
        Task<FoodDto?> Execute(long id);
    }
    public class GetFoodByIdService : IGetFoodByIdService
    {
        private readonly IDataBaseContext _context;

        public GetFoodByIdService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<FoodDto?> Execute(long id)
        {
            return await _context.Foods
                .Include(x => x.CategoryType)
                .Include(x => x.DefaultUnit)
                .Where(x => x.Id == id)
                .Select(x => new FoodDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    EnglishTitle = x.EnglishTitle,
                    CategoryTypeId = x.CategoryTypeId,
                    CategoryTypeName = x.CategoryType.Name,
                    CaloriesPerUnit = x.CaloriesPerUnit,
                    ProteinPerUnit = x.ProteinPerUnit,
                    CarbohydratePerUnit = x.CarbohydratePerUnit,
                    FatPerUnit = x.FatPerUnit,
                    DefaultUnitId = x.DefaultUnitId,
                    DefaultUnitName = x.DefaultUnit.Name,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync();
        }
    }
}
