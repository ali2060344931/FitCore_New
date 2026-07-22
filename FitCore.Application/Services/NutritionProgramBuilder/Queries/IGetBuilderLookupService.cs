using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.NutritionProgram.NutritionMeal;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;

namespace FitCore.Application.Services.NutritionProgramBuilder.Queries
{
    public interface IGetBuilderLookupService
    {
        ResultDto<BuilderLookupDto> Execute();
    }


    public class GetBuilderLookupService : IGetBuilderLookupService
    {
        private readonly IDataBaseContext _context;

        public GetBuilderLookupService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<BuilderLookupDto> Execute()
        {
            //var foods = _context.Foods
            //    .Select(x => new LookupItemDto
            //    {
            //        Id = x.Id,
            //        Name = x.Title,
            //        CategoryTypeId = x.CategoryTypeId
            //    }).OrderBy(c=>c.Name).ToList();

            var foods = _context.Foods
    .Select(x => new LookupItemDto
    {
        Id = x.Id,
        Name = x.Title,
        CategoryTypeId = x.CategoryTypeId,
        IsGlobal = x.GymId == null   // ← اضافه شد
    })
    .OrderBy(c => c.Name)
    .ToList();



            var units = _context.NutritionUnitTypes
                .Select(x => new LookupItemDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).OrderBy(c => c.Name).ToList();


            var mealTypes = _context.MealTypes
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new LookupItemDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

            return new ResultDto<BuilderLookupDto>
            {
                IsSuccess = true,
                Data = new BuilderLookupDto
                {
                    Foods = foods,
                    Units = units,
                    MealTypes = mealTypes

                }
            };
        }
    }
    public class BuilderLookupDto
    {
        public List<LookupItemDto> Foods { get; set; } = new();
        public List<LookupItemDto> Units { get; set; } = new();
        public List<LookupItemDto> MealTypes { get; set; } = new();   // NEW
    }

    public class LookupItemDto
    {
        public long Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// فقط برای غذاها استفاده می‌شود — شناسه گروه غذایی
        /// </summary>
        public int? CategoryTypeId { get; set; }

        /// <summary>
        /// اگر null باشد یعنی غذا عمومی (سراسری) است
        /// </summary>
        public bool IsGlobal { get; set; }
    }
}
