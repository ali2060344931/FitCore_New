using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
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
            IDataBaseContext context)
        {
            _nutritionFacad      = nutritionFacad;
            _trainingFacad       = trainingFacad;
            _nutritionPdfService = nutritionPdfService;
            _trainingPdfService  = trainingPdfService;
            _context             = context;
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
                    IsAdmin   = false,
                    Page      = 1,
                    PageSize  = 100
                });

            // برنامه‌های تمرینی
            var trainingResult = await _trainingFacad.GetTrainingProgramsService
                .Execute(new GetTrainingProgramsRequestDto
                {
                    AppUserId = appUserId,
                    IsAdmin   = false,
                    Page      = 1,
                    PageSize  = 100
                });

            // آخرین اندازه‌گیری بدن
            var lastMeasurement = member.memberBodyMeasurements?
                .Where(x => !x.IsRemoved)
                .OrderByDescending(x => x.InsertTime)
                .FirstOrDefault();

            // محاسبه وضعیت عضویت
            var membershipStatus = GetMembershipStatus(
                member.MembershipStartDate,
                member.MembershipEndDate);

            ViewBag.Member              = member;
            ViewBag.EncryptedUserId     = encryptedId;
            ViewBag.NutritionPrograms   = nutritionResult?.NutritionPrograms;
            ViewBag.TrainingPrograms    = trainingResult?.Data?.TrainingPrograms;
            ViewBag.LastMeasurement     = lastMeasurement;
            ViewBag.MembershipStatus    = membershipStatus;

            ViewData["Title"] = "داشبورد من";
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
                    IsAdmin   = false,
                    Page      = page,
                    PageSize  = PageSize,
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
                    IsAdmin   = false,
                    Page      = page,
                    PageSize  = PageSize,
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
            var appUserId  = GetCurrentUserId();

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
            var appUserId  = GetCurrentUserId();

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
        private MembershipStatusDto GetMembershipStatus(
            string startDate, string endDate)
        {
            var status = new MembershipStatusDto
            {
                StartDate = startDate ?? "-",
                EndDate   = endDate   ?? "-",
                Status    = MembershipState.Unknown,
                DaysLeft  = null
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

                    var today   = DateTime.Today;
                    var daysLeft = (endMiladi - today).Days;

                    status.DaysLeft = daysLeft;

                    if (daysLeft < 0)
                        status.Status = MembershipState.Expired;
                    else if (daysLeft <= 7)
                        status.Status = MembershipState.ExpiringSoon;
                    else
                        status.Status = MembershipState.Active;
                }
            }
            catch
            {
                // تاریخ قابل تبدیل نیست
            }

            return status;
        }
    }

    //====================================================
    // DTO وضعیت عضویت
    //====================================================
    public class MembershipStatusDto
    {
        public string StartDate { get; set; }
        public string EndDate   { get; set; }
        public MembershipState Status { get; set; }
        public int? DaysLeft { get; set; }
    }

    public enum MembershipState
    {
        Unknown,
        Active,
        ExpiringSoon,
        Expired
    }
}
