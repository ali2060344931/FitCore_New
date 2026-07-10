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

        public GetTrainingProgramsService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<GetTrainingProgramsResultDto>> Execute(
            GetTrainingProgramsRequestDto request)
        {
            // جلوگیری از مقادیر نامعتبر صفحه‌بندی
            request.Page = request.Page < 1 ? 1 : request.Page;
            request.PageSize = request.PageSize < 1 ? 10 : request.PageSize;

            var trainingPrograms = _context.TrainingPrograms
                .AsNoTracking()
                .Where(x => !x.IsRemoved)
                .AsQueryable();

            //====================================
            // فیلتر دسترسی
            //====================================
            if (request.IsAdmin || request.IsTriner)
            {
                // مدیر یا مربی فقط برنامه‌های باشگاه خودش را می‌بیند
                if (request.GymId.HasValue && request.GymId.Value > 0)
                {
                    trainingPrograms = trainingPrograms
                        .Where(x => x.GymId == request.GymId.Value);
                }
                else
                {
                    // مدیر یا مربی بدون باشگاه نباید داده‌ای ببیند
                    trainingPrograms = trainingPrograms.Where(x => false);
                }
            }
            else
            {
                // عضو فقط برنامه‌های تمرینی خودش را می‌بیند
                if (request.AppUserId > 0)
                {
                    trainingPrograms = trainingPrograms
                        .Where(x => x.Member.AppUserId == request.AppUserId);
                }
                else
                {
                    trainingPrograms = trainingPrograms.Where(x => false);
                }
            }

            //====================================
            // فیلتر جستجو
            //====================================
            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                var searchKey = request.SearchKey.Trim();

                trainingPrograms = trainingPrograms.Where(x =>
                    x.Member.AppUser.PhoneNumber.Contains(searchKey) ||
                    x.Member.AppUser.FullName.Contains(searchKey) ||
                    x.Title.Contains(searchKey) ||
                    x.TrainingProgramType.Name.Contains(searchKey) ||
                    x.TrainingGoalType.Name.Contains(searchKey) ||
                    x.StartDate.Contains(searchKey) ||
                    x.EndDate.Contains(searchKey));
            }

            var rowCount = await trainingPrograms.CountAsync();

            var programs = await trainingPrograms
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
                    IsSeen= x.IsSeen,
                    CountTrainingDays = x.Days.Count(day => !day.IsRemoved)
                })
                .ToListAsync();

            return new ResultDto<GetTrainingProgramsResultDto>
            {
                IsSuccess = true,
                Data = new GetTrainingProgramsResultDto
                {
                    TrainingPrograms = programs,
                    CurrentPage = request.Page,
                    RowCount = rowCount,
                    PageCount = (int)Math.Ceiling(
                        (double)rowCount / request.PageSize),
                    PageSize = request.PageSize
                }
            };
        }
    }
}
