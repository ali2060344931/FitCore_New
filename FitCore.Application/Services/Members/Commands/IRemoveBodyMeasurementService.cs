using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Commands
{
    public interface IRemoveBodyMeasurementService
    {
        ResultDto Execute(long id);
    }
    public class RemoveBodyMeasurementService : IRemoveBodyMeasurementService
    {
        private readonly IDataBaseContext _context;

        public RemoveBodyMeasurementService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long id)
        {
            var measurement = _context.memberBodyMeasurements
                .FirstOrDefault(x => x.Id == id);

            if (measurement == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "رکورد مورد نظر یافت نشد"
                };
            }

            _context.memberBodyMeasurements.Remove(measurement);

            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "اندازه گیری با موفقیت حذف شد"
            };
        }
    }
}
