using FitCore.Domain.Entities.Members;

namespace FitCore.Application.Services.Members.Commands
{
    public class RequestEditMemberDto
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Mobile { get; set; }

        public Gender Gender { get; set; }

        public string BirthDate { get; set; }

    }
}
