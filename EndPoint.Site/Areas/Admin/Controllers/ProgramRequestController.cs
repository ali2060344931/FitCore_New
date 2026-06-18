using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.ProgramRequests;
using FitCore.Common;
using FitCore.Domain.Entities.ProgramRequest;

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
    public class ProgramRequestController : Controller
    {
        private readonly IProgramRequestFacad _facad;
        private readonly IDataBaseContext _context;

        public ProgramRequestController(
            IProgramRequestFacad facad,
            IDataBaseContext context)
        {
            _facad   = facad;
            _context = context;
        }

        private long GetCurrentUserId() =>
            long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        private async Task<long?> GetCurrentUserGymIdAsync()
        {
            var uid = GetCurrentUserId();
            return await _context.Users
                .Where(u => u.Id == uid)
                .Select(u => u.GymId)
                .FirstOrDefaultAsync();
        }

        // ============================================================
        // داشبورد مدیر — لیست درخواست‌ها
        // ============================================================

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Index(
            string status = "", string searchKey = "")
        {
            var gymId = await GetCurrentUserGymIdAsync();
            if (gymId == null) return BadRequest("باشگاه تعریف نشده است.");

            ProgramRequestStatus? statusFilter = status switch
            {
                "pending"    => ProgramRequestStatus.Pending,
                "inprogress" => ProgramRequestStatus.InProgress,
                "done"       => ProgramRequestStatus.Done,
                "rejected"   => ProgramRequestStatus.Rejected,
                _            => null
            };

            var result = await _facad.GetProgramRequestsForAdminService
                .Execute(gymId.Value, statusFilter, searchKey);

            var pendingCount = await _facad.GetPendingRequestsCountService
                .Execute(gymId.Value);

            ViewBag.PendingCount = pendingCount;
            ViewBag.CurrentStatus  = status;
            ViewBag.SearchKey      = searchKey;
            ViewData["Title"]      = "درخواست‌های برنامه";

            return View(result.Data);
        }

        // ============================================================
        // مدیر — تغییر وضعیت درخواست
        // ============================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> UpdateStatus(UpdateProgramRequestDto dto)
        {
            dto.ProcessedByUserId = GetCurrentUserId();

            var result = await _facad.UpdateProgramRequestService.Execute(dto);

            return Json(new
            {
                isSuccess = result.IsSuccess,
                message   = result.Message
            });
        }

        // ============================================================
        // عضو — فرم ارسال درخواست (GET)
        // ============================================================

        [HttpGet]
        [Authorize(Roles = "Member")]
        public IActionResult Submit()
        {
            ViewData["Title"] = "درخواست برنامه";
            return View();
        }

        // ============================================================
        // عضو — ارسال درخواست (POST)
        // ============================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Submit(
            ProgramRequestType requestType, string memberNote)
        {
            var appUserId = GetCurrentUserId();

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.AppUserId == appUserId);

            if (member == null)
                return Json(new { isSuccess = false, message = "اطلاعات عضو یافت نشد." });

            var gymId = await _context.Users
                .Where(u => u.Id == appUserId)
                .Select(u => u.GymId)
                .FirstOrDefaultAsync();

            if (gymId == null)
                return Json(new { isSuccess = false, message = "باشگاه تعریف نشده است." });

            var dto = new SubmitProgramRequestDto
            {
                MemberId    = member.Id,
                GymId       = gymId.Value,
                RequestType = requestType,
                MemberNote  = memberNote
            };

            var result = await _facad.SubmitProgramRequestService.Execute(dto);

            return Json(new
            {
                isSuccess = result.IsSuccess,
                message   = result.Message
            });
        }

        // ============================================================
        // عضو — لیست درخواست‌های خودش
        // ============================================================

        [HttpGet]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> MyRequests()
        {
            var appUserId = GetCurrentUserId();

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.AppUserId == appUserId);

            if (member == null) return RedirectToAction("Index", "MemberDashboard");

            var result = await _facad.GetProgramRequestsForMemberService
                .Execute(member.Id);

            ViewData["Title"] = "درخواست‌های من";
            return View(result.Data);
        }

        // ============================================================
        // AJAX — تعداد درخواست‌های در انتظار (برای badge داشبورد)
        // ============================================================

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> PendingCount()
        {
            var gymId = await GetCurrentUserGymIdAsync();
            if (gymId == null) return Json(new { count = 0 });

            var count = await _facad.GetPendingRequestsCountService.Execute(gymId.Value);
            return Json(new { count });
        }
    }
}
