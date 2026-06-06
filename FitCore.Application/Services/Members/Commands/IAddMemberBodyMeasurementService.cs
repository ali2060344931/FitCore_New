using FitCore.Application.Contexts;
using FitCore.Application.Services.Members.Queries;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Members;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Commands
{
    public interface IAddMemberBodyMeasurementService
    {
        ResultDto Execute(RequestAddMemberBodyMeasurementDto request);
    }
    public class AddMemberBodyMeasurementService : IAddMemberBodyMeasurementService
    {
        private readonly IDataBaseContext _context;

        public AddMemberBodyMeasurementService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestAddMemberBodyMeasurementDto request)
        {
            var entity = new MemberBodyMeasurement
            {
                MemberId = request.MemberId,
                RecordDate = request.RecordDate,
                Weight = request.Weight,
                Height = request.Height,
                BodyFatPercentage = request.BodyFatPercentage,
                Waist = request.Waist,
                Hip = request.Hip,
                Chest = request.Chest
            };

            _context.memberBodyMeasurements.Add(entity);
            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "اندازه‌گیری بدن با موفقیت ثبت شد."
            };
        }
    }
}
