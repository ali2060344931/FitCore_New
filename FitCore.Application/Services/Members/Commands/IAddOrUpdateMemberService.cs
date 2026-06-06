using FitCore.Application.Contexts;
using FitCore.Application.Services.Members.Queries.ReportMembers;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Members;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Commands
{
    public interface IAddOrUpdateMemberService
    {
        ResultDto Execute(RequestCompleteMemberInfoDto request);
    }
    public class AddOrUpdateMemberService : IAddOrUpdateMemberService
    {
        private readonly IDataBaseContext _context;

        public AddOrUpdateMemberService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestCompleteMemberInfoDto request)
        {
            var member = _context.Members.FirstOrDefault(x => x.AppUserId == request.AppUserId);

            if (member == null)
            {
                member = new Domain.Entities.Members.Member
                {
                    AppUserId = request.AppUserId,
                    Gender = request.Gender == "Male" ? Gender.Male : Gender.Female,
                    BirthDate = request.BirthDate,
                    MembershipStartDate = request.MembershipStartDate,
                    MembershipEndDate = request.MembershipEndDate,
                    ActivityLevelId = request.ActivityLevelId,
                    ExperienceLevelId = request.ExperienceLevelId,
                    FoodAllergies = request.FoodAllergies,
                    MedicalConditions = request.MedicalConditions,
                    Injuries = request.Injuries,
                    IsActive = request.IsActive,
                    Description = request.Description
                };

                _context.Members.Add(member);
                _context.SaveChanges();

                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "اطلاعات عضو با موفقیت ثبت شد."
                };
            }

            member.Gender = request.Gender == "Male" ? Gender.Male : Gender.Female;
            member.BirthDate = request.BirthDate;
            member.MembershipStartDate = request.MembershipStartDate;
            member.MembershipEndDate = request.MembershipEndDate;
            member.ActivityLevelId = request.ActivityLevelId;
            member.ExperienceLevelId = request.ExperienceLevelId;
            member.FoodAllergies = request.FoodAllergies;
            member.MedicalConditions = request.MedicalConditions;
            member.Injuries = request.Injuries;
            member.IsActive = request.IsActive;
            member.Description = request.Description;

            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "اطلاعات عضو با موفقیت ویرایش شد."
            };
        }
    }
}
