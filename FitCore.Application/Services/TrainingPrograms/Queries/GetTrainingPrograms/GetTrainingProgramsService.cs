using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.TrainingPrograms.Queries.GetTrainingPrograms
{
    public class GetTrainingProgramsService : IGetTrainingProgramsService
    {
        private readonly IDataBaseContext _context;

        public GetTrainingProgramsService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<GetTrainingProgramsResultDto>> Execute(GetTrainingProgramsRequestDto request)
        {
            var trainingPrograms =
                _context.TrainingPrograms
                .Where(c => !c.IsRemoved)
                .Include(x => x.Member)
                .Include(x => x.TrainingProgramType)
                .Include(x => x.TrainingGoalType)
                .Include(x => x.Days)
                .AsQueryable();

            //====================================
            // فیلتر بر اساس کاربر
            //====================================

            if (!request.IsAdmin)
            {
                trainingPrograms =
                    trainingPrograms
                    .Where(x => x.Member.AppUserId == request.AppUserId);
            }

            //====================================
            // فیلتر جستجو
            //====================================

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                trainingPrograms =
                    trainingPrograms.Where(x =>
                        x.Member.AppUser.PhoneNumber.Contains(request.SearchKey) ||
                        x.Member.AppUser.FullName.Contains(request.SearchKey) ||
                        x.Title.Contains(request.SearchKey) ||
                        x.TrainingProgramType.Name.Contains(request.SearchKey) ||
                        x.TrainingGoalType.Name.Contains(request.SearchKey) ||
                        x.StartDate.Contains(request.SearchKey) ||
                        x.EndDate.Contains(request.SearchKey));
            }

            int rowCount = await trainingPrograms.CountAsync();

            var result =
                await trainingPrograms
                .OrderByDescending(x => x.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new GetTrainingProgramsDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    TrainingProgramType = x.TrainingProgramType.Name,
                    TrainingGoalType = x.TrainingGoalType.Name,
                    MemberName = x.Member.AppUser.FullName,
                    MemberMobile = x.Member.AppUser.PhoneNumber,
                    BaleChatId = x.Member.AppUser.BaleChatId,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    SessionsPerWeek = x.SessionsPerWeek,
                    IsActive = x.IsActive,
                    CountTrainingDays = x.Days.Count(c => !c.IsRemoved)
                })
                .ToListAsync();

            return new ResultDto<GetTrainingProgramsResultDto>
            {
                IsSuccess = true,
                Data = new GetTrainingProgramsResultDto
                {
                    TrainingPrograms = result,
                    CurrentPage = request.Page,
                    RowCount = rowCount,
                    PageCount = (int)Math.Ceiling((double)rowCount / request.PageSize),
                    PageSize = request.PageSize
                }
            };
        }
    }
}
