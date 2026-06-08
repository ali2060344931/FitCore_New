using FitCore.Application.Contexts;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Queries
{
    public interface IGetMemberBodyMeasurementsService
    {
        Task<ResultGetMemberBodyMeasurementsDto> Execute(RequestGetMemberBodyMeasurementsDto request);
    }
    public class GetMemberBodyMeasurementsService : IGetMemberBodyMeasurementsService
    {
        private readonly IDataBaseContext _context;

        public GetMemberBodyMeasurementsService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultGetMemberBodyMeasurementsDto> Execute(RequestGetMemberBodyMeasurementsDto request)
        {
            var query = _context.memberBodyMeasurements
                .Where(x => x.MemberId == request.MemberId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                query = query.Where(x =>
                    x.RecordDate.Contains(request.SearchKey));
            }

            var rowCount =  query.Count();

            var list = await query
                .OrderByDescending(x => x.RecordDate)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new GetMemberBodyMeasurementDto
                {
                    Id = x.Id,
                    MemberId = x.MemberId,
                    RecordDate = x.RecordDate,
                    Weight = x.Weight,
                    BodyFatPercentage = x.BodyFatPercentage,
                    Waist = x.Waist,
                    Hip = x.Hip,
                    Chest = x.Chest
                })
                .ToListAsync();

            return new ResultGetMemberBodyMeasurementsDto
            {
                Measurements = list,
                CurrentPage = request.Page,
                RowCount = rowCount,
                PageSize = request.PageSize,
                PageCount = (int)Math.Ceiling((double)rowCount / request.PageSize)
            };
        }
    }
}
