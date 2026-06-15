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

namespace FitCore.Application.Services.TrainingProgramBuilder.Commands.AddTrainingExercise
{
    public class AddTrainingExerciseService : IAddTrainingExerciseService
    {
        private readonly IDataBaseContext _context;

        public AddTrainingExerciseService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<RequestAddTrainingExerciseDto>> Execute(RequestAddTrainingExerciseDto request)
        {
            try
            {
                //====================================
                // بررسی روز تمرینی
                //====================================

                var trainingDay =
                    await _context.TrainingDays
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.TrainingDayId && !x.IsRemoved);

                if (trainingDay == null)
                {
                    return new ResultDto<RequestAddTrainingExerciseDto>()
                    {
                        IsSuccess = false,
                        Message = "روز تمرینی یافت نشد",
                        Data = null
                    };
                }

                //====================================
                // بررسی حرکت تمرینی
                //====================================

                var exercise =
                    await _context.Exercises
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.ExerciseId && !x.IsRemoved);

                if (exercise == null)
                {
                    return new ResultDto<RequestAddTrainingExerciseDto>()
                    {
                        IsSuccess = false,
                        Message = "حرکت تمرینی یافت نشد",
                        Data = null
                    };
                }

                //====================================
                // بررسی تکراری نبودن حرکت در همین روز
                //====================================

                bool isDuplicate =
                    await _context.TrainingExerciseItems
                    .AnyAsync(x =>
                        x.TrainingDayId == request.TrainingDayId &&
                        x.ExerciseId    == request.ExerciseId &&
                        !x.IsRemoved);

                if (isDuplicate)
                {
                    return new ResultDto<RequestAddTrainingExerciseDto>()
                    {
                        IsSuccess = false,
                        Message   = $"این حرکت قبلاً در این روز تمرینی ثبت شده است",
                        Data      = null
                    };
                }

                //====================================
                // تعیین ترتیب نمایش (در صورت عدم ارسال)
                //====================================

                int sortOrder = request.SortOrder ??
                    (await _context.TrainingExerciseItems
                        .Where(x => x.TrainingDayId == request.TrainingDayId && !x.IsRemoved)
                        .CountAsync()) + 1;

                //====================================
                // ساخت آیتم تمرینی
                //====================================

                TrainingExerciseItem exerciseItem =
                    new TrainingExerciseItem()
                    {
                        TrainingDayId = request.TrainingDayId,
                        ExerciseId = request.ExerciseId,
                        Sets = request.Sets,
                        Reps = request.Reps,
                        WeightKg = request.WeightKg,
                        RestSeconds = request.RestSeconds,
                        CoachNote = request.CoachNote,
                        SortOrder = sortOrder,
                        InsertTime = DateTime.Now,
                    };

                //====================================
                // ذخیره در دیتابیس
                //====================================

                await _context.TrainingExerciseItems
                    .AddAsync(exerciseItem);

                await _context.SaveChangesAsync();

                //====================================
                // نتیجه
                //====================================

                return new ResultDto<RequestAddTrainingExerciseDto>()
                {
                    IsSuccess = true,
                    Message = "حرکت با موفقیت به برنامه اضافه شد",
                    Data = new RequestAddTrainingExerciseDto
                    {
                        Id = exerciseItem.Id
                    }
                };
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message;
                return new ResultDto<RequestAddTrainingExerciseDto>
                {
                    IsSuccess = false,
                    Message = "خطا در افزودن حرکت تمرینی: " + inner,
                    Data = null
                };
            }
        }
    }

    public class RequestAddTrainingExerciseDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "شناسه روز تمرینی الزامی است")]
        public long TrainingDayId { get; set; }

        [DisplayName("حرکت تمرینی")]
        [Required(ErrorMessage = "انتخاب حرکت تمرینی الزامی است")]
        public long ExerciseId { get; set; }

        [DisplayName("تعداد ست")]
        public int? Sets { get; set; }

        [DisplayName("تعداد تکرار")]
        public string Reps { get; set; }

        [DisplayName("وزن (کیلوگرم)")]
        public decimal? WeightKg { get; set; }

        [DisplayName("استراحت (ثانیه)")]
        public int? RestSeconds { get; set; }

        [DisplayName("یادداشت مربی")]
        public string CoachNote { get; set; }

        public int? SortOrder { get; set; }
    }
}
