using FitCore.Domain.Entities.Members;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Queries.ReportMembers
{
    public class RequestCompleteMemberInfoDto
    {
        public long Id { get; set; }

        [Required]
        public long AppUserId { get; set; }

        public Gender Gender { get; set; }

        public string BirthDate { get; set; }
        public string MembershipStartDate { get; set; }
        public string MembershipEndDate { get; set; }

        public int? ActivityLevelId { get; set; }
        public int? ExperienceLevelId { get; set; }

        [MaxLength(500)]
        public string FoodAllergies { get; set; }

        [MaxLength(500)]
        public string MedicalConditions { get; set; }

        [MaxLength(500)]
        public string Injuries { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(500)]
        public string Description { get; set; }
        
        [Range(50, 250)]
        public decimal? Height { get; set; }

    }
}
