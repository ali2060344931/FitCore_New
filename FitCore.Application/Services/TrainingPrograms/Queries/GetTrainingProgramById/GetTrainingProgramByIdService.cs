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

        public GetTrainingProgramByIdService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<TrainingProgramDetailsDto>> Execute(long trainingProgramId)
        {
            var trainingProgram =
                await _context.TrainingPrograms
                .Where(x => x.Id == trainingProgramId)
                .Include(x => x.Member)
                .Include(x => x.TrainingProgramType)
                .Include(x => x.TrainingGoalType)
                .Select(x => new TrainingProgramDetailsDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    TrainingProgramType = x.TrainingProgramType.Name,
                    TrainingGoalType = x.TrainingGoalType.Name,
                    MemberName = x.Member.AppUser.FullName,
                    MemberMobile = x.Member.AppUser.PhoneNumber,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    SessionsPerWeek = x.SessionsPerWeek,
                    IsActive = x.IsActive,
                })
                .FirstOrDefaultAsync();

            if (trainingProgram == null)
            {
                return new ResultDto<TrainingProgramDetailsDto>
                {
                    IsSuccess = false,
                    Message = "برنامه تمرینی یافت نشد",
                    Data = null
                };
            }

            //====================================
            // روزهای برنامه به همراه تمرینات
            //====================================

            trainingProgram.Days =
                await _context.TrainingDays
                .Where(x => x.TrainingProgramId == trainingProgramId && !x.IsRemoved)
                .Include(x => x.DayType)
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.DayNumber)
                .Select(d => new TrainingDayDetailsDto
                {
                    //EquipmentTypes
                    Id = d.Id,
                    DayNumber = d.DayNumber,
                    Title = d.Title,
                    DayType = d.DayType.Name,
                    Description = d.Description,
                    DurationMinutes = d.DurationMinutes,
                    SortOrder = d.SortOrder,
                    Exercises =
                        _context.TrainingExerciseItems
                        .Where(e => e.TrainingDayId == d.Id && !e.IsRemoved)
                        .Include(e => e.Exercise)
                        
                        .ThenInclude(ex => ex.PrimaryMuscleGroup)
                        .OrderBy(e => e.SortOrder)
                        .Select(e => new TrainingExerciseItemDto
                        {
                            Id = e.Id,
                            ExerciseId = e.ExerciseId,
                            ExerciseName = e.Exercise.Name,
                            MuscleGroup = e.Exercise.PrimaryMuscleGroup.Name,
                            //EquipmentTypes = e.Exercise.Description,
                            Sets = e.Sets,
                            Reps = e.Reps,
                            WeightKg = e.WeightKg,
                            RestSeconds = e.RestSeconds,
                            CoachNote = e.CoachNote,
                            SortOrder = e.SortOrder
                        })
                        .ToList()
                })
                .ToListAsync();

            return new ResultDto<TrainingProgramDetailsDto>
            {
                IsSuccess = true,
                Data = trainingProgram
            };
        }
    }
}