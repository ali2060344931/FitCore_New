using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Members;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
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
            // Search (بهینه شده با پشتیبانی از Boolean و Enum)
            //========================================

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                // استانداردسازی کلمه جستجو شده
                var searchKeyLower = request.SearchKey.Trim().ToLower();

                // ۱. تنظیمات مربوط به IsActive (بولین)
                var trueKeywords = new[] { "true", "1", "بله", "فعال" };
                var falseKeywords = new[] { "false", "0", "خیر", "غیرفعال", "غیرفعــال" };

                bool isTrueKey = trueKeywords.Contains(searchKeyLower);
                bool isFalseKey = falseKeywords.Contains(searchKeyLower);

                // ۲. تنظیمات مربوط به Gender (اینوم)
                var genderMap = new Dictionary<string, Gender>(StringComparer.OrdinalIgnoreCase)
                {
                    { "مرد", Gender.Male },
                    { "آقا", Gender.Male },
                    { "male", Gender.Male },
                    { "زن", Gender.Female },
                    { "خانم", Gender.Female },
                    { "female", Gender.Female }
                };

                // بررسی اینکه آیا کلمه جستجو شده جزو کلمات جنسیت هست یا خیر
                Gender? searchedGender = null;
                if (genderMap.TryGetValue(searchKeyLower, out var genderValue))
                {
                    searchedGender = genderValue;
                }

                // ۳. اعمال فیلترها در کوئری
                usersQuery = usersQuery.Where(x =>

                    x.FullName.Contains(request.SearchKey) ||
                    x.PhoneNumber.Contains(request.SearchKey) ||
                    x.Member.MembershipStartDate.Contains(request.SearchKey) ||
                    x.Member.MembershipEndDate.Contains(request.SearchKey) ||
                    x.Member.BirthDate.Contains(request.SearchKey) ||

                    // فیلتر وضعیت فعالیت
                    (isTrueKey && x.Member.IsActive == true) ||
                    (isFalseKey && x.Member.IsActive == false) ||

                    // فیلتر جنسیت
                    (searchedGender.HasValue && x.Member.Gender == searchedGender.Value)
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
            // بهینه‌سازی شمارش برنامه‌ها (جلوگیری از مشکل N+1)
            //========================================

            // استخراج ID کاربران این صفحه
            var userIds = users.Select(u => u.Id).ToList();

            // استخراج ID اعضاء (Members) مرتبط با این کاربران
            var memberIds = users
                .Where(u => u.Member != null)
                .Select(u => u.Member.Id)
                .ToList();

            // گرفتن تعداد برنامه‌های غذایی با یک کوئری گروه‌بندی شده
            var nutritionCounts = await _context.NutritionPrograms
                .Where(np => memberIds.Contains(np.MemberId))
                .GroupBy(np => np.MemberId)
                .Select(g => new { MemberId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.MemberId, v => v.Count);

            // گرفتن تعداد برنامه‌های تمرینی با یک کوئری گروه‌بندی شده
            var trainingCounts = await _context.TrainingPrograms
                .Where(tp => memberIds.Contains(tp.MemberId))
                .GroupBy(tp => tp.MemberId)
                .Select(g => new { MemberId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.MemberId, v => v.Count);

            // گرفتن تعداد اندازه‌گیری‌های بدنی با یک کوئری گروه‌بندی شده
            var measurementCounts = await _context.memberBodyMeasurements
                .Where(bm => memberIds.Contains(bm.MemberId))
                .GroupBy(bm => bm.MemberId)
                .Select(g => new { MemberId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.MemberId, v => v.Count);


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

                    IsActive = x.Member != null ? x.Member.IsActive : false,

                    // خواندن مستقیم از دیکشنری‌های آماده شده در حافظه (سرعت فوق‌العاده بالا)
                    countNutritionProg = x.Member != null && nutritionCounts.ContainsKey(x.Member.Id)
                                        ? nutritionCounts[x.Member.Id] : 0,

                    countTrainingProg = x.Member != null && trainingCounts.ContainsKey(x.Member.Id)
                                       ? trainingCounts[x.Member.Id] : 0,

                    countBodyMeasurement = x.Member != null && measurementCounts.ContainsKey(x.Member.Id)
                                          ? measurementCounts[x.Member.Id] : 0,
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