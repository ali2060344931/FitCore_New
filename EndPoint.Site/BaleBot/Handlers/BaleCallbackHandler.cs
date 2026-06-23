using EndPoint.Site.BaleBot.Models;
using EndPoint.Site.BaleBot.Services;

using FitCore.Application.Contexts;
using FitCore.Application.Services.Auth;
using FitCore.Application.Services.NutritionProgramReports.Queries;
using FitCore.Application.Services.TrainingProgramReports.Queries;

using GymBot.Models;
using GymBot.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.BaleBot.Handlers
{
    public interface IBaleCallbackHandler
    {
        Task HandleAsync(long chatId, string data, string callbackId, string userName);
    }

    public class BaleCallbackHandler : IBaleCallbackHandler
    {
        private readonly IDataBaseContext _db;
        private readonly IBaleBotService _baleBotService;
        private readonly IMemoryCache _cache;
        private readonly IBaleMenuService _menuService;
        private readonly IGetTrainingProgramPdfService _trainingPdfService;
        private readonly IGetNutritionProgramPdfService _nutritionPdfService;
        private readonly ILogger<BaleCallbackHandler> _logger;

        public BaleCallbackHandler(
            IDataBaseContext db,
            IBaleBotService baleBotService,
            IMemoryCache cache,
            IBaleMenuService menuService,
            IGetTrainingProgramPdfService trainingPdfService,
            IGetNutritionProgramPdfService nutritionPdfService,
            ILogger<BaleCallbackHandler> logger)
        {
            _db = db;
            _baleBotService = baleBotService;
            _cache = cache;
            _menuService = menuService;
            _trainingPdfService = trainingPdfService;
            _nutritionPdfService = nutritionPdfService;
            _logger = logger;
        }

        public async Task HandleAsync(long chatId, string data, string callbackId, string userName)
        {
            string responseText = "";
            InlineKeyboardMarkup keyboard = null;

            if (data == "MAIN_MENU")
            {
                _cache.Remove(chatId.ToString());
                await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                await _menuService.ShowMainMenu(chatId, userName);
                return;
            }

            if (data == "INFO_CLASSES")
            {
                await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                await _baleBotService.SendMessageAsync(chatId, "زمانبندی کلاس‌ها:\n- ایروبیک: شنبه و سه‌شنبه ۱۸:۰۰\n- بدنسازی: روزهای زوج ۱۹:۰۰\n- یوگا: پنجشنبه ۱۰:۰۰");
                return;
            }
            else if (data == "SRV_SHOW")
            {
                await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                await _menuService.SendSurveyToBale(chatId);
                return;
            }
            else if (data == "MYCHATID")
            {
                await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                try
                {
                    var setting = await _db.Setings.FirstOrDefaultAsync(s => s.Code == "01");
                    if (setting != null)
                    {
                        setting.SuperAdminChatId = chatId;
                        await _db.SaveChangesAsync();
                        await _baleBotService.SendMessageAsync(chatId, $"✅ چت آیدی شما با موفقیت ثبت شد.\nشماره آیدی: {chatId}");
                    }
                    else { await _baleBotService.SendMessageAsync(chatId, "❌ خطا: رکورد تنظیمات یافت نشد."); }
                }
                catch (Exception ex) { _logger.LogError(ex, "Error saving SuperAdminChatId"); }
                return;
            }

            // ================= ثبت نام مدیر ----------------
            else if (data == "REG_MANAGER")
            {
                _cache.Set(chatId.ToString(), new BotState { Step = "WAITING_FOR_PROVINCE", RegType = "Manager" });
                var provinces = await _db.Provinces.OrderBy(p => p.Name).ToListAsync();
                var rows = provinces.Select(p => new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = p.Name, CallbackData = $"PROV_{p.Id}" } }).ToList();
                keyboard = new InlineKeyboardMarkup { InlineKeyboard = rows };
                responseText = "لطفاً استان محل باشگاه خود را انتخاب کنید:";
            }
            else if (data.StartsWith("PROV_"))
            {
                int provId = int.Parse(data.Split('_')[1]);
                var state = _cache.Get<BotState>(chatId.ToString());
                if (state != null) { state.ProvinceId = provId; state.Step = "WAITING_FOR_CITY"; _cache.Set(chatId.ToString(), state); }
                var cities = await _db.Cities.Where(c => c.ProvincesId == provId).OrderBy(c => c.Name).ToListAsync();
                var rows = cities.Select(c => new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = c.Name, CallbackData = $"CITY_{c.Id}" } }).ToList();
                keyboard = new InlineKeyboardMarkup { InlineKeyboard = rows };
                responseText = "عالی! حالا شهر را انتخاب کنید:";
            }
            else if (data.StartsWith("CITY_"))
            {
                int cityId = int.Parse(data.Split('_')[1]);
                var state = _cache.Get<BotState>(chatId.ToString());
                if (state != null) { state.CityId = cityId; state.Step = "WAITING_FOR_GYM_NAME"; _cache.Set(chatId.ToString(), state); }
                responseText = "لطفاً نام باشگاه خود را تایپ کنید:";
            }

            // ---------------- ثبت نام عضو ----------------
            else if (data == "REG_MEMBER")
            {
                var gyms = await _db.Gyms.Where(g => g.IsActive).OrderBy(g => g.Name).ToListAsync();
                var rows = gyms.Select(g => new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = g.Name, CallbackData = $"GYM_{g.Id}" } }).ToList();
                keyboard = new InlineKeyboardMarkup { InlineKeyboard = rows };
                _cache.Set(chatId.ToString(), new BotState { Step = "WAITING_FOR_GYM", RegType = "Member" });
                responseText = "لطفاً باشگاهی که می‌خواهید در آن عضو شوید را انتخاب کنید:";
            }
            else if (data.StartsWith("GYM_"))
            {
                long gymId = long.Parse(data.Split('_')[1]);
                var state = _cache.Get<BotState>(chatId.ToString());
                if (state != null) { state.GymId = gymId; state.Step = "WAITING_FOR_NAME"; _cache.Set(chatId.ToString(), state); }
                responseText = "لطفاً نام و نام خانوادگی خود را تایپ کنید:";
            }

            // ---------------- درخواست و دریافت برنامه ----------------
            else if (data == "REQ_PLANS_MENU")
            {
                keyboard = new InlineKeyboardMarkup { InlineKeyboard = new List<List<InlineKeyboardButton>> { new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🥩 درخواست برنامه غذایی", CallbackData = "SEND_REQ_NUTRITION" } }, new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "💪 درخواست برنامه تمرینی", CallbackData = "SEND_REQ_TRAINING" } }, new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔄 درخواست هر دو برنامه", CallbackData = "SEND_REQ_BOTH" } }, new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔙 بازگشت", CallbackData = "MAIN_MENU" } } } };
                responseText = "لطفاً نوع برنامه‌ای که از مدیر باشگاه خود درخواست دارید را انتخاب کنید:";
            }
            else if (data == "SEND_REQ_NUTRITION" || data == "SEND_REQ_TRAINING" || data == "SEND_REQ_BOTH")
            {
                string reqType = data == "SEND_REQ_NUTRITION" ? "غذایی" : (data == "SEND_REQ_TRAINING" ? "تمرینی" : "غذایی و تمرینی");
                var user = await _db.Users.FirstOrDefaultAsync(u => u.BaleChatId == chatId);
                if (user != null)
                {
                    try { var adminSetting = await _db.Setings.FirstOrDefaultAsync(s => s.Code == "01"); if (adminSetting != null && adminSetting.SuperAdminChatId.HasValue) { _ = _baleBotService.SendMessageAsync(adminSetting.SuperAdminChatId.Value, $"🔔 درخواست برنامه جدید\n👤 کاربر: {user.FullName}\n📱 شماره: {user.PhoneNumber}\n📋 نوع درخواست: {reqType}"); } } catch (Exception ex) { _logger.LogError(ex, "Error sending notification to admin"); }
                    responseText = $"✅ درخواست شما برای دریافت برنامه {reqType} با موفقیت ثبت شد.";
                }
                else { responseText = "❌ خطا در شناسایی حساب کاربری شما."; }
            }
            else if (data == "VIEW_PLANS_MENU")
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.BaleChatId == chatId);
                var member = await _db.Members.FirstOrDefaultAsync(m => m.AppUserId == user.Id);
                
                if (member == null)
                { responseText = "❌ پروفایل عضویت شما یافت نشد."; }
                else
                {
                    var rows = new List<List<InlineKeyboardButton>>();
                    // خواندن برنامه‌های غذایی
                    var nutritions = await _db.NutritionPrograms
                        .Include(n => n.ProgramType) // لود کردن نوع برنامه
                        .Include(n => n.GoalType)   // لود کردن هدف برنامه (اضافه شده)
                        .Where(n => n.MemberId == member.Id)
                        .OrderByDescending(n => n.Id) // مرتب‌سازی بر اساس جدیدترین (Id بزرگتر در اولویت اول)
                        .ToListAsync();

                    foreach (var n in nutritions)
                    {
                        // ترکیب هوشمندانه نام نوع و هدف برنامه
                        var titleParts = new List<string>();
                        if (!string.IsNullOrEmpty(n.ProgramType?.Name))
                            titleParts.Add(n.ProgramType.Name);

                        if (!string.IsNullOrEmpty(n.GoalType?.Name))
                            titleParts.Add(n.GoalType.Name);

                        // اگر هر دو وجود داشتند می‌نویسد: "کاهش وزن - عضله‌سازی"
                        // اگر فقط یکی بود فقط همان را می‌نویسد
                        string title = titleParts.Count > 0 ? string.Join(" - ", titleParts) : "بدون عنوان";

                        rows.Add(new List<InlineKeyboardButton> {
                            new InlineKeyboardButton { Text = $"🥩 برنامه غذایی: {title}", CallbackData = $"DL_NUT_{n.Id}" }
                        });
                    }






                    // خواندن برنامه‌های تمرینی (لود کردن جداول وابسته و مرتب‌سازی)
                    var trainings = await _db.TrainingPrograms
                        .Include(t => t.TrainingProgramType) // لود کردن نوع برنامه
                        .Include(t => t.TrainingGoalType)   // لود کردن هدف تمرینی
                        .Where(t => t.MemberId == member.Id)
                        .OrderByDescending(t => t.Id)        // جدیدترین برنامه در اولویت اول
                        .ToListAsync();

                    foreach (var t in trainings)
                    {
                        // شروع ساخت عنوان با عنوان اصلی
                        string displayTitle = t.Title ?? "بدون عنوان";

                        // استخراج نام نوع و هدف (اگر وجود داشتند)
                        var details = new List<string>();
                        if (!string.IsNullOrEmpty(t.TrainingProgramType?.Name))
                            details.Add(t.TrainingProgramType.Name);

                        if (!string.IsNullOrEmpty(t.TrainingGoalType?.Name))
                            details.Add(t.TrainingGoalType.Name);

                        // اگر نوع یا هدف وجود داشت، آن‌ها را داخل پرانتز به عنوان می‌چسبانیم
                        // خروجی مثال: "برنامه تمرینی ۳ روز در هفته (قدرتی - افزایش حجم)"
                        if (details.Count > 0)
                        {
                            displayTitle += $" ({string.Join(" - ", details)})";
                        }

                        // برش کردن متن اگر خیلی طولانی شد تا دکمه در بله زشت نشود
                        if (displayTitle.Length > 45)
                            displayTitle = displayTitle.Substring(0, 45) + "...";

                        rows.Add(new List<InlineKeyboardButton> {
                            new InlineKeyboardButton { Text = $"💪 برنامه تمرینی: {displayTitle}", CallbackData = $"DL_TRN_{t.Id}" }
                        });
                    }




                    if (rows.Count == 0)
                    
                    { responseText = "ℹ️ شما هنوز هیچ برنامه‌ای دریافت نکرده‌اید."; }
                    else { rows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔙 بازگشت به منوی اصلی", CallbackData = "MAIN_MENU" } }); keyboard = new InlineKeyboardMarkup { InlineKeyboard = rows }; responseText = "📥 لطفاً برنامه مورد نظر خود را برای دانلود انتخاب کنید:"; }
                }
            }
            else if (data.StartsWith("DL_NUT_") || data.StartsWith("DL_TRN_"))
            {
                await _baleBotService.AnswerCallbackQueryAsync(callbackId, "در حال ساخت فایل PDF...");
                bool isNutrition = data.StartsWith("DL_NUT_");
                int planId = int.Parse(data.Split('_')[2]);
                try
                {
                    if (isNutrition) { var plan = await _db.NutritionPrograms.FirstOrDefaultAsync(n => n.Id == planId); if (plan == null) throw new Exception("برنامه یافت نشد"); string title = plan.ProgramType?.Name ?? "بدون عنوان"; byte[] pdfBytes = _nutritionPdfService.Execute(planId); await _baleBotService.SendDocumentAsync(chatId, pdfBytes, $"Nutrition_{title}.pdf", $"📄 برنامه غذایی شما: {title}"); }
                    else { var plan = await _db.TrainingPrograms.FirstOrDefaultAsync(t => t.Id == planId); if (plan == null) throw new Exception("برنامه یافت نشد"); byte[] pdfBytes = _trainingPdfService.Execute(planId); await _baleBotService.SendDocumentAsync(chatId, pdfBytes, $"Training_{plan.Title}.pdf", $"📄 برنامه تمرینی شما: {plan.Title}"); }
                }
                catch (Exception ex) { _logger.LogError(ex, "Error generating or sending PDF for Bale"); await _baleBotService.SendMessageAsync(chatId, "❌ خطایی در ساخت یا ارسال فایل PDF رخ داد."); }
                return;
            }

            await _baleBotService.AnswerCallbackQueryAsync(callbackId);
            if (keyboard != null) await _baleBotService.SendMessageAsync(chatId, responseText, keyboard);
            else await _baleBotService.SendMessageAsync(chatId, responseText);
        }
    }
}