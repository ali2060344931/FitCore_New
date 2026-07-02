using FitCore.Application.Contexts;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
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
                // --- تغییر اول: اضافه شدن Include های جدول واسط ---
                .Include(x => x.UnitConversions)
                    .ThenInclude(uc => uc.UnitType)
                // --------------------------------------------------
                .AsQueryable();

            // فیلتر بر اساس نام فارسی
            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                query = query.Where(x => x.Title.Contains(request.SearchKey));
            }

            // فیلتر بر اساس دسته‌بندی
            if (request.CategoryTypeId.HasValue && request.CategoryTypeId.Value > 0)
            {
                query = query.Where(x => x.CategoryTypeId == request.CategoryTypeId.Value);
            }

            // فیلتر بر اساس مالکیت (جدید)
            if (request.IsGlobalFilter.HasValue)
            {
                if (request.IsGlobalFilter.Value)
                {
                    // فقط غذاهای عمومی (سراسری)
                    query = query.Where(x => x.GymId == null);
                }
                else
                {
                    // فقط غذاهای متعلق به باشگاه
                    query = query.Where(x => x.GymId != null);
                }
            }

            var rowCount = await query.CountAsync();

            // مرتب‌سازی و صفحه‌بندی صحیح - فقط یک بار OrderBy قبل از Skip/Take
            var foods = await query
                .OrderBy(c => c.Title)
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
                    IsActive = x.IsActive,
                    IsGlobal = x.GymId == null,
                    GymName = x.Gym != null ? x.Gym.Name : null,

                    // در GetFoodsService بخش Select تغییر می‌کند:
                    Conversions = x.UnitConversions
    .Where(c => c.GymId == null || c.GymId == request.GymId) // ✅ فیلتر بر اساس باشگاه
    .Select(c => new FoodUnitConversionDto
    {
        UnitName = c.UnitType.Name,
        ConversionFactor = c.ConversionFactor
    }).ToList()                    // ----------------------------------------------------
                })
                .ToListAsync();

            return new ResultGetFoodsDto
            {
                Foods = foods,
                CurrentPage = request.Page,
                RowCount = rowCount,
                PageCount = (int)Math.Ceiling((double)rowCount / request.PageSize),
                PageSize = request.PageSize
            };
        }
    }
}