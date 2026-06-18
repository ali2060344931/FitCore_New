using FitCore.Application.Contexts;
using FitCore.Application.Services.Dashboard;
using FitCore.Common;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
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
            _context = context;
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

            // --- دریافت اطلاعات اصلی داشبورد ---
            var dashboard = await _dashboardService.Execute(gymId.Value);

            // --- دریافت اعضای در حال انقضا (کد جدید) ---
            var criticalMembers = await GetCriticalMembersAsync(gymId.Value);
            ViewBag.CriticalMembers = criticalMembers;
            // ---------------------------------------------

            return View(dashboard);
        }

        //====================================================
        // Helper — دریافت اعضای بحرانی (پایان اشتراک)
        //====================================================
        private async Task<List<dynamic>> GetCriticalMembersAsync(long gymId)
        {
            var members = await _context.Members
                .Include(m => m.AppUser)
                .Where(m => m.AppUser.GymId == gymId &&
                            m.IsActive &&
                            !string.IsNullOrWhiteSpace(m.MembershipEndDate))
                .ToListAsync();

            var criticalList = new List<dynamic>();

            foreach (var m in members)
            {
                // استفاده از همان منطق تبدیل تاریخ
                try
                {
                    var parts = m.MembershipEndDate.Split('/');
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

                        // اگر کمتر یا مساوی ۷ روز مانده یا منقضی شده
                        if (daysLeft <= 7)
                        {
                            criticalList.Add(new
                            {
                                Id = m.Id,
                                FullName = m.AppUser.FullName,
                                Mobile = m.AppUser.PhoneNumber,
                                EndDate = m.MembershipEndDate,
                                RemainingDays = daysLeft
                            });
                        }
                    }
                }
                catch
                {
                    // اگر تاریخ اشتباه بود، نادیده بگیر
                    continue;
                }
            }

            // مرتب‌سازی: کسانی که زودتر منقضی می‌شوند اول بیایند
            return criticalList.OrderBy(x => x.RemainingDays).ToList();
        }
    }
}