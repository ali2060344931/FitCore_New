using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.Member.Queries;
using FitCore.Application.Services.NutritionProgramReports.Queries;
using FitCore.Application.Services.NutritionPrograms.Queries.GetNutritionProgram;
using FitCore.Application.Services.TrainingProgramReports.Queries;
using FitCore.Application.Services.TrainingPrograms.Queries.GetTrainingPrograms;
using FitCore.Common;
using FitCore.Domain.Entities.Members;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Member")]   // فقط عضو — مدیر باشگاه نمی‌تواند وارد شود
    public class MemberDashboardController : Controller
    {
        private readonly INutritionProgramFacad _nutritionFacad;
        private readonly ITrainingProgramFacad _trainingFacad;
        private readonly IGetNutritionProgramPdfService _nutritionPdfService;
        private readonly IGetTrainingProgramPdfService _trainingPdfService;
        private readonly IDataBaseContext _context;
        public MemberDashboardController(
            INutritionProgramFacad nutritionFacad,
            ITrainingProgramFacad trainingFacad,
            IGetNutritionProgramPdfService nutritionPdfService,
            IGetTrainingProgramPdfService trainingPdfService,
            IDataBaseContext context
           )
        {
            _nutritionFacad = nutritionFacad;
            _trainingFacad = trainingFacad;
            _nutritionPdfService = nutritionPdfService;
            _trainingPdfService = trainingPdfService;
            _context = context;
        }




        private long GetCurrentUserId() =>
            long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        //====================================================
        // دریافت اطلاعات کامل عضو جاری
        //====================================================
        private async Task<(Member member, string encryptedUserId)> GetCurrentMemberAsync()
        {
            var appUserId = GetCurrentUserId();

            var member = await _context.Members
                .Include(m => m.AppUser)
                .Include(m => m.ActivityLevel)
                .Include(m => m.ExperienceLevel)
                .Include(m => m.memberBodyMeasurements)
                .FirstOrDefaultAsync(m => m.AppUserId == appUserId);

            var encryptedId = SecurityUtils.EncryptId(appUserId);

            return (member, encryptedId);
        }

        //====================================================
        // داشبورد اصلی عضو
        //====================================================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var appUserId = GetCurrentUserId();
            var (member, encryptedId) = await GetCurrentMemberAsync();

            if (member == null)
                return RedirectToAction("CompleteInfo", "MemberProfile", new { area = "Admin" });

            // برنامه‌های غذایی
            var nutritionResult = await _nutritionFacad.GetNutritionProgramsService
                .Execute(new RequestGetNutritionProgramsDto
                {
                    AppUserId = appUserId,
                    IsAdmin = false,
                    Page = 1,
                    PageSize = 100
                });

            // برنامه‌های تمرینی
            var trainingResult = await _trainingFacad.GetTrainingProgramsService
                .Execute(new GetTrainingProgramsRequestDto
                {
                    AppUserId = appUserId,
                    IsAdmin = false,
                    Page = 1,
                    PageSize = 100
                });

            // آخرین اندازه‌گیری بدن
            var lastMeasurement = member.memberBodyMeasurements?
                .Where(x => !x.IsRemoved)
                .OrderByDescending(x => x.InsertTime)
                .FirstOrDefault();

            // محاسبه وضعیت عضویت
            var membershipStatus = GetMembershipStatus(member.MembershipStartDate, member.MembershipEndDate, member.IsActive);

            ViewBag.Member = member;
            ViewBag.EncryptedUserId = encryptedId;
            ViewBag.NutritionPrograms = nutritionResult?.NutritionPrograms;
            ViewBag.TrainingPrograms = trainingResult?.Data?.TrainingPrograms;
            ViewBag.LastMeasurement = lastMeasurement;
            ViewBag.MembershipStatus = membershipStatus;
            ViewBag.MemberBirthDate = PersianDateCalse.GetAge( member.BirthDate,PersianDateCalse.AgeDisplayMode.YearMonthDay);


            ViewData["Title"] = "داشبورد من";

            // در متد Index اضافه کنید:
            ViewBag.Analytics = GetMemberAnalytics(member.Id);

            return View();
        }

        //====================================================
        // لیست برنامه‌های غذایی
        //====================================================
        [HttpGet]
        public async Task<IActionResult> MyNutritionPrograms(
            int page = 1, int PageSize = 10, string SearchKey = "")
        {
            var appUserId = GetCurrentUserId();

            var result = await _nutritionFacad.GetNutritionProgramsService
                .Execute(new RequestGetNutritionProgramsDto
                {
                    AppUserId = appUserId,
                    IsAdmin = false,
                    Page = page,
                    PageSize = PageSize,
                    SearchKey = SearchKey
                });

            ViewData["Title"] = "برنامه‌های غذایی من";
            return View(result);
        }

        //====================================================
        // لیست برنامه‌های تمرینی
        //====================================================
        [HttpGet]
        public async Task<IActionResult> MyTrainingPrograms(
            int page = 1, int PageSize = 10, string SearchKey = "")
        {
            var appUserId = GetCurrentUserId();

            var result = await _trainingFacad.GetTrainingProgramsService
                .Execute(new GetTrainingProgramsRequestDto
                {
                    AppUserId = appUserId,
                    IsAdmin = false,
                    Page = page,
                    PageSize = PageSize,
                    SearchKey = SearchKey
                });

            ViewData["Title"] = "برنامه‌های تمرینی من";
            return View(result?.Data);
        }

        //====================================================
        // چاپ PDF برنامه غذایی (فقط متعلق به خود عضو)
        //====================================================
        [HttpGet]
        public async Task<IActionResult> PrintNutritionProgram(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            long programId = SecurityUtils.DecryptId(id);
            var appUserId = GetCurrentUserId();

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.AppUserId == appUserId);

            if (member == null) return Forbid();

            var owns = await _context.NutritionPrograms
                .AnyAsync(p =>
                    p.Id == programId &&
                    p.MemberId == member.Id &&
                    !p.IsRemoved);

            if (!owns) return Forbid();

            var pdfBytes = _nutritionPdfService.Execute(programId);
            return File(pdfBytes, "application/pdf", "NutritionProgram.pdf");
        }

        //====================================================
        // چاپ PDF برنامه تمرینی (فقط متعلق به خود عضو)
        //====================================================
        [HttpGet]
        public async Task<IActionResult> PrintTrainingProgram(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            long programId = SecurityUtils.DecryptId(id);
            var appUserId = GetCurrentUserId();

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.AppUserId == appUserId);

            if (member == null) return Forbid();

            var owns = await _context.TrainingPrograms
                .AnyAsync(p =>
                    p.Id == programId &&
                    p.MemberId == member.Id &&
                    !p.IsRemoved);

            if (!owns) return Forbid();

            var pdfBytes = _trainingPdfService.Execute(programId);
            return File(pdfBytes, "application/pdf", "TrainingProgram.pdf");
        }

        //====================================================
        // Helper — وضعیت عضویت
        //====================================================
        private MembershipStatusDto GetMembershipStatus(string startDate, string endDate, bool MemberIsActive = true)
        {

            var status = new MembershipStatusDto
            {
                StartDate = startDate ?? "-",
                EndDate = endDate ?? "-",
                Status = MembershipState.Unknown,
                DaysLeft = null
            };


            if (string.IsNullOrWhiteSpace(endDate))
                return status;


            // تبدیل تاریخ شمسی به میلادی برای محاسبه
            // (چون تاریخ‌ها به صورت رشته شمسی ذخیره می‌شوند)
            try
            {


                var parts = endDate.Split('/');
                if (parts.Length == 3)
                {
                    var pc = new System.Globalization.PersianCalendar();
                    var endMiladi = pc.ToDateTime(
                        int.Parse(parts[0]),
                        int.Parse(parts[1]),
                        int.Parse(parts[2]),
                        0, 0, 0, 0);

                    var today = DateTime.Today;
                    var daysLeft = (endMiladi - today).Days;

                    status.DaysLeft = daysLeft;

                    if (daysLeft < 0)
                    {
                        status.Status = MembershipState.Expired;
                        return status;
                    }
                    else if (daysLeft <= 7)
                    {
                        status.Status = MembershipState.ExpiringSoon;
                        return status;
                    }
                    else
                        status.Status = MembershipState.Active;

                    if (MemberIsActive)
                        status.Status = MembershipState.Active;
                    else
                        status.Status = MembershipState.NotActive;




                    return status;
                }






            }
            catch
            {
                // تاریخ قابل تبدیل نیست
            }

            return status;
        }



        private MemberAnalyticsDto GetMemberAnalytics(long memberId)
        {
            var analytics = new MemberAnalyticsDto();

            // 1. دریافت تاریخچه اندازه گیری ها
            var measurements = _context.memberBodyMeasurements
                .Where(m => m.MemberId == memberId && !m.IsRemoved)
                .OrderBy(m => m.InsertTime)
                .ToList();

            if (measurements.Any())
            {
                var first = measurements.First();
                var last = measurements.Last();

                analytics.FirstWeight = first.Weight;
                analytics.CurrentWeight = last.Weight;
                analytics.WeightChange = analytics.CurrentWeight - analytics.FirstWeight;

                analytics.FirstFat = first.BodyFatPercentage;
                analytics.CurrentFat = last.BodyFatPercentage;
                analytics.FatChange = analytics.CurrentFat - analytics.FirstFat;

                // محاسبه نسبت کمر به باسن (WHR) از آخرین اندازه گیری
                if (last.Waist.HasValue && last.Hip.HasValue && last.Hip > 0)
                {
                    analytics.WaistToHipRatio = Math.Round(last.Waist.Value / last.Hip.Value, 2);
                }

                // محاسبه سرعت پیشرفت (هفته ای چند کیلو تغییر کرده)
                if (first.RecordDate != null && last.RecordDate != null && analytics.WeightChange.HasValue)
                {
                    try
                    {
                        var pc = new System.Globalization.PersianCalendar();
                        var startDate = first.RecordDate.Split('/').Select(int.Parse).ToArray();
                        var endDate = last.RecordDate.Split('/').Select(int.Parse).ToArray();

                        var startMiladi = pc.ToDateTime(startDate[0], startDate[1], startDate[2], 0, 0, 0, 0);
                        var endMiladi = pc.ToDateTime(endDate[0], endDate[1], endDate[2], 0, 0, 0, 0);

                        int totalDays = (endMiladi - startMiladi).Days;
                        analytics.TotalWeeksPassed = totalDays / 7;

                        if (analytics.TotalWeeksPassed > 0)
                        {
                            analytics.WeeklyWeightChangeRate = Math.Round(analytics.WeightChange.Value / analytics.TotalWeeksPassed.Value, 2);
                        }
                    }
                    catch { }
                }

                // آماده سازی دیتای نمودارها
                analytics.WeightHistory = measurements.Select(m => new ChartPointDto { Label = m.RecordDate, Value = m.Weight }).ToList();
                analytics.FatHistory = measurements.Select(m => new ChartPointDto { Label = m.RecordDate, Value = m.BodyFatPercentage }).ToList();
                analytics.WaistHistory = measurements.Select(m => new ChartPointDto { Label = m.RecordDate, Value = m.Waist }).ToList();
            }

            // 2. محاسبه دقیق ماکروها و درصدهای کالری
            var activeNutrition = _context.NutritionPrograms
                .Where(p => p.MemberId == memberId && p.IsActive && !p.IsRemoved)
                .Include(p => p.Days).ThenInclude(d => d.Meals).ThenInclude(m => m.Items)
                .Include(p => p.GoalType)
                .FirstOrDefault();

            if (activeNutrition != null && activeNutrition.Days != null)
            {
                var allItems = activeNutrition.Days.Where(d => !d.IsRemoved)
                    .SelectMany(d => d.Meals.Where(m => !m.IsRemoved))
                    .SelectMany(m => m.Items.Where(i => !i.IsRemoved))
                    .ToList();

                int totalDays = activeNutrition.Days.Count(d => !d.IsRemoved);

                if (totalDays > 0)
                {
                    decimal totalCal = allItems.Sum(i => i.Calories ?? 0) / totalDays;
                    decimal totalPro = allItems.Sum(i => i.Protein ?? 0) / totalDays;
                    decimal totalCarb = allItems.Sum(i => i.Carbohydrate ?? 0) / totalDays;
                    decimal totalFat = allItems.Sum(i => i.Fat ?? 0) / totalDays;

                    // محاسبه درصد کالری هر ماکرو (قانون 4-4-9)
                    int calFromPro = totalPro > 0 ? (int)((totalPro * 4) / totalCal * 100) : 0;
                    int calFromCarb = totalCarb > 0 ? (int)((totalCarb * 4) / totalCal * 100) : 0;
                    int calFromFat = totalFat > 0 ? (int)((totalFat * 9) / totalCal * 100) : 0;

                    analytics.Nutrition = new NutritionSummaryDto
                    {
                        GoalName = activeNutrition.GoalType?.Name,
                        AvgDailyCalories = totalCal,
                        AvgDailyProtein = totalPro,
                        AvgDailyCarbs = totalCarb,
                        AvgDailyFat = totalFat,
                        ProteinPercent = calFromPro,
                        CarbsPercent = calFromCarb,
                        FatPercent = calFromFat
                    };
                }
            }

            // 3. محاسبه حجم تمرین (بر اساس ست ها)
            var activeTraining = _context.TrainingPrograms
                .Where(t => t.MemberId == memberId && t.IsActive && !t.IsRemoved)
                .Include(t => t.TrainingGoalType)
                .Include(t => t.Days).ThenInclude(d => d.ExerciseItems)
                .FirstOrDefault();

            if (activeTraining != null && activeTraining.Days != null)
            {
                var allExerciseItems = activeTraining.Days.Where(d => !d.IsRemoved)
                    .SelectMany(d => d.ExerciseItems.Where(i => !i.IsRemoved))
                    .ToList();

                analytics.Training = new TrainingSummaryDto
                {
                    GoalName = activeTraining.TrainingGoalType?.Name,
                    SessionsPerWeek = activeTraining.SessionsPerWeek,
                    TotalActiveDays = activeTraining.Days.Count(d => !d.IsRemoved),
                    TotalSets = allExerciseItems.Sum(i => i.Sets ?? 0),
                    TotalExercises = allExerciseItems.Select(i => i.ExerciseId).Distinct().Count()
                };
            }

            return analytics;
        }
    }

    //====================================================
    // DTO وضعیت عضویت
    //====================================================
    public class MembershipStatusDto
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public MembershipState Status { get; set; }
        public int? DaysLeft { get; set; }
    }

    public enum MembershipState
    {
        Unknown,
        Active,
        ExpiringSoon,
        Expired,
        NotActive
    }

}
