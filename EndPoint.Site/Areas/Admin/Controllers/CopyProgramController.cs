using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.CopyPrograms;
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
    [Authorize]
    public class CopyProgramController : Controller
    {
        private readonly ICopyProgramFacad _copyFacad;
        private readonly IDataBaseContext _context;

        public CopyProgramController(
            ICopyProgramFacad copyFacad,
            IDataBaseContext context)
        {
            _copyFacad = copyFacad;
            _context = context;
        }

        //====================================================
        // Helper: گرفتن GymId کاربر جاری
        //====================================================
        private async Task<long?> GetCurrentUserGymIdAsync()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdValue)) return null;

            var appUserId = long.Parse(userIdValue);

            return await _context.Users
                .Where(x => x.Id == appUserId)
                .Select(x => x.GymId)
                .FirstOrDefaultAsync();
        }

        //====================================================
        // دریافت لیست برنامه‌های غذایی برای انتخاب مبدأ (AJAX)
        //====================================================
        [HttpGet]
        public async Task<IActionResult> GetNutritionPrograms(string searchKey = "")
        {
            var gymId = await GetCurrentUserGymIdAsync();
            if (gymId == null)
                return Json(new { isSuccess = false, message = "باشگاه تعریف نشده است" });

            var result = await _copyFacad.GetNutritionProgramsForCopyService.Execute(gymId.Value, searchKey);
            return Json(result);
        }

        //====================================================
        // دریافت لیست برنامه‌های تمرینی برای انتخاب مبدأ (AJAX)
        //====================================================
        [HttpGet]
        public async Task<IActionResult> GetTrainingPrograms(string searchKey = "")
        {
            var gymId = await GetCurrentUserGymIdAsync();
            if (gymId == null)
                return Json(new { isSuccess = false, message = "باشگاه تعریف نشده است" });

            var result = await _copyFacad.GetTrainingProgramsForCopyService.Execute(gymId.Value, searchKey);
            return Json(result);
        }

        //====================================================
        // دریافت لیست اعضا برای انتخاب مقصد (AJAX)
        //====================================================
        [HttpGet]
        public async Task<IActionResult> GetMembers(string searchKey = "")
        {
            var gymId = await GetCurrentUserGymIdAsync();
            if (gymId == null)
                return Json(new { isSuccess = false, message = "باشگاه تعریف نشده است" });

            var result = await _copyFacad.GetMembersForCopyService.Execute(gymId.Value, searchKey);
            return Json(result);
        }

        //====================================================
        // کپی برنامه غذایی (AJAX POST)
        //====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CopyNutritionProgram(CopyNutritionProgramDto request)
        {
            if (!ModelState.IsValid)
                return Json(new { isSuccess = false, message = "اطلاعات ورودی معتبر نیست" });

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdValue))
                return Json(new { isSuccess = false, message = "کاربر نامعتبر" });

            request.CreatedByUserId = long.Parse(userIdValue);

            var result = await _copyFacad.CopyNutritionProgramService.Execute(request);

            if (!result.IsSuccess)
                return Json(new { isSuccess = false, message = result.Message });

            return Json(new
            {
                isSuccess = true,
                message = result.Message,
                newProgramId = result.Data,
                redirectUrl = Url.Action("Index", "NutritionProgram", new { area = "Admin" })
            });
        }

        //====================================================
        // کپی برنامه تمرینی (AJAX POST)
        //====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CopyTrainingProgram(CopyTrainingProgramDto request)
        {
            if (!ModelState.IsValid)
                return Json(new { isSuccess = false, message = "اطلاعات ورودی معتبر نیست" });

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdValue))
                return Json(new { isSuccess = false, message = "کاربر نامعتبر" });

            request.CreatedByUserId = long.Parse(userIdValue);

            var result = await _copyFacad.CopyTrainingProgramService.Execute(request);

            if (!result.IsSuccess)
                return Json(new { isSuccess = false, message = result.Message });

            return Json(new
            {
                isSuccess = true,
                message = result.Message,
                newProgramId = result.Data,
                redirectUrl = Url.Action("Index", "TrainingProgram", new { area = "Admin" })
            });
        }
    }
}