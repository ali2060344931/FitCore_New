using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.TrainingProgramBuilder.Commands.RemoveTrainingDay
{
    public class RemoveTrainingDayService : IRemoveTrainingDayService
    {
        private readonly IDataBaseContext _context;

        public RemoveTrainingDayService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Execute(long trainingDayId)
        {
            var trainingDay =
                await _context.TrainingDays
                .FirstOrDefaultAsync(x => x.Id == trainingDayId && !x.IsRemoved);

            if (trainingDay == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "روز تمرینی یافت نشد"
                };
            }

            //====================================
            // حذف نرم تمرینات این روز
            //====================================

            var exerciseItems =
                await _context.TrainingExerciseItems
                .Where(x => x.TrainingDayId == trainingDayId && !x.IsRemoved)
                .ToListAsync();

            foreach (var item in exerciseItems)
            {
                item.IsRemoved = true;
                item.RemoveTime = DateTime.Now;
            }

            //====================================
            // حذف نرم روز تمرینی
            //====================================

            trainingDay.RemoveTime = DateTime.Now;
            trainingDay.IsRemoved = true;

            await _context.SaveChangesAsync();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "روز تمرینی با موفقیت حذف شد"
            };
        }
    }
}