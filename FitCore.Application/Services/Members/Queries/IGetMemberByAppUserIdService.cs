using FitCore.Application.Contexts;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Queries
{
    public interface IGetMemberByAppUserIdService
    {
        GetMemberCompleteInfoDto Execute(long appUserId);
    }
    public class GetMemberByAppUserIdService : IGetMemberByAppUserIdService
    {
        private readonly IDataBaseContext _context;

        public GetMemberByAppUserIdService(IDataBaseContext context)
        {
            _context = context;
        }

        public GetMemberCompleteInfoDto Execute(long appUserId)
        {
            var member = _context.Members
                .Include(x => x.AppUser)
                .Include(x => x.ActivityLevel)
                .Include(x => x.ExperienceLevel)
                .FirstOrDefault(x => x.AppUserId == appUserId);

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
