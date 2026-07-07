using System;
using System.Collections.Generic;

namespace FitCore.Application.Services.TrainingPrograms.Queries.GetTrainingPrograms
{
    public class GetTrainingProgramsResultDto
    {
        public List<GetTrainingProgramsDto> TrainingPrograms { get; set; }

        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public int PageSize { get; set; }
    }

    public class GetTrainingProgramsDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string TrainingProgramType { get; set; }

        public string TrainingGoalType { get; set; }

        public string MemberName { get; set; }

        public string MemberMobile { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int? SessionsPerWeek { get; set; }

        public bool IsActive { get; set; }

        public int CountTrainingDays { get; set; }
        public long? BaleChatId { get; set; }
    }

    public class GetTrainingProgramsRequestDto
    {
        public string SearchKey { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public long AppUserId { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsTriner { get; set; }
        public long? GymId { get; set; }

    }
}