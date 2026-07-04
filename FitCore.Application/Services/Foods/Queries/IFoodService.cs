using FitCore.Application.Contexts;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static FitCore.Application.Services.Foods.Queries.FoodService;

namespace FitCore.Application.Services.Foods.Queries
{
    public interface IFoodService
    {
        long GetDefaultUnitId(long foodId);
        Task<FoodIndexResultDto> GetFoodsAsync(FoodIndexRequestDto request);
        Task<FoodCreateEditViewModel> GetFoodByIdAsync(long id);
        List<FoodAllowedUnitDto> GetAllowedUnitsForFood(long foodId);
    }

    public class FoodService : IFoodService
    {
        private readonly IDataBaseContext _context;

        public FoodService(IDataBaseContext context)
        {
            _context = context;
        }


        public async Task<FoodIndexResultDto> GetFoodsAsync(FoodIndexRequestDto request)
        {
            var foods = _context.Foods
                .Where(x => x.IsRemoved == false)
                .Include(x => x.CategoryType)
                .Include(x => x.DefaultUnit)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                foods = foods.Where(x =>
                    x.Title.Contains(request.SearchKey) ||
                    x.EnglishTitle.Contains(request.SearchKey) ||
                    x.CategoryType.Name.Contains(request.SearchKey) ||
                    x.DefaultUnit.Name.Contains(request.SearchKey));
            }

            int rowCount = await foods.CountAsync();

            var result = await foods
                .OrderByDescending(x => x.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new FoodIndexItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    EnglishTitle = x.EnglishTitle,
                    CategoryType = x.CategoryType.Name,
                    DefaultUnit = x.DefaultUnit.Name,
                    CaloriesPerUnit = x.CaloriesPerUnit,
                    ProteinPerUnit = x.ProteinPerUnit,
                    CarbohydratePerUnit = x.CarbohydratePerUnit,
                    FatPerUnit = x.FatPerUnit,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return new FoodIndexResultDto
            {
                Data = result,
                CurrentPage = request.Page,
                RowCount = rowCount,
                PageSize = request.PageSize,
                PageCount = (int)Math.Ceiling((double)rowCount / request.PageSize)
            };
        }


        public async Task<FoodCreateEditViewModel> GetFoodByIdAsync(long id)
        {
            var item = await _context.Foods
                .FirstOrDefaultAsync(x => x.Id == id && x.IsRemoved == false);

            if (item == null)
                return null;

            return new FoodCreateEditViewModel
            {
                Id = item.Id,
                Title = item.Title,
                EnglishTitle = item.EnglishTitle,
                CategoryTypeId = item.CategoryTypeId,
                CaloriesPerUnit = item.CaloriesPerUnit,
                ProteinPerUnit = item.ProteinPerUnit,
                CarbohydratePerUnit = item.CarbohydratePerUnit,
                FatPerUnit = item.FatPerUnit,
                DefaultUnitId = item.DefaultUnitId,
                IsActive = item.IsActive
            };
        }

        public long GetDefaultUnitId(long foodId)
        {
            // پیدا کردن غذا و دریافت واحد پیش‌فرض آن
            var food = _context.Foods.FirstOrDefault(f => f.Id == foodId);

            // اگر غذا یافت نشد یا واحدی نداشت، 0 برگردان
            return food?.DefaultUnitId ?? 0;
        }



        public List<FoodAllowedUnitDto> GetAllowedUnitsForFood(long foodId)
        {
            var food = _context.Foods
                .Include(f => f.DefaultUnit)
                .Include(f => f.UnitConversions)
                    .ThenInclude(uc => uc.UnitType)
                .FirstOrDefault(f => f.Id == foodId);

            if (food == null) return new List<FoodAllowedUnitDto>();

            var allowedUnits = new List<FoodAllowedUnitDto>
    {
        // همیشه واحد پیش‌فرض اضافه می‌شود
        new FoodAllowedUnitDto
        {
            Id = food.DefaultUnitId,
            Name = food.DefaultUnit?.Name ?? "واحد پیش‌فرض"
        }
    };

            // اضافه کردن واحدهای تبدیل (جلوگیری از تکرار واحد پیش‌فرض)
            if (food.UnitConversions != null)
            {
                foreach (var conv in food.UnitConversions)
                {
                    if (conv.UnitTypeId != food.DefaultUnitId && conv.UnitType != null)
                    {
                        allowedUnits.Add(new FoodAllowedUnitDto
                        {
                            Id = conv.UnitTypeId,
                            Name = conv.UnitType.Name
                        });
                    }
                }
            }

            return allowedUnits;
        }

        public class FoodCreateEditViewModel
        {
            public long? Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string EnglishTitle { get; set; } = string.Empty;
            public long CategoryTypeId { get; set; }
            public decimal CaloriesPerUnit { get; set; }
            public decimal ProteinPerUnit { get; set; }
            public decimal CarbohydratePerUnit { get; set; }
            public decimal FatPerUnit { get; set; }
            public long DefaultUnitId { get; set; }
            public bool IsActive { get; set; } = true;

            public List<SelectListItem> CategoryTypes { get; set; } = new();
            public List<SelectListItem> DefaultUnits { get; set; } = new();
        }

        public class FoodIndexItemDto
        {
            public long Id { get; set; }
            public string Title { get; set; }
            public string EnglishTitle { get; set; }
            public string CategoryType { get; set; }
            public string DefaultUnit { get; set; }
            public decimal CaloriesPerUnit { get; set; }
            public decimal ProteinPerUnit { get; set; }
            public decimal CarbohydratePerUnit { get; set; }
            public decimal FatPerUnit { get; set; }
            public bool IsActive { get; set; }
        }

        public class FoodIndexResultDto
        {
            public List<FoodIndexItemDto> Data { get; set; }
            public int CurrentPage { get; set; }
            public int PageCount { get; set; }
            public int RowCount { get; set; }
            public int PageSize { get; set; }
        }

        public class FoodIndexRequestDto
        {
            public string SearchKey { get; set; }
            public int Page { get; set; } = 1;
            public int PageSize { get; set; } = 20;
        }

        public class FoodAllowedUnitDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
