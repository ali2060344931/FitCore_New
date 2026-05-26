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
            var member = _context.Members
                .FirstOrDefault(x => x.Id == request.Id);

            if (member == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "عضو یافت نشد"
                };
            }

            member.FirstName = request.FirstName;
            member.LastName = request.LastName;
            member.Mobile = request.Mobile;
            member.Gender = request.Gender;
            member.BirthDate = request.BirthDate;
            //member.MembershipEndDate = request.MembershipEndDate;
            //member.Height = request.Height;
            //member.Weight = request.Weight;
            member.UpdateTime = DateTime.Now;

            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "ویرایش انجام شد"
            };
        }
    }
}
