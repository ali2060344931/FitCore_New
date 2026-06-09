using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.IMembers;
using FitCore.Application.Services.Member.Queries;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Members;

using System.Linq;

namespace FitCore.Application.Services.Members.Queries
{
    public class GetMembersByIdService : IGetMembersByIdService
    {
        private readonly IDataBaseContext _context;

        public GetMembersByIdService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<GetMemberByIdDto> Execute(int id)
        {

            var q = _context.Members.Where(c => c.AppUserId == id).First();
            var user = _context.Users.Where(c => c.Id == id).First();
            if (q == null)
            {
                return new ResultDto<GetMemberByIdDto>
                {
                    Data = null,

                    IsSuccess = false
                };

            }

            return new ResultDto<GetMemberByIdDto>
            {
                Data = new GetMemberByIdDto
                {
                    Id = id,
                    FullName = user.FullName,
                    BirthDate = q.BirthDate,
                    Gender = q.Gender,
                    Mobile = user.PhoneNumber,
                    MembershipEndDate=q.MembershipEndDate,
                    MembershipStartDate=q.MembershipStartDate
                },

                IsSuccess = true
            };
        }
    }
}
