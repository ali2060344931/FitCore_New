using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.TrainingPrograms.Queries.GetTrainingProgramById
{
    public class GetTrainingProgramByIdService : IGetTrainingProgramByIdService
    {
        private readonly IDataBaseContext _context;

        public GetTrainingProgramByIdService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<TrainingProgramDetailsDto>> Execute(long trainingProgramId)
        {
            //====================================
            // یک کوئری واحد — همه چیز با Include
            // (حل مشکل N+1 Query)
            //====================================

            var entity = await _context.TrainingPrograms
                .Where(x => x.Id == trainingProgramId)
                .Include(x => x.Member)
                    .ThenInclude(m => m.AppUser)
                .Include(x => x.TrainingProgramType)
                .Include(x => x.TrainingGoalType)
                .Include(x => x.Days.Where(d => !d.IsRemoved))
                    .ThenInclude(d => d.DayType)
                .Include(x => x.Days.Where(d => !d.IsRemoved))
                    .ThenInclude(d => d.ExerciseItems.Where(e => !e.IsRemoved))
                        .ThenInclude(e => e.Exercise)
                            .ThenInclude(ex => ex.PrimaryMuscleGroup)
                .Include(x => x.Days.Where(d => !d.IsRemoved))
                    .ThenInclude(d => d.ExerciseItems.Where(e => !e.IsRemoved))
                        .ThenInclude(e => e.Exercise)
                            .ThenInclude(ex => ex.EquipmentType)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return new ResultDto<TrainingProgramDetailsDto>
                {
                    IsSuccess = false,
                    Message = "برنامه تمرینی یافت نشد",
                    Data = null
                };
            }

            //====================================
            // تبدیل به DTO
            //====================================

            var dto = new TrainingProgramDetailsDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                TrainingProgramType = entity.TrainingProgramType?.Name,
                TrainingGoalType = entity.TrainingGoalType?.Name,
                MemberName = entity.Member?.AppUser?.FullName,
                MemberMobile = entity.Member?.AppUser?.PhoneNumber,
                BaleChatId= entity.Member.AppUser.BaleChatId,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                SessionsPerWeek = entity.SessionsPerWeek,
                IsActive = entity.IsActive,

                Days = entity.Days?
                    .OrderBy(d => d.SortOrder)
                    .ThenBy(d => d.DayNumber)
                    .Select(d => new TrainingDayDetailsDto
                    {
                        Id = d.Id,
                        DayNumber = d.DayNumber,
                        Title = d.Title,
                        DayType = d.DayType?.Name,
                        Description = d.Description,
                        DurationMinutes = d.DurationMinutes,
                        SortOrder = d.SortOrder,

                        Exercises = d.ExerciseItems?
                            .OrderBy(e => e.SortOrder)
                            .Select(e => new TrainingExerciseItemDto
                            {
                                Id = e.Id,
                                ExerciseId = e.ExerciseId,
                                ExerciseName = e.Exercise?.Name,
                                MuscleGroup = e.Exercise?.PrimaryMuscleGroup?.Name,
                                EquipmentType = e.Exercise?.EquipmentType?.Name,
                                Sets = e.Sets,
                                Reps = e.Reps,
                                WeightKg = e.WeightKg,
                                RestSeconds = e.RestSeconds,
                                CoachNote = e.CoachNote,
                                SortOrder = e.SortOrder
                            })
                            .ToList()
                    })
                    .ToList()
            };

            return new ResultDto<TrainingProgramDetailsDto>
            {
                IsSuccess = true,
                Data = dto
            };
        }
    }
}
