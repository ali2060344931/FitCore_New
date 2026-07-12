using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Queries.GetAnnouncementById
{
    public class GetAnnouncementByIdService
        : IGetAnnouncementByIdService
    {
        private readonly IDataBaseContext _context;

        public GetAnnouncementByIdService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<ResultGetAnnouncementByIdDto>> Execute(long id)
        {
            var announcement =
                await _context.Announcements
                .Include(x => x.Roles)
                .Include(x => x.Gyms)
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    !x.IsRemoved);

            if (announcement == null)
            {
                return new ResultDto<ResultGetAnnouncementByIdDto>()
                {
                    IsSuccess = false,
                    Message = "اطلاعیه یافت نشد",
                    Data = null
                };
            }

            return new ResultDto<ResultGetAnnouncementByIdDto>()
            {
                IsSuccess = true,
                Data = new ResultGetAnnouncementByIdDto()
                {
                    Id = announcement.Id,
                    Title = announcement.Title,
                    Message = announcement.Message,
                    ImageUrl = announcement.ImageUrl,
                    ButtonText = announcement.ButtonText,
                    ButtonUrl = announcement.ButtonUrl,
                    Type = announcement.Type,
                    Priority = announcement.Priority,
                    IsActive = announcement.IsActive,
                    ShowOnce = announcement.ShowOnce,
                    IsPinned = announcement.IsPinned,
                    IsForAllRoles = announcement.IsForAllRoles,
                    IsForAllGyms = announcement.IsForAllGyms,
                    CanDismiss = announcement.CanDismiss,
                    RepeatAfterDays = announcement.RepeatAfterDays,
                    DisplayOrder = announcement.DisplayOrder,
                    StartDate = announcement.StartDate,
                    EndDate = announcement.EndDate,

                    RoleIds = announcement.Roles
                        .Select(x => x.RoleId)
                        .ToList(),

                    GymIds = announcement.Gyms
                        .Select(x => x.GymId)
                        .ToList()
                }
            };
        }
    }
}