/*
using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.IMembers;
using FitCore.Application.Services.Member.Queries;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Members;
using System.Linq;
using Microsoft.EntityFrameworkCore; // حتما این خط اضافه شود

namespace FitCore.Application.Services.Members.Queries
{
    public class GetMembersByIdService : IGetMembersByIdService
    {
        private readonly IDataBaseContext _context;

        public GetMembersByIdService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<GetMemberByIdDto> Execute(int id)
        {
            // تغییرات انجام شده:
            // 1. جستجو بر اساس Id خود جدول Member (c.Id == id)
            // 2. استفاده از Include برای گرفتن کاربر مربوطه (برای جلوگیری از دیتابیس رفت و آمد اضافی)
            // 3. استفاده از FirstOrDefault به جای First برای جلوگیری از کرش در صورت پیدا نشدن
            var member = _context.Members
                .Include(m => m.AppUser)
                .FirstOrDefault(c => c.Id == id);

            // بررسی نال بودن قبل از دسترسی به پراپرتی‌ها
            if (member == null)
            {
                return new ResultDto<GetMemberByIdDto>
                {
                    Data = null,
                    IsSuccess = false
                };
            }

            return new ResultDto<GetMemberByIdDto>
            {
                Data = new GetMemberByIdDto
                {
                    // آی دی را از موجودیت پیدا شده می‌گیریم
                    Id = member.Id,
                    FullName = member.AppUser.FullName,
                    BirthDate = member.BirthDate,
                    Gender = member.Gender,
                    Mobile = member.AppUser.PhoneNumber,
                    MembershipEndDate = member.MembershipEndDate,
                    MembershipStartDate = member.MembershipStartDate
                },
                IsSuccess = true
            };
        }
    }
}

*/





using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.IMembers;
using FitCore.Application.Services.Member.Queries;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Members;

using System.Linq;

namespace FitCore.Application.Services.Members.Queries
{
    public class GetMembersByIdService : IGetMembersByIdService
    {
        private readonly IDataBaseContext _context;

        public GetMembersByIdService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<GetMemberByIdDto> Execute(int id)
        {

            var q = _context.Members.Where(c => c.AppUserId == id).First();
            var user = _context.Users.Where(c => c.Id == id).First();
            if (q == null)
            {
                return new ResultDto<GetMemberByIdDto>
                {
                    Data = null,

                    IsSuccess = false
                };

            }

            return new ResultDto<GetMemberByIdDto>
            {
                Data = new GetMemberByIdDto
                {
                    Id = id,
                    FullName = user.FullName,
                    BirthDate = q.BirthDate,
                    Gender = q.Gender,
                    Mobile = user.PhoneNumber,
                    MembershipEndDate = q.MembershipEndDate,
                    MembershipStartDate = q.MembershipStartDate,
                    IsActive=q.IsActive,
                    
                },

                IsSuccess = true
            };
        }
    }
}
