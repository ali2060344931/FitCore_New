using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Queries
{
    public class GetMemberCompleteInfoDto
    {
        public long Id { get; set; }
        public long AppUserId { get; set; }

        public string FullName { get; set; }
        public string Mobile { get; set; }

        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string MembershipStartDate { get; set; }
        public string MembershipEndDate { get; set; }

        public int? ActivityLevelId { get; set; }
        public string ActivityLevelName { get; set; }

        public int? ExperienceLevelId { get; set; }
        public string ExperienceLevelName { get; set; }

        public string FoodAllergies { get; set; }
        public string MedicalConditions { get; set; }
        public string Injuries { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
    }
}
