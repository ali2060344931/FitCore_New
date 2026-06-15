using FitCore.Application.Contexts;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Dashboard
{
    public class GymDashboardDto
    {
        // اطلاعات باشگاه
        public string GymName { get; set; }
        public string GymCode { get; set; }
        public string GymPhone { get; set; }
        public string GymMobile { get; set; }
        public string GymEmail { get; set; }
        public string GymAddress { get; set; }
        public string GymLogo { get; set; }
        public string SubscriptionExpireDate { get; set; }
        public int? MaxMembers { get; set; }
        public bool IsActive { get; set; }

        // آمار کلی
        public int TotalMembers { get; set; }
        public int ActiveMembers { get; set; }
        public int NewMembersThisMonth { get; set; }
        public int TotalNutritionPrograms { get; set; }
        public int ActiveNutritionPrograms { get; set; }
        public int TotalTrainingPrograms { get; set; }
        public int ActiveTrainingPrograms { get; set; }
        public int TotalFoods { get; set; }
        public int TotalExercises { get; set; }

        // آمار ماهانه برای نمودار (۶ ماه گذشته)
        public List<MonthlyStatDto> MonthlyNewMembers { get; set; }
        public List<MonthlyStatDto> MonthlyNutritionPrograms { get; set; }
        public List<MonthlyStatDto> MonthlyTrainingPrograms { get; set; }

        // اعضای اخیر
        public List<RecentMemberDto> RecentMembers { get; set; }

        // برنامه‌های اخیر
        public List<RecentProgramDto> RecentNutritionPrograms { get; set; }
        public List<RecentProgramDto> RecentTrainingPrograms { get; set; }

        // توزیع برنامه‌ها بر اساس نوع
        public List<DistributionDto> NutritionProgramTypes { get; set; }
        public List<DistributionDto> TrainingGoalTypes { get; set; }
    }

    public class MonthlyStatDto
    {
        public string Label { get; set; }
        public int Value { get; set; }
    }

    public class RecentMemberDto
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public DateTime InsertTime { get; set; }
        public int NutritionCount { get; set; }
        public int TrainingCount { get; set; }
    }

    public class RecentProgramDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string MemberName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class DistributionDto
    {
        public string Label { get; set; }
        public int Count { get; set; }
    }

    // ====================================================

    public interface IGymDashboardService
    {
        Task<GymDashboardDto> Execute(long gymId);
    }

    public class GymDashboardService : IGymDashboardService
    {
        private readonly IDataBaseContext _context;

        public GymDashboardService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<GymDashboardDto> Execute(long gymId)
        {
            var gym = await _context.Gyms
                .FirstOrDefaultAsync(g => g.Id == gymId);

            if (gym == null)
                return null;

            var now = DateTime.Now;

            // اعضای این باشگاه
            var memberIds = await _context.Users
                .Where(u => u.GymId == gymId)
                .Select(u => u.Id)
                .ToListAsync();

            var membersQuery = _context.Members
                .Include(m => m.AppUser)
                .Where(m => memberIds.Contains(m.AppUserId));

            var allMembers = await membersQuery
                .Select(m => new
                {
                    m.Id,
                    m.InsertTime,
                    FullName = m.AppUser.FullName,
                    Mobile   = m.AppUser.PhoneNumber
                })
                .ToListAsync();

            var allMemberIds = allMembers.Select(m => m.Id).ToList();

            // برنامه‌های غذایی
            var nutritionPrograms = await _context.NutritionPrograms
                .Include(p => p.ProgramType)
                .Include(p => p.Member).ThenInclude(m => m.AppUser)
                .Where(p => p.GymId == gymId && !p.IsRemoved)
                .ToListAsync();

            // برنامه‌های تمرینی
            var trainingPrograms = await _context.TrainingPrograms
                .Include(p => p.TrainingGoalType)
                .Include(p => p.Member).ThenInclude(m => m.AppUser)
                .Where(p => p.GymId == gymId && !p.IsRemoved)
                .ToListAsync();

            // آمار ماهانه (۶ ماه گذشته)
            var monthlyStats = BuildMonthlyStats(
                allMembers.Select(m => m.InsertTime).ToList(),
                nutritionPrograms.Select(p => p.InsertTime).ToList(),
                trainingPrograms.Select(p => p.InsertTime).ToList(),
                now);

            // توزیع انواع برنامه
            var nutritionDist = nutritionPrograms
                .GroupBy(p => p.ProgramType?.Name ?? "-")
                .Select(g => new DistributionDto { Label = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            var trainingGoalDist = trainingPrograms
                .GroupBy(p => p.TrainingGoalType?.Name ?? "-")
                .Select(g => new DistributionDto { Label = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            // ۸ عضو اخیر
            var recentMembers = allMembers
                .OrderByDescending(m => m.InsertTime)
                .Take(8)
                .Select(m => new RecentMemberDto
                {
                    Id            = m.Id,
                    FullName      = m.FullName,
                    Mobile        = m.Mobile,
                    InsertTime    = m.InsertTime,
                    NutritionCount = nutritionPrograms.Count(p => p.MemberId == m.Id),
                    TrainingCount  = trainingPrograms.Count(p => p.MemberId == m.Id)
                })
                .ToList();

            // ۵ برنامه اخیر
            var recentNutrition = nutritionPrograms
                .OrderByDescending(p => p.InsertTime)
                .Take(5)
                .Select(p => new RecentProgramDto
                {
                    Id         = p.Id,
                    Title      = p.ProgramType?.Name ?? "-",
                    MemberName = p.Member?.AppUser?.FullName ?? "-",
                    StartDate  = p.StartDate,
                    EndDate    = p.EndDate,
                    IsActive   = p.IsActive
                })
                .ToList();

            var recentTraining = trainingPrograms
                .OrderByDescending(p => p.InsertTime)
                .Take(5)
                .Select(p => new RecentProgramDto
                {
                    Id         = p.Id,
                    Title      = p.Title ?? "-",
                    MemberName = p.Member?.AppUser?.FullName ?? "-",
                    StartDate  = p.StartDate,
                    EndDate    = p.EndDate,
                    IsActive   = p.IsActive
                })
                .ToList();

            var totalFoods = await _context.Foods
                .CountAsync(f => !f.IsRemoved);

            var totalExercises = await _context.Exercises
                .CountAsync(e => !e.IsRemoved &&
                    (e.GymId == null || e.GymId == gymId));

            return new GymDashboardDto
            {
                // اطلاعات باشگاه
                GymName                = gym.Name,
                GymCode                = gym.Code,
                GymPhone               = gym.PhoneNumber,
                GymMobile              = gym.MobileNumber,
                GymEmail               = gym.Email,
                GymAddress             = gym.Address,
                GymLogo                = gym.Logo,
                SubscriptionExpireDate = gym.SubscriptionExpireDate,
                MaxMembers             = gym.MaxMembers,
                IsActive               = gym.IsActive,

                // آمار
                TotalMembers           = allMembers.Count,
                ActiveMembers          = allMembers.Count,
                NewMembersThisMonth    = allMembers.Count(m =>
                    m.InsertTime.Year  == now.Year &&
                    m.InsertTime.Month == now.Month),
                TotalNutritionPrograms  = nutritionPrograms.Count,
                ActiveNutritionPrograms = nutritionPrograms.Count(p => p.IsActive),
                TotalTrainingPrograms   = trainingPrograms.Count,
                ActiveTrainingPrograms  = trainingPrograms.Count(p => p.IsActive),
                TotalFoods              = totalFoods,
                TotalExercises          = totalExercises,

                // نمودار
                MonthlyNewMembers         = monthlyStats.members,
                MonthlyNutritionPrograms  = monthlyStats.nutrition,
                MonthlyTrainingPrograms   = monthlyStats.training,

                // لیست‌ها
                RecentMembers           = recentMembers,
                RecentNutritionPrograms = recentNutrition,
                RecentTrainingPrograms  = recentTraining,
                NutritionProgramTypes   = nutritionDist,
                TrainingGoalTypes       = trainingGoalDist,
            };
        }

        private (
            List<MonthlyStatDto> members,
            List<MonthlyStatDto> nutrition,
            List<MonthlyStatDto> training)
        BuildMonthlyStats(
            List<DateTime> memberDates,
            List<DateTime> nutritionDates,
            List<DateTime> trainingDates,
            DateTime now)
        {
            var months = new List<MonthlyStatDto>();
            var nutrition = new List<MonthlyStatDto>();
            var training  = new List<MonthlyStatDto>();

            for (int i = 5; i >= 0; i--)
            {
                var month = now.AddMonths(-i);
                var label = month.ToString("MM/yyyy");

                months.Add(new MonthlyStatDto
                {
                    Label = label,
                    Value = memberDates.Count(d =>
                        d.Year == month.Year && d.Month == month.Month)
                });

                nutrition.Add(new MonthlyStatDto
                {
                    Label = label,
                    Value = nutritionDates.Count(d =>
                        d.Year == month.Year && d.Month == month.Month)
                });

                training.Add(new MonthlyStatDto
                {
                    Label = label,
                    Value = trainingDates.Count(d =>
                        d.Year == month.Year && d.Month == month.Month)
                });
            }

            return (months, nutrition, training);
        }
    }
}
