using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.TrainingProgram;

using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.TrainingProgramBuilder.Commands.AddTrainingDay
{
    public class AddTrainingDayService : IAddTrainingDayService
    {
        private readonly IDataBaseContext _context;

        public AddTrainingDayService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<RequestAddTrainingDayDto>> Execute(RequestAddTrainingDayDto request)
        {
            try
            {
                //====================================
                // بررسی برنامه تمرینی
                //====================================

                var trainingProgram =
                    await _context.TrainingPrograms
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.TrainingProgramId);

                if (trainingProgram == null)
                {
                    return new ResultDto<RequestAddTrainingDayDto>()
                    {
                        IsSuccess = false,
                        Message = "برنامه تمرینی یافت نشد",
                        Data = null
                    };
                }

                //====================================
                // تعیین ترتیب نمایش (در صورت عدم ارسال)
                //====================================

                int sortOrder = request.SortOrder ??
                    (await _context.TrainingDays
                        .Where(x => x.TrainingProgramId == request.TrainingProgramId && !x.IsRemoved)
                        .CountAsync()) + 1;

                //====================================
                // ساخت روز تمرینی
                //====================================

                TrainingDay trainingDay =
                    new TrainingDay()
                    {
                        TrainingProgramId = request.TrainingProgramId,
                        DayNumber = request.DayNumber,
                        Title = request.Title,
                        DayTypeId = request.DayTypeId,
                        Description = request.Description,
                        DurationMinutes = request.DurationMinutes,
                        SortOrder = sortOrder,
                        InsertTime = DateTime.Now,
                    };

                //====================================
                // ذخیره در دیتابیس
                //====================================

                await _context.TrainingDays
                    .AddAsync(trainingDay);

                await _context.SaveChangesAsync();

                //====================================
                // نتیجه
                //====================================

                return new ResultDto<RequestAddTrainingDayDto>()
                {
                    IsSuccess = true,
                    Message = "روز تمرینی با موفقیت اضافه شد",
                    Data = new RequestAddTrainingDayDto
                    {
                        Id = trainingDay.Id
                    }
                };
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message;
                return new ResultDto<RequestAddTrainingDayDto>
                {
                    IsSuccess = false,
                    Message = "خطا در افزودن روز تمرینی: " + inner,
                    Data = null
                };
            }
        }
    }

    public class RequestAddTrainingDayDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "شناسه برنامه تمرینی الزامی است")]
        public long TrainingProgramId { get; set; }

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