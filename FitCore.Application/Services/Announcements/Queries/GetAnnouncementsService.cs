using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Queries.GetAnnouncements
{
    public class GetAnnouncementsService : IGetAnnouncementsService
    {
        private readonly IDataBaseContext _context;

        public GetAnnouncementsService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<ResultGetAnnouncementsListDto>> Execute(RequestGetAnnouncementsDto request)
        {
            var query =
                _context.Announcements
                .Where(x => !x.IsRemoved)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                query =
                    query.Where(x =>
                        x.Title.Contains(request.SearchKey) ||
                        x.Message.Contains(request.SearchKey));
            }

            if (request.IsActive != null)
            {
                query =
                    query.Where(x => x.IsActive == request.IsActive);
            }

            int rowCount = await query.CountAsync();

            var data =
                await query
                .OrderByDescending(x => x.IsPinned)
                .ThenBy(x => x.DisplayOrder)
                .ThenByDescending(x => x.InsertTime)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ResultGetAnnouncementsDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Type = x.Type,
                    Priority = x.Priority,
                    IsActive = x.IsActive,
                    IsPinned = x.IsPinned,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                })
                .ToListAsync();

            return new ResultDto<ResultGetAnnouncementsListDto>
            {
                IsSuccess = true,
                Data = new ResultGetAnnouncementsListDto
                {
                    Announcements = data,
                    RowCount = rowCount
                }
            };
        }
    }
}