using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Queries.GetRoles
{
    public class GetRolesService : IGetRolesService
    {
        private readonly IDataBaseContext _context;

        public GetRolesService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<List<ResultGetRolesDto>>> Execute()
        {
            var roles =
                await _context.Roles
                .OrderBy(p => p.Name)
                .Select(p => new ResultGetRolesDto
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToListAsync();

            return new ResultDto<List<ResultGetRolesDto>>
            {
                IsSuccess = true,
                Data = roles
            };
        }
    }
}