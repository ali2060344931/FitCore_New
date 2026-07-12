using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Dashboard.DismissAnnouncement
{
    public class DismissAnnouncementService :
        IDismissAnnouncementService
    {
        private readonly IDataBaseContext _context;

        public DismissAnnouncementService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Execute(
            RequestDismissAnnouncementDto request)
        {
            var view =
                await _context.AnnouncementViews
                .FirstOrDefaultAsync(x =>
                    x.AnnouncementId == request.AnnouncementId &&
                    x.UserId == request.UserId);

            if (view == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "اطلاعیه یافت نشد."
                };
            }

            view.DismissedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "انجام شد."
            };
        }
    }
}