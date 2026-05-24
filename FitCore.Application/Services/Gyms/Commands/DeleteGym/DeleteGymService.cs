using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.IGym;
using FitCore.Application.Interfaces.ISms;
using FitCore.Common.Dto;

using System;
using System.Threading;
using System.Threading.Tasks;
namespace FitCore.Application.Services.Gyms.Commands.DeleteGym
{
    public class DeleteGymService : IDeleteGymService
    {

        private readonly IDataBaseContext _context;

        public DeleteGymService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long UserId)
        {
            var user = _context.Gyms.Find(UserId);
            if (user == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }
            user.RemoveTime = DateTime.Now;
            user.IsRemoved = true;
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "کاربر با موفقیت حذف شد"
            };
        }



    }

}
