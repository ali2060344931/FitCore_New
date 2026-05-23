using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using System.Collections.Generic;
using System.Linq;

namespace FitCore.Application.Services.Setings.Queries.GetSetings
{
    /// <summary>
    /// تنظیمات برنامه
    /// </summary>
    public interface IGetSetings
    {
        ResultDto<List<SetingDto>> Execute();
    }

    /// <summary>
    /// تنظیمات برنامه
    /// </summary>
    public class GetSetingService : IGetSetings
    {
        private readonly IDataBaseContext _context;

        public GetSetingService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<SetingDto>> Execute()
        {
            var roles = _context.Setings.ToList().Select(p => new SetingDto
            {
                Id = p.Id,
                TextFilde = p.TextFilde

            }).ToList();

            return new ResultDto<List<SetingDto>>()
            {
                Data = roles,
                IsSuccess = true,
                Message = "",
            };
        }
    }


    /// <summary>
    /// تنظیمات برنامه
    /// </summary>
    public class SetingDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string TextFilde { get; set; }
        public int NumberFilde { get; set; }
        public bool BoolFilde { get; set; }
    }

}
