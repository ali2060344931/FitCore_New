using FitCore.Application.Contexts;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Queries
{
    public interface IGetMemberByIdService
    {
        GetMemberCompleteInfoDto Execute(long memberId);
    }
    public class GetMemberByIdService : IGetMemberByIdService
    {
        private readonly IDataBaseContext _context;

        public GetMemberByIdService(IDataBaseContext context)
        {
            _context = context;
        }

        public GetMemberCompleteInfoDto Execute(long memberId)
        {
            var member = _context.Members
                .Include(x => x.AppUser)
                .Include(x => x.ActivityLevel)
                .Include(x => x.ExperienceLevel)
                .FirstOrDefault(x => x.Id == memberId);

            if (member == null)
                return null;

            return new GetMemberCompleteInfoDto
            {
                Id = member.Id,
                AppUserId = member.AppUserId,
                FullName = member.AppUser?.FullName,
                Mobile = member.AppUser?.PhoneNumber,
                Gender = member.Gender.ToString(),
                BirthDate = member.BirthDate,
                MembershipStartDate = member.MembershipStartDate,
                MembershipEndDate = member.MembershipEndDate,
                ActivityLevelId = member.ActivityLevelId,
                ActivityLevelName = member.ActivityLevel?.Name,
                ExperienceLevelId = member.ExperienceLevelId,
                ExperienceLevelName = member.ExperienceLevel?.Name,
                FoodAllergies = member.FoodAllergies,
                MedicalConditions = member.MedicalConditions,
                Injuries = member.Injuries,
                IsActive = member.IsActive,
                Description = member.Description
            };
        }
    }
}
