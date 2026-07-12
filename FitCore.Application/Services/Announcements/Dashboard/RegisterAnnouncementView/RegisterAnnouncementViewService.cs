using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Announcements;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Dashboard.RegisterAnnouncementView
{
    public class RegisterAnnouncementViewService :
        IRegisterAnnouncementViewService
    {
        private readonly IDataBaseContext _context;

        public RegisterAnnouncementViewService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Execute(
            RequestRegisterAnnouncementViewDto request)
        {
            //---------------------------------------------
            // اگر قبلاً دیده شده است چیزی ثبت نکن
            //---------------------------------------------

            bool exist =
                await _context.AnnouncementViews
                .AnyAsync(p =>
                    p.AnnouncementId == request.AnnouncementId &&
                    p.UserId == request.UserId);

            if (exist)
            {
                return new ResultDto()
                {
                    IsSuccess = true,
                    Message = "قبلاً ثبت شده است."
                };
            }

            //---------------------------------------------
            // ثبت مشاهده
            //---------------------------------------------

            AnnouncementView view =
                new AnnouncementView()
                {
                    AnnouncementId = request.AnnouncementId,

                    UserId = request.UserId,

                    ViewedAt = DateTime.Now,

                    IsClicked = false,

                    DismissedAt = null
                };

            await _context.AnnouncementViews.AddAsync(view);

            await _context.SaveChangesAsync();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ثبت شد."
            };
        }
    }
}