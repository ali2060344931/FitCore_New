using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Announcements;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Commands.AddAnnouncement
{
    public class AddAnnouncementService : IAddAnnouncementService
    {
        private readonly IDataBaseContext _context;

        public AddAnnouncementService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<RequestAddAnnouncementDto>> Execute(RequestAddAnnouncementDto request)
        {
            try
            {
                //------------------------------------
                // بررسی عنوان تکراری
                //------------------------------------

                bool isExist =
                    await _context.Announcements
                    .AnyAsync(x =>
                        x.Title == request.Title &&
                        !x.IsRemoved);

                if (isExist)
                {
                    return new ResultDto<RequestAddAnnouncementDto>()
                    {
                        IsSuccess = false,
                        Message = "اطلاعیه ای با این عنوان قبلاً ثبت شده است"
                    };
                }

                //------------------------------------
                // ایجاد اطلاعیه
                //------------------------------------

                Announcement announcement =
                    new Announcement()
                    {
                        Title = request.Title,
                        Message = request.Message,
                        ImageUrl = request.ImageUrl,
                        ButtonText = request.ButtonText,
                        ButtonUrl = request.ButtonUrl,

                        Type = request.Type,
                        Priority = request.Priority,

                        IsActive = request.IsActive,
                        IsPinned = request.IsPinned,
                        ShowOnce = request.ShowOnce,

                        IsForAllRoles = request.IsForAllRoles,
                        IsForAllGyms = request.IsForAllGyms,

                        CanDismiss = request.CanDismiss,
                        RepeatAfterDays = request.RepeatAfterDays,

                        DisplayOrder = request.DisplayOrder,

                        StartDate = request.StartDate,
                        EndDate = request.EndDate,

                        InsertTime = DateTime.Now
                    };

                await _context.Announcements.AddAsync(announcement);

                //------------------------------------
                // ذخیره نقش ها
                //------------------------------------

                if (!request.IsForAllRoles)
                {
                    foreach (var roleId in request.RoleIds)
                    {
                        await _context.AnnouncementRoles.AddAsync(
                            new AnnouncementRole()
                            {
                                Announcement = announcement,
                                RoleId = roleId
                            });
                    }
                }

                //------------------------------------
                // ذخیره باشگاه ها
                //------------------------------------

                if (!request.IsForAllGyms)
                {
                    foreach (var gymId in request.GymIds)
                    {
                        await _context.AnnouncementGyms.AddAsync(
                            new AnnouncementGym()
                            {
                                Announcement = announcement,
                                GymId = gymId
                            });
                    }
                }

                //------------------------------------

                await _context.SaveChangesAsync();

                //------------------------------------

                return new ResultDto<RequestAddAnnouncementDto>()
                {
                    IsSuccess = true,
                    Message = "اطلاعیه با موفقیت ثبت شد",

                    Data = new RequestAddAnnouncementDto()
                    {
                        Id = announcement.Id
                    }
                };
            }
            catch (DbUpdateException ex)
            {
                return new ResultDto<RequestAddAnnouncementDto>()
                {
                    IsSuccess = false,
                    Message = ex.InnerException?.Message ?? ex.Message
                };
            }
        }
    }
}