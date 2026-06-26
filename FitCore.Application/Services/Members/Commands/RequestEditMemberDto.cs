using FitCore.Domain.Entities.Members;

using System.ComponentModel.DataAnnotations;

namespace FitCore.Application.Services.Members.Commands
{
    public class RequestEditMemberDto
    {
        public long Id { get; set; }

        public string FullName { get; set; }

        public string Mobile { get; set; }

        public Gender Gender { get; set; }

        public string BirthDate { get; set; }

        public string MembershipStartDate { get; set; }

        public string MembershipEndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
