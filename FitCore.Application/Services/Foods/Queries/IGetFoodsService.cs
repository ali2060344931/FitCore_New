using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Foods.Queries
{
    public interface IGetFoodsService
    {
        Task<ResultGetFoodsDto> Execute(RequestGetFoodsDto request);
    }

    public class GetFoodsService : IGetFoodsService
    {
        private readonly IDataBaseContext _context;

        public GetFoodsService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultGetFoodsDto> Execute(RequestGetFoodsDto request)
        {
            var query = _context.Foods
                .Include(x => x.CategoryType)
                .Include(x => x.DefaultUnit)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                query = query.Where(x =>
                    x.Title.Contains(request.SearchKey) ||
                    x.EnglishTitle.Contains(request.SearchKey)||
                    x.CategoryType.Name.Contains(request.SearchKey)||
                    x.DefaultUnit.Name.Contains(request.SearchKey)
                   
                    );
            }

            var rowCount = await query.CountAsync();

            var foods = await query
                .OrderByDescending(x => x.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
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
                }).OrderBy(c=>c.Title)
                .ToListAsync();

            return  new ResultGetFoodsDto
            {
                Foods = foods,

                CurrentPage = request.Page,

                RowCount = rowCount,

                PageCount =
                    (int)Math.Ceiling(
                        (double)rowCount / request.PageSize),
                PageSize = request.PageSize
            };
        }
    }
}
