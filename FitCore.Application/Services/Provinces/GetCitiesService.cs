using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.IGym;
using FitCore.Common.Dto;

using System.Collections.Generic;
using System.Linq;

namespace FitCore.Application.Services.Provinces.Queries
{
    public class GetCitiesService : IGetCitiesService
    {
        private readonly IDataBaseContext _context;

        public GetCitiesService(IDataBaseContext context)
        {
            _context = context;
        }

        public List<SelectListDto> Execute(int provinceId)
        {
            return _context.Cities

                .Where(c => c.ProvincesId == provinceId)

                .Select(c => new SelectListDto
                {
                    Id = c.Id,
                    
                    Name = c.Name
                })

                .ToList();
        }
    }
}