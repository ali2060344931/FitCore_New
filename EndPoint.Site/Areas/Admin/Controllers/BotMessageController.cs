using EndPoint.Site.Areas.Admin.Models;


using FitCore.Application.Contexts;
using FitCore.Application.Services.Auth;
using FitCore.Common;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Users;

using GymBot.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BotMessageController : Controller
    {
        private readonly IDataBaseContext _db;
        private readonly IBaleBotService _baleBotService;
        private readonly UserManager<AppUser> _userManager;

        public BotMessageController(
            IDataBaseContext db,
            IBaleBotService baleBotService,
            UserManager<AppUser> userManager)
        {
            _db = db;
            _baleBotService = baleBotService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> SendBulkMessage()
        {
            var model = new SendBaleBulkMessageViewModel();
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.GymId == null) return BadRequest();

            string todayShamsi = DateConverterMlladiToShamsi.ToShamsi(DateTime.Now);

            model.Members = await (from ur in _db.UserRoles
                                   join u in _db.Users on ur.UserId equals u.Id
                                   join r in _db.Roles on ur.RoleId equals r.Id
                                   join m in _db.Members on u.Id equals m.AppUserId into memberGroup
                                   from m in memberGroup.DefaultIfEmpty()
                                   where r.Name == "Member"
                                      && u.GymId == currentUser.GymId
                                      && u.BaleChatId.HasValue
                                      && u.BaleChatId > 0
                                      && u.IsActive == true
                                   select new MemberSelectItem
                                   {
                                       Id = u.Id,
                                       BaleChatId = u.BaleChatId.Value,
                                       FullName = u.FullName ?? "بدون نام",
                                       PhoneNumber = u.PhoneNumber,
                                       IsExpired = m != null &&
                                                   !string.IsNullOrEmpty(m.MembershipEndDate) &&
                                                   m.MembershipEndDate != "نامحدود" &&
                                                   string.Compare(m.MembershipEndDate.Trim(), todayShamsi, StringComparison.Ordinal) < 0
                                   }).ToListAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendBulkMessage(SendBaleBulkMessageViewModel model)
        {
            var selectedMembers = model.Members?.Where(m => m.IsSelected).ToList();

            // ==========================================================
            // فیلتر امنیتی سمت سرور بر اساس انتخاب رادیوباتن
            // ==========================================================
            if (selectedMembers != null && selectedMembers.Any())
            {
                switch (model.FilterType)
                {
                    case MemberFilterType.Active:
                        selectedMembers = selectedMembers.Where(m => !m.IsExpired).ToList();
                        break;
                    case MemberFilterType.Expired:
                        selectedMembers = selectedMembers.Where(m => m.IsExpired).ToList();
                        break;
                    case MemberFilterType.All:
                    default:
                        break; // نیازی به فیلتر نیست
                }
            }

            if (selectedMembers == null || !selectedMembers.Any())
            {
                ModelState.AddModelError("", "هیچ فردی مطابق با فیلتر انتخابی شما پیدا نشد. لطفاً گزینه‌ها و تیک‌ها را بررسی کنید.");

                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser?.GymId != null)
                {
                    string todayShamsi = DateConverterMlladiToShamsi.ToShamsi(DateTime.Now);
                    model.Members = await (from ur in _db.UserRoles
                                           join u in _db.Users on ur.UserId equals u.Id
                                           join r in _db.Roles on ur.RoleId equals r.Id
                                           join m in _db.Members on u.Id equals m.AppUserId into memberGroup
                                           from m in memberGroup.DefaultIfEmpty()
                                           where r.Name == "Member" && u.GymId == currentUser.GymId && u.BaleChatId.HasValue && u.IsActive == true
                                           select new MemberSelectItem { Id = u.Id, BaleChatId = u.BaleChatId.Value, FullName = u.FullName ?? "بدون نام", PhoneNumber = u.PhoneNumber, IsExpired = m != null && !string.IsNullOrEmpty(m.MembershipEndDate) && m.MembershipEndDate != "نامحدود" && string.Compare(m.MembershipEndDate.Trim(), todayShamsi, StringComparison.Ordinal) < 0 }).ToListAsync();
                }
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.MessageText))
            {
                ModelState.AddModelError("MessageText", "لطفاً متن پیام را وارد کنید.");
                return View(model);
            }

            // ==========================================================
            // دریافت نام باشگاه برای قرار دادن در متن پیام
            // ==========================================================
            var adminUser = await _userManager.GetUserAsync(User);
            var gym = await _db.Gyms.FirstOrDefaultAsync(g => g.Id == adminUser.GymId);
            string gymName = gym?.Name ?? "نامشخص";
            string finalMessage = $"📢 اطلاعیه باشگاه ({gymName}):\n\n{model.MessageText}";

            int sentCount = 0;
            int failedCount = 0;

            foreach (var member in selectedMembers)
            {
                try
                {
                    // ارسال پیام نهایی که نام باشگاه به آن اضافه شده است
                    await _baleBotService.SendMessageAsync(member.BaleChatId, finalMessage);
                    sentCount++;
                }
                catch (Exception) { failedCount++; }
            }

            model.SentCount = sentCount;
            model.FailedCount = failedCount;
            model.IsSubmitted = true;

            return View(model);
        }
    }
}