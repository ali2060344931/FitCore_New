using Bugeto_Store.Application.Interfaces.Contexts;
using Bugeto_Store.Application.Services.Users.Commands.RgegisterUser;
using Bugeto_Store.Common.Dto;

namespace Bugeto_Store.Application.Services.Users.Commands.EditUser
{
    public class EditUserService : IEditUserService
    {
        private readonly IDataBaseContext _context;

        public EditUserService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(RequestEdituserDto request)
        {


            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "پست الکترونیک را وارد نمایید"
                };
            }

            if (string.IsNullOrWhiteSpace(request.Fullname))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "نام و نام خانوادگی را وارد نمائید"
                };
            }





            var user = _context.Users.Find(request.UserId);
            if (user == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }

            user.FullName = request.Fullname;
            user.Email = request.Email;
            user.Tel=request.Tel;

            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ویرایش کاربر انجام شد"
            };

        }
    }
}
