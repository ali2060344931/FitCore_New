using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.INutritionProgram;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Security.Cryptography.Xml;
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

            // ✅ عضو فقط برنامه‌های خودش را ببیند
            if (!request.IsAdmin && !request.IsTrainer)
            {
                nutritionPrograms = nutritionPrograms
                    .Where(x => x.Member.AppUserId == request.AppUserId);
            }

            // ✅ مدیر/مربی فقط برنامه‌های باشگاه خودش را ببیند
            if (request.IsAdmin || request.IsTrainer)
            {
                if (request.GymId.HasValue && request.GymId.Value > 0)
                {
                    nutritionPrograms = nutritionPrograms
                        .Where(x => x.GymId == request.GymId.Value);
                }
                else
                {
                    nutritionPrograms = nutritionPrograms.Where(x => false);
                }
            }

            // ✅ جستجو
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
                    IsSeen=x.IsSeen,
                    BaleChatId = x.Member.AppUser.BaleChatId,
                    CountProgramBuilder = _context.NutritionProgramDays.Count(c => c.NutritionProgramId == x.Id)
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
