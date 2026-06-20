using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;

namespace FitCore.Application.Services.TrainingProgramBuilder.Commands.RemoveAllTrainingDays
{
    public interface IRemoveAllTrainingDaysService
    {
        ResultDto Execute(RemoveAllTrainingDaysDto request);
    }

    public class RemoveAllTrainingDaysService : IRemoveAllTrainingDaysService
    {
        private readonly IDataBaseContext _context;

        public RemoveAllTrainingDaysService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RemoveAllTrainingDaysDto request)
        {
            var days = _context.TrainingDays
                .Include(d => d.ExerciseItems)
                .Where(d => d.TrainingProgramId == request.TrainingProgramId && !d.IsRemoved)
                .ToList();

            if (!days.Any())
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "هیچ روز فعالی برای این برنامه یافت نشد"
                };
            }

            var now = DateTime.Now;

            foreach (var day in days)
            {
                day.IsRemoved = true;
                day.RemoveTime = now;

                if (day.ExerciseItems != null)
                {
                    foreach (var exercise in day.ExerciseItems.Where(e => !e.IsRemoved))
                    {
                        exercise.IsRemoved = true;
                        exercise.RemoveTime = now;
                    }
                }
            }

            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = $"تمامی {days.Count} روز به همراه تمام حرکات آن‌ها با موفقیت حذف شدند"
            };
        }
    }

    public class RemoveAllTrainingDaysDto
    {
        public long TrainingProgramId { get; set; }
    }
}