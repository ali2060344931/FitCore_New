using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Commands
{
    public interface IRemoveMemberService
    {
        ResultDto Execute(long memberId);
    }

    public class RemoveMemberService : IRemoveMemberService
    {
        private readonly IDataBaseContext _context;

        public RemoveMemberService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long memberId)
        {
            var member = _context.Members
                .FirstOrDefault(x => x.Id == memberId);

            if (member == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "عضو یافت نشد"
                };
            }

            member.IsRemoved = true;
            member.RemoveTime = DateTime.Now;

            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "عضو حذف شد"
            };
        }
    }
}
