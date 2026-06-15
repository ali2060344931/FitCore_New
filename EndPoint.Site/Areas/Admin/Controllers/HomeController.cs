using FitCore.Application.Contexts;
using FitCore.Application.Services.Dashboard;
using FitCore.Common;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IDataBaseContext _context;
        private readonly IGymDashboardService _dashboardService;

        public HomeController(
            IDataBaseContext context,
            IGymDashboardService dashboardService)
        {
            _context          = context;
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            // اگر SuperAdmin بود فقط پیام کلی نمایش بده
            if (User.IsSuperAdmin())
            {
                ViewBag.IsSuperAdmin = true;
                ViewBag.GymCount = await _context.Gyms.CountAsync();
                ViewBag.TotalUsers = await _context.Users.CountAsync();
                return View();
            }

            // پیدا کردن GymId کاربر جاری
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdValue))
                return Unauthorized();

            var appUserId = long.Parse(userIdValue);

            var gymId = await _context.Users
                .Where(u => u.Id == appUserId)
                .Select(u => u.GymId)
                .FirstOrDefaultAsync();

            if (gymId == null)
            {
                ViewBag.NoGym = true;
                return View();
            }

            var dashboard = await _dashboardService.Execute(gymId.Value);

            return View(dashboard);
        }
    }
}
