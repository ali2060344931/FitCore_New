using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.NutritionProgramReports.Queries;
using FitCore.Application.Services.NutritionPrograms.Queries.GetNutritionProgram;
using FitCore.Application.Services.TrainingProgramReports.Queries;
using FitCore.Application.Services.TrainingPrograms.Queries.GetTrainingPrograms;
using FitCore.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Member")]
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
            _nutritionFacad     = nutritionFacad;
            _trainingFacad      = trainingFacad;
            _nutritionPdfService = nutritionPdfService;
            _trainingPdfService  = trainingPdfService;
            _context             = context;
        }

        private long GetCurrentUserId()
        {
            return long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        //====================================================
        // داشبورد عضو — صفحه اصلی
        //====================================================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var appUserId = GetCurrentUserId();

            var nutritionResult = await _nutritionFacad.GetNutritionProgramsService
                .Execute(new RequestGetNutritionProgramsDto
                {
                    AppUserId = appUserId,
                    IsAdmin   = false,
                    Page      = 1,
                    PageSize  = 100
                });

            var trainingResult = await _trainingFacad.GetTrainingProgramsService
                .Execute(new GetTrainingProgramsRequestDto
                {
                    AppUserId = appUserId,
                    IsAdmin   = false,
                    Page      = 1,
                    PageSize  = 100
                });

            var memberInfo = await _context.Members
                .Include(m => m.AppUser)
                .FirstOrDefaultAsync(m => m.AppUserId == appUserId);

            ViewBag.MemberName = memberInfo?.AppUser?.FullName ?? "-";
            ViewBag.NutritionPrograms = nutritionResult?.NutritionPrograms;
            ViewBag.TrainingPrograms  = trainingResult?.Data?.TrainingPrograms;

            return View();
        }

        //====================================================
        // لیست برنامه‌های غذایی عضو
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
        // لیست برنامه‌های تمرینی عضو
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
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            long programId  = SecurityUtils.DecryptId(id);
            var appUserId   = GetCurrentUserId();

            // بررسی تعلق برنامه به همین عضو
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.AppUserId == appUserId);

            if (member == null) return Forbid();

            var owns = await _context.NutritionPrograms
                .AnyAsync(p => p.Id == programId && p.MemberId == member.Id && !p.IsRemoved);

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
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            long programId = SecurityUtils.DecryptId(id);
            var appUserId  = GetCurrentUserId();

            // بررسی تعلق برنامه به همین عضو
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.AppUserId == appUserId);

            if (member == null) return Forbid();

            var owns = await _context.TrainingPrograms
                .AnyAsync(p => p.Id == programId && p.MemberId == member.Id && !p.IsRemoved);

            if (!owns) return Forbid();

            var pdfBytes = _trainingPdfService.Execute(programId);
            return File(pdfBytes, "application/pdf", "TrainingProgram.pdf");
        }
    }
}
