using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Members;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Member.Queries
{
    public interface IGetMembersService
    {
        Task<ResultDto<ResultGetMembersDto>> Execute(RequestGetMemberDto request);
    }

    public class GetMembersService : IGetMembersService
    {
        private readonly IDataBaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMembersService(IDataBaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResultDto<ResultGetMembersDto>> Execute(RequestGetMemberDto request)
        {
            //========================================
            // پیدا کردن کاربر جاری
            //========================================

            var currentUser = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Id == request.AppUserId);

            if (currentUser == null)
            {
                return new ResultDto<ResultGetMembersDto>()
                {
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }

            //========================================
            // GymId مدیر باشگاه
            //========================================

            var gymId = currentUser.GymId;

            if (gymId == null)
            {
                return new ResultDto<ResultGetMembersDto>()
                {
                    IsSuccess = false,
                    Message = "باشگاه کاربر مشخص نیست"
                };
            }

            //========================================
            // Role Member
            //========================================

            var memberRole = await _context.Roles
                .FirstOrDefaultAsync(x =>
                    x.Name == "Member");

            if (memberRole == null)
            {
                return new ResultDto<ResultGetMembersDto>()
                {
                    IsSuccess = false,
                    Message = "نقش Member یافت نشد"
                };
            }

            //========================================
            // کاربران Member همان باشگاه
            //========================================

            var usersQuery = _context.Users

                // لود Member
                .Include(x => x.Member)

                .Where(x =>

                    x.GymId == gymId &&

                    _context.UserRoles.Any(ur =>

                        ur.UserId == x.Id &&

                        ur.RoleId == memberRole.Id
                    )
                );

            //========================================
            // Search
            //========================================

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                usersQuery = usersQuery.Where(x =>

                    x.FullName.Contains(request.SearchKey) ||

                    x.PhoneNumber.Contains(request.SearchKey)
                );
            }

            //========================================
            // Count
            //========================================

            var rowCount =
                await usersQuery.CountAsync();

            //========================================
            // Paging
            //========================================

            var users = await usersQuery
                .OrderByDescending(x => x.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)

                .ToListAsync();

            //========================================
            // DTO
            //========================================

            var members = users.Select(x =>
                new ResultGetMemberDto()
                {
                    Id = x.Id,

                    FullName = x.FullName,

                    Mobile = x.PhoneNumber,

                    BirthDate = x.Member?.BirthDate,

                    Gender = x.Member != null
                        ? x.Member.Gender : Gender.Male,
                    MembershipStartDate = x.Member?.MembershipStartDate,
                    MembershipEndDate = x.Member?.MembershipEndDate,


                    countNutritionProg = _context.NutritionPrograms.Count(c => c.MemberId == _context.Members.Where(p => p.AppUserId == x.Id).FirstOrDefault().Id),
                    countTrainingProg = _context.TrainingPrograms.Count(c => c.MemberId == _context.Members.Where(p => p.AppUserId == x.Id).FirstOrDefault().Id),
                    countBodyMeasurement = _context.memberBodyMeasurements.Count(c => c.MemberId == _context.Members.Where(p => p.AppUserId == x.Id).FirstOrDefault().Id),
                })
                .ToList();

            //========================================
            // Result
            //========================================

            return new ResultDto<ResultGetMembersDto>()
            {
                IsSuccess = true,

                Data = new ResultGetMembersDto()
                {
                    Members = members,

                    CurrentPage = request.Page,

                    PageSize = request.PageSize,

                    RowCount = rowCount,

                    Rows = members.Count
                }
            };
        }
    }
}