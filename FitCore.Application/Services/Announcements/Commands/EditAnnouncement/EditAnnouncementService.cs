using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Announcements;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Commands.EditAnnouncement
{
    public class EditAnnouncementService : IEditAnnouncementService
    {
        private readonly IDataBaseContext _context;

        public EditAnnouncementService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<RequestEditAnnouncementDto>> Execute(RequestEditAnnouncementDto request)
        {
            try
            {
                var announcement =
                    await _context.Announcements
                    .Include(p => p.Roles)
                    .Include(p => p.Gyms)
                    .FirstOrDefaultAsync(p =>
                        p.Id == request.Id &&
                        !p.IsRemoved);

                if (announcement == null)
                {
                    return new ResultDto<RequestEditAnnouncementDto>()
                    {
                        IsSuccess = false,
                        Message = "اطلاعیه یافت نشد"
                    };
                }

                bool isExist =
                    await _context.Announcements
                    .AnyAsync(p =>
                        p.Id != request.Id &&
                        p.Title == request.Title &&
                        !p.IsRemoved);

                if (isExist)
                {
                    return new ResultDto<RequestEditAnnouncementDto>()
                    {
                        IsSuccess = false,
                        Message = "اطلاعیه ای با این عنوان قبلاً ثبت شده است"
                    };
                }

                announcement.Title = request.Title;
                announcement.Message = request.Message;
                announcement.ImageUrl = request.ImageUrl;
                announcement.ButtonText = request.ButtonText;
                announcement.ButtonUrl = request.ButtonUrl;

                announcement.Type = request.Type;
                announcement.Priority = request.Priority;

                announcement.IsActive = request.IsActive;
                announcement.IsPinned = request.IsPinned;
                announcement.ShowOnce = request.ShowOnce;

                announcement.IsForAllRoles = request.IsForAllRoles;
                announcement.IsForAllGyms = request.IsForAllGyms;

                announcement.CanDismiss = request.CanDismiss;
                announcement.RepeatAfterDays = request.RepeatAfterDays;

                announcement.DisplayOrder = request.DisplayOrder;

                announcement.StartDate = request.StartDate;
                announcement.EndDate = request.EndDate;

                announcement.UpdateTime = DateTime.Now;

                //--------------------------------------------------
                // حذف نقش های قبلی
                //--------------------------------------------------

                _context.AnnouncementRoles.RemoveRange(announcement.Roles);

                if (!request.IsForAllRoles)
                {
                    foreach (var roleId in request.RoleIds.Distinct())
                    {
                        await _context.AnnouncementRoles.AddAsync(
                            new AnnouncementRole
                            {
                                AnnouncementId = announcement.Id,
                                RoleId = roleId
                            });
                    }
                }

                //--------------------------------------------------
                // حذف باشگاه های قبلی
                //--------------------------------------------------

                _context.AnnouncementGyms.RemoveRange(announcement.Gyms);

                if (!request.IsForAllGyms)
                {
                    foreach (var gymId in request.GymIds.Distinct())
                    {
                        await _context.AnnouncementGyms.AddAsync(
                            new AnnouncementGym
                            {
                                AnnouncementId = announcement.Id,
                                GymId = gymId
                            });
                    }
                }

                await _context.SaveChangesAsync();

                return new ResultDto<RequestEditAnnouncementDto>()
                {
                    IsSuccess = true,
                    Message = "اطلاعیه با موفقیت ویرایش شد",
                    Data = new RequestEditAnnouncementDto
                    {
                        Id = announcement.Id
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResultDto<RequestEditAnnouncementDto>()
                {
                    IsSuccess = false,
                    Message = ex.InnerException?.Message ?? ex.Message
                };
            }
        }
    }
}