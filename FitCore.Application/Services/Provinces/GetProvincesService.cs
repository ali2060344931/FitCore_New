using FitCore.Application.Contexts;

using FitCore.Application.Interfaces.IGym;

using FitCore.Common.Dto;

using System.Collections.Generic;
using System.Linq;

namespace FitCore.Application.Services.Provinces.Queries
{
    public class GetProvincesService : IGetProvincesService
    {
        private readonly IDataBaseContext _context;

        public GetProvincesService(IDataBaseContext context)
        {
            _context = context;
        }

        public List<SelectListDto> Execute()
        {
            return _context.Provinces.Select(p => new SelectListDto
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
        }
    }
}