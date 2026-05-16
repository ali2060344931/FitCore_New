using FitCore.Application.Interfaces.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Members;

using System;
using System.Collections.Generic;
using System.Linq;

namespace FitCore.Application.Services.Member.Queries
{
    public interface IGetMembersService
    {
        ResultDto<ResultGetMembersDto> Execute(long gymId);
    }

    public class GetMembersService : IGetMembersService
    {
        private readonly IDataBaseContext _context;

        public GetMembersService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultGetMembersDto> Execute(long gymId)
        {
            var query = _context.Members
                .Where(x => x.GymId == gymId);

            int totalRows = query.Count();

            var members = query
                .Select(x => new GetMemberDto
                {
                    Id = x.Id,
                    FullName = x.FirstName + " " + x.LastName,
                    Mobile = x.Mobile,
                    Gender = x.Gender,
                    BirthDate = x.BirthDate
                })
                .ToList();

            return new ResultDto<ResultGetMembersDto>()
            {
                Data = new ResultGetMembersDto
                {
                    Members = members,
                    Rows = members.Count,
                    TotalRows = totalRows
                },
                IsSuccess = true
            };
        }
    }

    public class GetMemberDto
    {
        public long Id { get; set; }

        public string FullName { get; set; }

        public string Mobile { get; set; }

        public Gender Gender { get; set; }

        public DateTime BirthDate { get; set; }
    }

    public class ResultGetMembersDto
    {
        public List<GetMemberDto> Members { get; set; }

        public int Rows { get; set; }

        public int TotalRows { get; set; }
    }
}
