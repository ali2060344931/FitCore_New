using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgramDay;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionProgramBuilder.Commands.AddNutritionProgramDay
{
    public interface IAddNutritionProgramDayService
    {
        ResultDto Execute(
            RequestAddNutritionProgramDayDto request);
    }

    public class AddNutritionProgramDayService :
    IAddNutritionProgramDayService
    {
        private readonly IDataBaseContext _context;

        public AddNutritionProgramDayService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(
            RequestAddNutritionProgramDayDto request)
        {
            try
            {
                var day =
                    new NutritionProgramDay()
                    {
                        NutritionProgramId =
                            request.NutritionProgramId,

                        Title =
                            request.Title,

                        DayNumber =
                            request.DayNumber
                    };

                _context.NutritionProgramDays.Add(day);

                _context.SaveChanges();

                return new ResultDto()
                {
                    IsSuccess = true,
                    Message = "روز با موفقیت ثبت شد"
                };
            }
            catch (Exception ex)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }

    public class RequestAddNutritionProgramDayDto
    {
        public long NutritionProgramId { get; set; }

        public string Title { get; set; }

        public int DayNumber { get; set; }
    }
}
