using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Dashboard.RegisterAnnouncementClick
{
    public class RegisterAnnouncementClickService :
        IRegisterAnnouncementClickService
    {
        private readonly IDataBaseContext _context;

        public RegisterAnnouncementClickService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Execute(
            RequestRegisterAnnouncementClickDto request)
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
                    Message = "رکورد مشاهده پیدا نشد."
                };
            }

            view.IsClicked = true;

            await _context.SaveChangesAsync();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "ثبت شد."
            };
        }
    }
}