using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.INutritionProgram;
using FitCore.Application.Services.Member.Queries;
using FitCore.Application.Services.NutritionPrograms.Commands.AddNutritionProgram;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Members;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionPrograms.Queries.GetNutritionProgram
{
    public class GetNutritionProgramsService :IGetNutritionProgramsService
    {
        private readonly IDataBaseContext _context;

        public GetNutritionProgramsService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultGetNutritionProgramsDto> Execute(RequestGetNutritionProgramsDto request)
        {
            int pageSize = 20;

            var nutritionPrograms =
                _context.NutritionPrograms
                .Include(x => x.Member)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                nutritionPrograms =
                    nutritionPrograms.Where(x =>
                        x.Title.Contains(request.SearchKey) ||
                        x.GoalType.Name.Contains(request.SearchKey) ||
                        x.Member.AppUser.FullName.Contains(request.SearchKey) ||
                        x.ProgramType.Name.Contains(request.SearchKey) ||
                        x.StartDate.Contains(request.SearchKey) ||
                        x.EndDate.Contains(request.SearchKey));
            }

            int rowCount =
                await nutritionPrograms.CountAsync();

            var result =
                await nutritionPrograms
                .OrderByDescending(x => x.Id)
                .Skip((request.Page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new GetNutritionProgramsDto
                {
                    Id = x.Id,

                    Title = x.Title,

                    GoalType = x.GoalType.Name,
                    ProgramType=x.ProgramType.Name,
                    MemberName =x.Member.AppUser.FullName,

                    StartDate = x.StartDate,

                    EndDate = x.EndDate,

                    IsActive = x.IsActive
                })
                .ToListAsync();

            return new ResultGetNutritionProgramsDto
            {
                NutritionPrograms = result,

                CurrentPage = request.Page,

                RowCount = rowCount,

                PageCount =
                    (int)Math.Ceiling(
                        (double)rowCount / pageSize)
            };
        }
    }

}
