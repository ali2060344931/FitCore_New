using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.INutritionProgram;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionPrograms.Queries.GetNutritionProgram
{
    public class GetNutritionProgramsService : IGetNutritionProgramsService
    {
        private readonly IDataBaseContext _context;

        public GetNutritionProgramsService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultGetNutritionProgramsDto> Execute(RequestGetNutritionProgramsDto request)
        {
            var nutritionPrograms =
                _context.NutritionPrograms
                .Where(c => !c.IsRemoved)
                .Include(x => x.Member)
                .AsQueryable();


            // ✅ فیلتر بر اساس کاربر
            if (!request.IsAdmin)
            {
                nutritionPrograms =
                    nutritionPrograms
                    .Where(x => x.Member.AppUserId == request.AppUserId);
            }

            // ✅ فیلتر جستجو
            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                nutritionPrograms =
                    nutritionPrograms.Where(x =>
                        x.Member.AppUser.PhoneNumber.Contains(request.SearchKey) ||
                        x.GoalType.Name.Contains(request.SearchKey) ||
                        x.Member.AppUser.FullName.Contains(request.SearchKey) ||
                        x.ProgramType.Name.Contains(request.SearchKey) ||
                        x.StartDate.Contains(request.SearchKey) ||
                        x.EndDate.Contains(request.SearchKey));
            }

            int rowCount = await nutritionPrograms.CountAsync();

            var result =
                await nutritionPrograms
                .OrderByDescending(x => x.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new GetNutritionProgramsDto
                {
                    Id = x.Id,
                    GoalType = x.GoalType.Name,
                    ProgramType = x.ProgramType.Name,
                    MemberName = x.Member.AppUser.FullName,
                    MemberMobile = x.Member.AppUser.PhoneNumber,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsActive = x.IsActive,
                    CountProgramBuilder =
                        _context.NutritionProgramDays
                            .Count(c => c.NutritionProgramId == x.Id)
                })
                .ToListAsync();

            return new ResultGetNutritionProgramsDto
            {
                NutritionPrograms = result,
                CurrentPage = request.Page,
                RowCount = rowCount,
                PageCount = (int)Math.Ceiling((double)rowCount / request.PageSize),
                PageSize = request.PageSize
            };
        }


    }

}
