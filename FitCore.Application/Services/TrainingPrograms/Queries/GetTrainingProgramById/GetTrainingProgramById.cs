using System.Collections.Generic;

namespace FitCore.Application.Services.TrainingPrograms.Queries.GetTrainingProgramById
{
    public class TrainingProgramDetailsDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string TrainingProgramType { get; set; }

        public string TrainingGoalType { get; set; }

        public string MemberName { get; set; }

        public string MemberMobile { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int? SessionsPerWeek { get; set; }

        public bool IsActive { get; set; }

        public List<TrainingDayDetailsDto> Days { get; set; }
    }

    public class TrainingDayDetailsDto
    {
        public long Id { get; set; }

        public int DayNumber { get; set; }

        public string Title { get; set; }

        public string DayType { get; set; }

        public string Description { get; set; }

        public int? DurationMinutes { get; set; }

        public int SortOrder { get; set; }

        public List<TrainingExerciseItemDto> Exercises { get; set; }
    }

    public class TrainingExerciseItemDto
    {
        public long Id { get; set; }

        public long ExerciseId { get; set; }

        public string ExerciseName { get; set; }

        public string MuscleGroup { get; set; }

        public int? Sets { get; set; }

        public string Reps { get; set; }

        public decimal? WeightKg { get; set; }

        public int? RestSeconds { get; set; }

        public string CoachNote { get; set; }

        public int SortOrder { get; set; }
    }
}