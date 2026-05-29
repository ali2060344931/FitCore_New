using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgram;
using FitCore.Domain.Entities.Users;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionPrograms.Commands.AddNutritionProgram
{
    public class Create_UpdateNutritionProgramDto
    {
        public long Id { get; set; }
        public long GymId { get; set; }

        public long MemberId { get; set; }

        public long CreatedByUserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public bool IsActive { get; set; }

        public int GoalTypeId { get; set; }
        public GoalType GoalType { get; set; }
    }
}
