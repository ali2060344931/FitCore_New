using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Commands.DeleteAnnouncement
{
    public class DeleteAnnouncementService : IDeleteAnnouncementService
    {
        private readonly IDataBaseContext _context;

        public DeleteAnnouncementService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Execute(long announcementId)
        {
            var announcement =
                await _context.Announcements
                .FirstOrDefaultAsync(x =>
                    x.Id == announcementId &&
                    !x.IsRemoved);

            if (announcement == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "اطلاعیه یافت نشد"
                };
            }

            //----------------------------------------------------
            // Soft Delete
            //----------------------------------------------------

            announcement.IsRemoved = true;
            announcement.RemoveTime = DateTime.Now;

            await _context.SaveChangesAsync();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "اطلاعیه با موفقیت حذف شد"
            };
        }
    }
}