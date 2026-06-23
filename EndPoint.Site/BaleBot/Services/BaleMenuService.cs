using FitCore.Application.Contexts;
using FitCore.Application.Services.Auth;
using FitCore.Common.Roles;
using FitCore.Domain.Entities.Users;

using GymBot.Models;
using GymBot.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.BaleBot.Services
{
    public class BaleMenuService : IBaleMenuService
    {
        private readonly IBaleBotService _baleBotService;
        private readonly IDataBaseContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMemoryCache _cache; // اضافه شده برای مرحله لینک شماره

        public BaleMenuService(
            IBaleBotService baleBotService,
            IDataBaseContext db,
            UserManager<AppUser> userManager,
            IMemoryCache cache)
        {
            _baleBotService = baleBotService;
            _db = db;
            _userManager = userManager;
            _cache = cache;
        }

        // این متد منوی اصلی کامل را با تمام دکمه‌ها نشان می‌دهد
        public async Task ShowMainMenu(long chatId, string userName)
        {
            var existingUser = await _db.Users.Include(u => u.Gym).FirstOrDefaultAsync(u => u.BaleChatId == chatId);

            if (existingUser != null)
            {
                var roles = await _userManager.GetRolesAsync(existingUser);
                bool isSuperAdmin = roles.Contains(UserRoles.SuperAdmin);
                bool isMember = roles.Contains(UserRoles.Member);

                var loggedKeyboardRows = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "📋 اطلاعات کلاس‌ها", CallbackData = "INFO_CLASSES" } },
                    new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "📊 نظرسنجی", CallbackData = "SRV_SHOW" } }
                };

                if (isMember)
                {
                    loggedKeyboardRows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "📝 درخواست برنامه جدید از مدیر", CallbackData = "REQ_PLANS_MENU" } });
                    loggedKeyboardRows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "📥 دریافت لیست برنامه‌های من", CallbackData = "VIEW_PLANS_MENU" } });
                }

                if (isSuperAdmin)
                {
                    loggedKeyboardRows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔑 ثبت چت آیدی مدیر", CallbackData = "MYCHATID" } });
                }

                // استخراج قطعی نام باشگاه
                string gymName = "ثبت نشده";
                if (existingUser.GymId.HasValue && existingUser.GymId.Value > 0)
                {
                    if (existingUser.Gym != null)
                        gymName = existingUser.Gym.Name;
                    else
                    {
                        var gymData = await _db.Gyms.FindAsync(existingUser.GymId.Value);
                        if (gymData != null) gymName = gymData.Name;
                    }
                }

                // تعیین نام نقش به فارسی
                string roleName = "کاربر";
                if (isSuperAdmin) roleName = "سوپر ادمین";
                else if (roles.Contains("Admin")) roleName = "مدیر باشگاه";
                else if (isMember) roleName = "عضو باشگاه";

                // ساخت بدنه پیام
                string welcomeText = $"👋 سلام مجدد {existingUser.FullName} عزیز!\n\n" +
                                     $"👤 نقش شما: {roleName}\n" +
                                     $"🏢 نام باشگاه: {gymName}\n" +
                                     $"📱 شماره موبایل: {existingUser.PhoneNumber}\n" +
                                     $"🆔 شناسه چت بله: {existingUser.BaleChatId}";

                // اطلاعات عضویت
                if (isMember)
                {
                    var memberInfo = await _db.Members.FirstOrDefaultAsync(m => m.AppUserId == existingUser.Id);
                    if (memberInfo != null && !string.IsNullOrEmpty(memberInfo.MembershipStartDate))
                    {
                        welcomeText += $"\n\n🗓️ **وضعیت عضویت شما:**\n" +
                                       $"├ 📅 از تاریخ: {memberInfo.MembershipStartDate}\n" +
                                       $"└ 📅 تا تاریخ: {memberInfo.MembershipEndDate ?? "نامحدود"}";
                    }
                    else
                    {
                        welcomeText += "\n\nℹ️ وضعیت عضویت: هنوز دوره عضویتی برای شما تعریف نشده است.";
                    }
                }

                welcomeText += "\n\n✅ از منوی زیر خدمات مورد نظر خود را انتخاب کنید:";

                var loggedKeyboard = new InlineKeyboardMarkup { InlineKeyboard = loggedKeyboardRows };
                await _baleBotService.SendMessageAsync(chatId, welcomeText, loggedKeyboard);
            }
            else
            {
                // ==========================================================
                // کاربر پیدا نشد -> درخواست وصل شدن با شماره موبایل
                // ==========================================================
                _cache.Set(chatId.ToString(), new BaleBot.Models.BotState { Step = "WAITING_FOR_LINK_PHONE" });

                await _baleBotService.SendMessageAsync(chatId, $"به سیستم مدیریت باشگاهای فیتکور FitCore خوش آمدید {userName} عزیز.\nبرای اتصال به حساب کاربری خود در سایت، لطفاً شماره موبایلی که با آن ثبت نام کرده‌اید را ارسال کنید:");
                await _baleBotService.SendMessageWithContactKeyboardAsync(chatId, "📱 ارسال شماره موبایل");
            }
        }

        public async Task ShowErrorWithMenu(long chatId, string errorMessage)
        {
            var keyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔙 بازگشت به منوی اصلی", CallbackData = "MAIN_MENU" } }
                }
            };
            await _baleBotService.SendMessageAsync(chatId, errorMessage, keyboard);
        }

        public async Task SendSurveyToBale(long chatId)
        {
            string TextMessge = "نظر سنجی\nهمکارم محترم، با سلام\nلطفا نظر خود را نسبت به عملکرد شرکت بیمه پارسیان در خصوص خدمات بیمه تکمیلی در سال 1404 را اعلام بفرمائید.\nبا تشکر- امور اداری\n";

            var btn1 = new InlineKeyboardButton { Text = "ضعیف", CallbackData = "SRV_Survey1" };
            var btn2 = new InlineKeyboardButton { Text = "متوسط", CallbackData = "SRV_Survey2" };
            var btn3 = new InlineKeyboardButton { Text = "خوب", CallbackData = "SRV_Survey3" };
            var btn4 = new InlineKeyboardButton { Text = "عالی", CallbackData = "SRV_Survey4" };

            var row = new List<InlineKeyboardButton> { btn1, btn2, btn3, btn4 };
            var keyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>> { row }
            };

            await _baleBotService.SendMessageAsync(chatId, TextMessge, keyboard);
        }
    }
}