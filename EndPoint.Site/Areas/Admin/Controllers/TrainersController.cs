using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Users; // اضافه شده

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TrainersController : Controller
    {
        private readonly IDataBaseContext _context;

        public TrainersController(IDataBaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var adminId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var admin = await _context.Users.FindAsync(adminId);

            if (admin == null || !admin.GymId.HasValue)
                return NotFound();

            var trainerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Trainer");

            // =======================================================
            // اصلاح ۱: جلوگیری از کرش در صورت نبود نقش در دیتابیس
            // =======================================================
            if (trainerRole == null)
            {
                // اگر نقش وجود نداشت، لیست خالی برمی‌گرداند تا ارور ندهد
                return View(new List<AppUser>());
            }

            // =======================================================
            // اصلاح ۲: برگرداندن لیست AppUser به جای dynamic
            // =======================================================
            var trainers = await _context.Users
                .Where(u => u.GymId == admin.GymId.Value &&
                            _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == trainerRole.Id))
                .ToListAsync();

            return View(trainers);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(long trainerUserId, bool isActive)
        {
            var adminId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var admin = await _context.Users.FindAsync(adminId);

            if (admin == null || !admin.GymId.HasValue)
                return Json(new ResultDto { IsSuccess = false, Message = "اطلاعات مدیر ناقص است." });

            var trainer = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == trainerUserId && u.GymId == admin.GymId.Value);

            if (trainer == null)
                return Json(new ResultDto { IsSuccess = false, Message = "مربی یافت نشد یا متعلق به باشگاه شما نیست." });

            var trainerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Trainer");
            bool isTrainer = await _context.UserRoles
                .AnyAsync(ur => ur.UserId == trainerUserId && ur.RoleId == trainerRole.Id);

            if (!isTrainer)
                return Json(new ResultDto { IsSuccess = false, Message = "شما فقط مجاز به تغییر وضعیت مربیان هستید." });

            trainer.IsActive = isActive;
            await _context.SaveChangesAsync();

            string message = isActive ? "مربی با موفقیت فعال شد." : "مربی با موفقیت غیرفعال شد.";
            return Json(new ResultDto { IsSuccess = true, Message = message });
        }
    }
}