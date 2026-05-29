using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Commands
{
    public interface IEditMemberService
    {
        ResultDto Execute(RequestEditMemberDto request);
    }
    public class EditMemberService : IEditMemberService
    {
        private readonly IDataBaseContext _context;

        public EditMemberService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestEditMemberDto request)
        {
            try
            {
                var member = _context.Members
            .FirstOrDefault(x => x.AppUserId == request.Id);
                var user = _context.Users.Where(c => c.Id == request.Id).FirstOrDefault();

                if (member == null)
                {
                    return new ResultDto
                    {
                        IsSuccess = false,
                        Message = "عضو یافت نشد"
                    };
                }


                user.FullName = request.FullName;
                user.PasswordHash = request.Mobile;
                member.Gender = request.Gender;
                member.BirthDate = request.BirthDate;
                member.UpdateTime = DateTime.Now;

                 _context.SaveChanges();

                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "ویرایش انجام شد"
                };

            }
            catch (Exception)
            {

                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "ویرایش انجام نشـــد"
                };


            }
        }
    }
}
