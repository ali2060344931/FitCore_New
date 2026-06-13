using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FitCore.Application.Services.TrainingProgramBuilder.Commands.EditTrainingDay
{
    public class EditTrainingDayService : IEditTrainingDayService
    {
        private readonly IDataBaseContext _context;

        public EditTrainingDayService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<RequestEditTrainingDayDto>> Execute(RequestEditTrainingDayDto request)
        {
            try
            {
                var trainingDay =
                    await _context.TrainingDays
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.Id && !x.IsRemoved);

                if (trainingDay == null)
                {
                    return new ResultDto<RequestEditTrainingDayDto>()
                    {
                        IsSuccess = false,
                        Message = "روز تمرینی یافت نشد",
                        Data = null
                    };
                }

                trainingDay.DayNumber = request.DayNumber;
                trainingDay.Title = request.Title;
                trainingDay.DayTypeId = request.DayTypeId;
                trainingDay.Description = request.Description;
                trainingDay.DurationMinutes = request.DurationMinutes;

                if (request.SortOrder.HasValue)
                {
                    trainingDay.SortOrder = request.SortOrder.Value;
                }

                trainingDay.UpdateTime = DateTime.Now;

                await _context.SaveChangesAsync();

                return new ResultDto<RequestEditTrainingDayDto>()
                {
                    IsSuccess = true,
                    Message = "روز تمرینی با موفقیت ویرایش شد",
                    Data = new RequestEditTrainingDayDto
                    {
                        Id = trainingDay.Id
                    }
                };
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message;
                return new ResultDto<RequestEditTrainingDayDto>
                {
                    IsSuccess = false,
                    Message = "خطا در ویرایش روز تمرینی: " + inner,
                    Data = null
                };
            }
        }
    }

    public class RequestEditTrainingDayDto
    {
        [Required]
        public long Id { get; set; }

        [DisplayName("شماره روز")]
        [Required(ErrorMessage = "شماره روز الزامی است")]
        [Range(1, 14, ErrorMessage = "شماره روز باید بین 1 تا 14 باشد")]
        public int DayNumber { get; set; }

        [DisplayName("عنوان روز")]
        [Required(ErrorMessage = "عنوان روز الزامی است")]
        public string Title { get; set; }

        [DisplayName("نوع روز")]
        [Required(ErrorMessage = "نوع روز الزامی است")]
        public int DayTypeId { get; set; }

        [DisplayName("توضیحات")]
        public string Description { get; set; }

        [DisplayName("مدت زمان (دقیقه)")]
        public int? DurationMinutes { get; set; }

        public int? SortOrder { get; set; }
    }
}