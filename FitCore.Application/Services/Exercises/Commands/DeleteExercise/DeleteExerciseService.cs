using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Exercises.Commands.DeleteExercise
{
    public class DeleteExerciseService : IDeleteExerciseService
    {
        private readonly IDataBaseContext _context;

        public DeleteExerciseService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Execute(long exerciseId)
        {
            var exercise =
                await _context.Exercises
                .FirstOrDefaultAsync(x => x.Id == exerciseId && !x.IsRemoved);

            if (exercise == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "حرکت تمرینی یافت نشد"
                };
            }

            //====================================
            // بررسی استفاده شدن در برنامه‌های فعال
            //====================================

            bool isUsed =
                await _context.TrainingExerciseItems
                .AnyAsync(x => x.ExerciseId == exerciseId && !x.IsRemoved);

            if (isUsed)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این حرکت در برنامه‌های تمرینی استفاده شده و قابل حذف نیست"
                };
            }

            exercise.RemoveTime = DateTime.Now;
            exercise.IsRemoved = true;

            await _context.SaveChangesAsync();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "حرکت تمرینی با موفقیت حذف شد"
            };
        }
    }
}