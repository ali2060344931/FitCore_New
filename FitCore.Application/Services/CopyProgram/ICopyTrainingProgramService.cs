using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.TrainingProgram;

using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.CopyPrograms
{
    public interface ICopyTrainingProgramService
    {
        Task<ResultDto<long>> Execute(CopyTrainingProgramDto request);
    }

    public class CopyTrainingProgramDto
    {
        /// <summary>
        /// شناسه برنامه تمرینی مبدأ (که می‌خواهیم کپی شود)
        /// </summary>
        [Required]
        public long SourceProgramId { get; set; }

        /// <summary>
        /// شناسه عضو مقصد (که برنامه برای او کپی می‌شود)
        /// </summary>
        [Required]
        public long TargetMemberId { get; set; }

        /// <summary>
        /// شناسه کاربری که عملیات کپی را انجام می‌دهد
        /// </summary>
        public long CreatedByUserId { get; set; }

        /// <summary>
        /// تاریخ شروع جدید برای برنامه کپی‌شده
        /// </summary>
        [Required]
        public string NewStartDate { get; set; }

        /// <summary>
        /// تاریخ پایان جدید برای برنامه کپی‌شده
        /// </summary>
        [Required]
        public string NewEndDate { get; set; }
    }

    public class CopyTrainingProgramService : ICopyTrainingProgramService
    {
        private readonly IDataBaseContext _context;

        public CopyTrainingProgramService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<long>> Execute(CopyTrainingProgramDto request)
        {
            //====================================
            // بارگذاری کامل برنامه مبدأ با تمام جزئیات
            //====================================

            var source =
                await _context.TrainingPrograms
                .Include(p => p.Days)
                    .ThenInclude(d => d.ExerciseItems)
                .FirstOrDefaultAsync(p =>
                    p.Id == request.SourceProgramId &&
                    !p.IsRemoved);

            if (source == null)
            {
                return new ResultDto<long>
                {
                    IsSuccess = false,
                    Message = "برنامه تمرینی مبدأ یافت نشد",
                    Data = 0
                };
            }

            //====================================
            // بررسی عضو مقصد
            //====================================

            var targetMember =
                await _context.Members
                .FirstOrDefaultAsync(m =>
                    m.Id == request.TargetMemberId);

            if (targetMember == null)
            {
                return new ResultDto<long>
                {
                    IsSuccess = false,
                    Message = "عضو مقصد یافت نشد",
                    Data = 0
                };
            }

            //====================================
            // ساخت برنامه جدید (کپی عمیق)
            //====================================

            var newProgram = new TrainingProgram
            {
                MemberId = request.TargetMemberId,
                GymId = source.GymId,
                CreatedByUserId = request.CreatedByUserId,
                Title = source.Title,
                Description = source.Description,
                TrainingProgramTypeId = source.TrainingProgramTypeId,
                TrainingGoalTypeId = source.TrainingGoalTypeId,
                SessionsPerWeek = source.SessionsPerWeek,
                StartDate = request.NewStartDate,
                EndDate = request.NewEndDate,
                IsActive = true,
                InsertTime = DateTime.Now,
            };

            await _context.TrainingPrograms.AddAsync(newProgram);
            await _context.SaveChangesAsync();

            //====================================
            // کپی روزها
            //====================================

            if (source.Days != null)
            {
                foreach (var sourceDay in source.Days.Where(d => !d.IsRemoved).OrderBy(d => d.SortOrder).ThenBy(d => d.DayNumber))
                {
                    var newDay = new TrainingDay
                    {
                        TrainingProgramId = newProgram.Id,
                        DayNumber = sourceDay.DayNumber,
                        Title = sourceDay.Title,
                        DayTypeId = sourceDay.DayTypeId,
                        Description = sourceDay.Description,
                        DurationMinutes = sourceDay.DurationMinutes,
                        SortOrder = sourceDay.SortOrder,
                        InsertTime = DateTime.Now,
                    };

                    await _context.TrainingDays.AddAsync(newDay);
                    await _context.SaveChangesAsync();

                    //====================================
                    // کپی حرکات هر روز
                    //====================================

                    if (sourceDay.ExerciseItems == null) continue;

                    foreach (var sourceItem in sourceDay.ExerciseItems.Where(e => !e.IsRemoved).OrderBy(e => e.SortOrder))
                    {
                        var newItem = new TrainingExerciseItem
                        {
                            TrainingDayId = newDay.Id,
                            ExerciseId = sourceItem.ExerciseId,
                            Sets = sourceItem.Sets,
                            Reps = sourceItem.Reps,
                            WeightKg = sourceItem.WeightKg,
                            RestSeconds = sourceItem.RestSeconds,
                            CoachNote = sourceItem.CoachNote,
                            SortOrder = sourceItem.SortOrder,
                            InsertTime = DateTime.Now,
                        };

                        await _context.TrainingExerciseItems.AddAsync(newItem);
                    }

                    await _context.SaveChangesAsync();
                }
            }

            return new ResultDto<long>
            {
                IsSuccess = true,
                Message = $"برنامه تمرینی با موفقیت برای عضو مقصد کپی شد ({source.Days?.Count ?? 0} روز)",
                Data = newProgram.Id
            };
        }
    }
}