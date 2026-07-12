using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Queries.GetGyms
{
    public class GetGymsService : IGetGymsService
    {
        private readonly IDataBaseContext _context;

        public GetGymsService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<List<ResultGetGymsDto>>> Execute()
        {
            var gyms =
                await _context.Gyms
                .Where(p => !p.IsRemoved)
                .OrderBy(p => p.Name)
                .Select(p => new ResultGetGymsDto
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToListAsync();

            return new ResultDto<List<ResultGetGymsDto>>
            {
                IsSuccess = true,
                Data = gyms
            };
        }
    }
}