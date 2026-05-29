using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Users;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Domain.Entities.NutritionProgram.NutritionProgram
{
    /// <summary>
    /// برنامه تغذیه
    /// </summary>
    public class NutritionProgram:BaseEntity
    {

        public long GymId { get; set; }
        
        public long MemberId { get; set; }

        public long CreatedByUserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int ProgramTypeId { get; set; }
        public NutritionProgramType ProgramType { get; set; }

        public int GoalTypeId { get; set; }
        public GoalType GoalType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        // Navigation

        public Gym Gym { get; set; }

        public Member Member { get; set; }

        public AppUser CreatedByUser { get; set; }

        public ICollection<NutritionProgramDay.NutritionProgramDay> Days { get; set; }
    }


    public class NutritionProgramType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<NutritionProgram> nutritionPrograms { get; set; }

    }
    public class GoalType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<NutritionProgram> nutritionPrograms { get; set; }
    }
}
