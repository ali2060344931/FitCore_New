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
        //ResultDto Execute(RequestEditMemberDto request);
        ResultDto<long> Execute(RequestEditMemberDto request);
    }
    public class EditMemberService : IEditMemberService
    {
        private readonly IDataBaseContext _context;

        public EditMemberService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<long> Execute(RequestEditMemberDto request)
        {
            try
            {
                var member = _context.Members
            .FirstOrDefault(x => x.AppUserId == request.Id);
                var user = _context.Users.Where(c => c.Id == request.Id).FirstOrDefault();

                if (member == null)
                {
                    //return new ResultDto
                    //{
                    //    IsSuccess = false,
                    //    Message = "عضو یافت نشد"
                    //};
                    return ResultDto<long>.Failure("عضو یافت نشد");
                }


                user.FullName = request.FullName;
                user.PasswordHash = request.Mobile;
                member.Gender = request.Gender;
                member.BirthDate = request.BirthDate;
                member.MembershipStartDate=request.MembershipStartDate;
                member.MembershipEndDate=request.MembershipEndDate;
                member.UpdateTime = DateTime.Now;
                member.IsActive = request.IsActive;

                 _context.SaveChanges();

                //return new ResultDto
                //{
                //    IsSuccess = true,
                //    Message = "ویرایش انجام شد",
                //};
                
                return ResultDto<long>.Success(member.Id, "ویرایش انجام شد. پیامی از طریق ربات فیتکور برای "+ request.FullName + "  ارسال گردید.");
            }
            catch (Exception)
            {

                //return new ResultDto
                //{
                //    IsSuccess = false,
                //    Message = "ویرایش انجام نشـــد"
                //};
                return ResultDto<long>.Failure("ویرایش انجام نشـــد");

            }
        }
    }
}
