using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FitCore.Application.Services.TrainingProgramBuilder.Commands.EditTrainingExercise
{
    public class EditTrainingExerciseService : IEditTrainingExerciseService
    {
        private readonly IDataBaseContext _context;

        public EditTrainingExerciseService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<RequestEditTrainingExerciseDto>> Execute(RequestEditTrainingExerciseDto request)
        {
            try
            {
                var exerciseItem =
                    await _context.TrainingExerciseItems
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.Id && !x.IsRemoved);

                if (exerciseItem == null)
                {
                    return new ResultDto<RequestEditTrainingExerciseDto>()
                    {
                        IsSuccess = false,
                        Message = "حرکت تمرینی یافت نشد",
                        Data = null
                    };
                }

                //====================================
                // بررسی حرکت تمرینی جدید
                //====================================

                var exercise =
                    await _context.Exercises
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.ExerciseId && !x.IsRemoved);

                if (exercise == null)
                {
                    return new ResultDto<RequestEditTrainingExerciseDto>()
                    {
                        IsSuccess = false,
                        Message = "حرکت تمرینی یافت نشد",
                        Data = null
                    };
                }

                exerciseItem.ExerciseId = request.ExerciseId;
                exerciseItem.Sets = request.Sets;
                exerciseItem.Reps = request.Reps;
                exerciseItem.WeightKg = request.WeightKg;
                exerciseItem.RestSeconds = request.RestSeconds;
                exerciseItem.CoachNote = request.CoachNote;

                if (request.SortOrder.HasValue)
                {
                    exerciseItem.SortOrder = request.SortOrder.Value;
                }

                exerciseItem.UpdateTime = DateTime.Now;

                await _context.SaveChangesAsync();

                return new ResultDto<RequestEditTrainingExerciseDto>()
                {
                    IsSuccess = true,
                    Message = "حرکت تمرینی با موفقیت ویرایش شد",
                    Data = new RequestEditTrainingExerciseDto
                    {
                        Id = exerciseItem.Id
                    }
                };
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message;
                return new ResultDto<RequestEditTrainingExerciseDto>
                {
                    IsSuccess = false,
                    Message = "خطا در ویرایش حرکت تمرینی: " + inner,
                    Data = null
                };
            }
        }
    }

    public class RequestEditTrainingExerciseDto
    {
        [Required]
        public long Id { get; set; }

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