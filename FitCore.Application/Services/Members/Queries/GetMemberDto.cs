using FitCore.Domain.Entities.Members;

using System;

namespace FitCore.Application.Services.Member.Queries
{
    public class GetMemberDto
    {
        public long Id { get; set; }

        public string FullName { get; set; }

        public string Mobile { get; set; }

        public Gender Gender { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
