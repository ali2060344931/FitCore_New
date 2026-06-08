using FitCore.Application.Contexts;
using FitCore.Application.Services.Members.Queries;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Commands
{
    public interface IEditMemberBodyMeasurementService
    {
        ResultDto Execute(RequestEditMemberBodyMeasurementDto request);
    }
    public class EditMemberBodyMeasurementService : IEditMemberBodyMeasurementService
    {
        private readonly IDataBaseContext _context;

        public EditMemberBodyMeasurementService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestEditMemberBodyMeasurementDto request)
        {
            var entity = _context.memberBodyMeasurements.FirstOrDefault(x => x.Id == request.Id);

            if (entity == null)
                return new ResultDto { IsSuccess = false, Message = "رکورد یافت نشد." };

            entity.RecordDate = request.RecordDate;
            entity.Weight = request.Weight;
            
            entity.BodyFatPercentage = request.BodyFatPercentage;
            entity.Waist = request.Waist;
            entity.Hip = request.Hip;
            entity.Chest = request.Chest;

            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "اندازه‌گیری بدن با موفقیت ویرایش شد."
            };
        }
    }
}
