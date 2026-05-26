using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using System;
using System.Linq;

namespace FitCore.Application.Services.Member.Queries
{
    public interface IGetMembersService
    {
        ResultDto<ResultGetMembersDto> Execute(RequestGetMemberDto request);
    }

    public class GetMembersService : IGetMembersService
    {
        private readonly IDataBaseContext _context;

        public GetMembersService(IDataBaseContext context)
        {
            _context = context;
        }


        public ResultDto<ResultGetMembersDto> Execute(RequestGetMemberDto request)
        {
            var members = _context.Members
                .Where(x => x.GymId == request.GymId);

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                members = members.Where(x =>
                    x.FirstName.Contains(request.SearchKey) ||
                    x.LastName.Contains(request.SearchKey) ||
                    x.Mobile.Contains(request.SearchKey));
            }

            int rowCount = members.Count();

            var result = members
                .OrderByDescending(x => x.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ResultGetMemberDto
                {
                    Id = x.Id,
                    FullName = x.FirstName + " " + x.LastName,
                    Mobile = x.Mobile,
                    Gender = x.Gender,
                    BirthDate = x.BirthDate
                }).ToList();

            return new ResultDto<ResultGetMembersDto>
            {
                Data = new ResultGetMembersDto
                {
                    Members = result,
                    CurrentPage = request.Page,
                    PageSize = request.PageSize,
                    RowCount = rowCount,
                    Rows = result.Count()
                },

                IsSuccess = true
            };
        }
    }
}
