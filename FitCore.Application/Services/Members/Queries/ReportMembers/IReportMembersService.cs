using FitCore.Application.Contexts;
using FitCore.Application.Services.Member.Queries;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Queries.ReportMembers
{

    public interface IReportMembersService
    {
        ResultDto<MemberReportDto> Execute(long Id);
    }


    public class ReportMembersService : IReportMembersService
    {
        private readonly IDataBaseContext _context;

        public ReportMembersService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<MemberReportDto> Execute(long userId)
        {
            var currentUser = _context.Users
    .FirstOrDefault(x =>
        x.Id == userId);

            var gymId = currentUser.GymId;

            var memberRole = _context.Roles
               .FirstOrDefaultAsync(x =>
                   x.Name == "Member");


            var usersQuery = _context.Users

                // لود Member
                .Include(x => x.Member)

                .Where(x =>

                    x.GymId == gymId &&

                    _context.UserRoles.Any(ur =>

                        ur.UserId == x.Id &&

                        ur.RoleId == memberRole.Id
                    )
                );

            var users = usersQuery
                .OrderByDescending(x => x.Id).ToList();



            var members = users.Select(x => new MemberReportDto
            {
                FullName = x.FullName,

                Mobile = x.PhoneNumber,

                BirthDate = x.Member?.BirthDate,

            }).ToList();


            return new ResultDto<MemberReportDto>
            {
                IsSuccess = true,

                //Data = members,
            };
        }
    }




    public class MemberReportDto
    {

        public long AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string BirthDate { get; set; }

        public bool IsActive { get; set; } = true;

        public string Description { get; set; }
    }

}
