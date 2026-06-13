using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace FitCore.Application.Services.TrainingProgramBuilder.Commands.RemoveTrainingExercise
{
    public class RemoveTrainingExerciseService : IRemoveTrainingExerciseService
    {
        private readonly IDataBaseContext _context;

        public RemoveTrainingExerciseService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Execute(long trainingExerciseItemId)
        {
            var exerciseItem =
                await _context.TrainingExerciseItems
                .FirstOrDefaultAsync(x => x.Id == trainingExerciseItemId && !x.IsRemoved);

            if (exerciseItem == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "حرکت تمرینی یافت نشد"
                };
            }

            exerciseItem.RemoveTime = DateTime.Now;
            exerciseItem.IsRemoved = true;

            await _context.SaveChangesAsync();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "حرکت تمرینی با موفقیت حذف شد"
            };
        }
    }
}