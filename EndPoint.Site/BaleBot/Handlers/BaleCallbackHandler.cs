using EndPoint.Site.BaleBot.Models;
using EndPoint.Site.BaleBot.Services;

using FitCore.Application.Contexts;
using FitCore.Application.Services.Auth;
using FitCore.Application.Services.NutritionProgramReports.Queries;
using FitCore.Application.Services.TrainingProgramReports.Queries;
using FitCore.Common.Roles;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.ProgramRequest;
using FitCore.Domain.Entities.Users;

using GymBot.Models;
using GymBot.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EndPoint.Site.BaleBot.Handlers
{
    public interface IBaleCallbackHandler
    {
        Task HandleAsync(long chatId, string data, string callbackId, string userName, long? callbackMessageId = null);
    }



    /// <summary>
    /// ارسال اطلاعات با دکمه ها
    /// </summary>
    public class BaleCallbackHandler : IBaleCallbackHandler
    {
        private readonly IDataBaseContext _db;
        private readonly IBaleBotService _baleBotService;
        private readonly IMemoryCache _cache;
        private readonly IBaleMenuService _menuService;
        private readonly IGetTrainingProgramPdfService _trainingPdfService;
        private readonly IGetNutritionProgramPdfService _nutritionPdfService;
        private readonly ILogger<BaleCallbackHandler> _logger;
        private readonly UserManager<AppUser> _userManager;

        public BaleCallbackHandler(
            IDataBaseContext db,
            IBaleBotService baleBotService,
            IMemoryCache cache,
            IBaleMenuService menuService,
            IGetTrainingProgramPdfService trainingPdfService,
            IGetNutritionProgramPdfService nutritionPdfService,
            ILogger<BaleCallbackHandler> logger,
            UserManager<AppUser> userManager)
        {
            _db = db;
            _baleBotService = baleBotService;
            _cache = cache;
            _menuService = menuService;
            _trainingPdfService = trainingPdfService;
            _nutritionPdfService = nutritionPdfService;
            _logger = logger;
            _userManager = userManager;
        }

        /// <summary>
        /// نام ماه‌های شمسی
        /// </summary>
        private static readonly string[] ShamsiMonthNames =
        {
    "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
    "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
};

        public async Task HandleAsync(long chatId, string data, string callbackId, string userName, long? callbackMessageId = null)
        {
            string responseText = "";
            InlineKeyboardMarkup keyboard = null;

            try
            {


                if (data == "MAIN_MENU")
                {
                    _cache.Remove(chatId.ToString());
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                    await _menuService.ShowMainMenu(chatId, userName);
                    return;
                }

                if (data == "MAIN_MENU_2")
                {
                    _cache.Remove(chatId.ToString());

                    await _menuService.ShowMainMenu(chatId);
                    return;
                }
                //------------------------

                // انتخاب باشگاه (جدید - قبلاً نبود)
                else if (data.StartsWith("SEL_GYM_"))
                {
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);

                    long userId = long.Parse(data.Substring("SEL_GYM_".Length));
                    var selectedUser = await _db.Users
                        .Include(u => u.Gym)
                        .FirstOrDefaultAsync(u => u.Id == userId);

                    if (selectedUser != null)
                    {
                        _menuService.SetUserContext(chatId, selectedUser.Id, selectedUser.GymId);
                        var gymName = selectedUser.Gym?.Name ?? "نامشخص";
                        await _baleBotService.SendMessageAsync(chatId, $"✅ باشگاه «{gymName}» انتخاب شد.");
                        await _menuService.ShowMainMenu(chatId, selectedUser.FullName ?? userName);
                    }
                    else
                    {
                        await _menuService.ShowErrorWithMenu(chatId, "❌ یافت نشد.");
                    }
                    return;
                }

                // تغییر باشگاه (جدید - قبلاً نبود)
                else if (data == "SWITCH_GYM")
                {
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);

                    var allUsers = await _db.Users
                        .Include(u => u.Gym)
                        .Where(u => u.BaleChatId == chatId)
                        .ToListAsync();

                    if (allUsers.Count > 1)
                    {
                        await _menuService.ShowGymSelectionMenu(chatId, allUsers, "🔄 لطفاً باشگاه جدید را انتخاب کنید:");
                    }
                    else
                    {
                        await _menuService.ShowMainMenu(chatId, userName);
                    }
                    return;
                }

                // بازگشت به منوی اولیه (رفع مشکل دکمه)
                else if (data == "BACK_TO_UNAUTH")
                {
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                    _cache.Remove(chatId.ToString());
                    await _menuService.ShowUnauthenticatedMenu(chatId, userName);
                    return;
                }

                //------------------------

                else if (data == "REQ_LINK_PHONE")
                {
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                    _cache.Set(chatId.ToString(), new BotState { Step = "WAITING_FOR_LINK_PHONE" });
                    await _baleBotService.SendMessageAsync(chatId, "لطفاً شماره موبایلی که با آن در سایت ثبت‌نام کرده‌اید را ارسال کنید:");
                    await _baleBotService.SendMessageWithContactKeyboardAsync(chatId, "📱 ارسال شماره موبایل");
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
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                    _cache.Set(chatId.ToString(), new BotState { Step = "WAITING_FOR_PROVINCE", RegType = "Manager" }, TimeSpan.FromMinutes(10));

                    var provinces = await _db.Provinces.OrderBy(p => p.Name).ToListAsync();
                    var rows = provinces.Select(p => new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = p.Name, CallbackData = $"PROV_{p.Id}" } }).ToList();

                    var provKeyboard = new InlineKeyboardMarkup { InlineKeyboard = rows };

                    // استفاده از متد جادویی Edit
                    await SendOrEditAsync(chatId, callbackMessageId, "لطفاً استان محل باشگاه خود را انتخاب کنید:", provKeyboard);
                    return;
                }

                else if (data.StartsWith("PROV_"))
                {
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);

                    int provId = int.Parse(data.Split('_')[1]);
                    var state = _cache.Get<BotState>(chatId.ToString());
                    if (state != null)
                    {
                        state.ProvinceId = provId;
                        state.Step = "WAITING_FOR_CITY";
                        _cache.Set(chatId.ToString(), state, TimeSpan.FromMinutes(10));
                    }

                    var cities = await _db.Cities.Where(c => c.ProvincesId == provId).OrderBy(c => c.Name).ToListAsync();
                    var rows = cities.Select(c => new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = c.Name, CallbackData = $"CITY_{c.Id}" } }).ToList();

                    // اضافه کردن دکمه بازگشت به استان‌ها
                    rows.Add(new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton { Text = "🔙 بازگشت به لیست استان‌ها", CallbackData = "BACK_TO_PROVINCES" }
                });

                    var cityKeyboard = new InlineKeyboardMarkup { InlineKeyboard = rows };

                    // جایگزینی لیست استان‌ها با لیست شهرها
                    await SendOrEditAsync(chatId, callbackMessageId, "عالی! حالا شهر را انتخاب کنید:", cityKeyboard);
                    return;
                }

                // بازگشت از شهر به استان (با ادیت پیام)
                else if (data == "BACK_TO_PROVINCES")
                {
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);

                    var state = _cache.Get<BotState>(chatId.ToString());
                    if (state != null)
                    {
                        state.ProvinceId = null;
                        state.Step = "WAITING_FOR_PROVINCE";
                        _cache.Set(chatId.ToString(), state, TimeSpan.FromMinutes(10));
                    }

                    var provinces = await _db.Provinces.OrderBy(p => p.Name).ToListAsync();
                    var rows = provinces.Select(p => new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = p.Name, CallbackData = $"PROV_{p.Id}" } }).ToList();

                    var provKeyboard = new InlineKeyboardMarkup { InlineKeyboard = rows };

                    // جایگزینی لیست شهرها با لیست استان‌ها
                    await SendOrEditAsync(chatId, callbackMessageId, "لطفاً استان محل باشگاه خود را انتخاب کنید:", provKeyboard);
                    return;
                }

                // بازگشت از تایپ نام باشگاه به لیست شهرها (برای مدیران)
                else if (data == "BACK_TO_CITIES")
                {
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                    var state = _cache.Get<BotState>(chatId.ToString());
                    if (state != null)
                    {
                        state.GymName = null;
                        state.Step = "WAITING_FOR_CITY";
                        _cache.Set(chatId.ToString(), state, TimeSpan.FromMinutes(10));
                    }

                    var cities = await _db.Cities.Where(c => c.ProvincesId == state.ProvinceId).OrderBy(c => c.Name).ToListAsync();
                    var rows = cities.Select(c => new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = c.Name, CallbackData = $"CITY_{c.Id}" } }).ToList();
                    rows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔙 بازگشت به لیست استان‌ها", CallbackData = "BACK_TO_PROVINCES" } });

                    var cityKeyboard = new InlineKeyboardMarkup { InlineKeyboard = rows };

                    // ادیت پیام نام باشگاه به لیست شهرها
                    await SendOrEditAsync(chatId, callbackMessageId, "عالی! حالا شهر را انتخاب کنید:", cityKeyboard);
                    return;
                }

                else if (data.StartsWith("CITY_"))
                {
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);

                    int cityId = int.Parse(data.Split('_')[1]);
                    var state = _cache.Get<BotState>(chatId.ToString());
                    if (state != null)
                    {
                        state.CityId = cityId;
                        state.Step = "WAITING_FOR_GYM_NAME";
                        _cache.Set(chatId.ToString(), state, TimeSpan.FromMinutes(10));
                    }

                    // جایگزینی لیست شهرها با پیام درخواست نام باشگاه (بدون دکمه)
                    await SendOrEditAsync(chatId, callbackMessageId, "لطفاً نام باشگاه خود را تایپ کنید:", null);
                    return;
                }

                // ---------------- لیست باشگاه ها جهت ثبت نام ----------------
                else if (data == "GYM_LIST")
                {
                    keyboard = new InlineKeyboardMarkup
                    {
                        InlineKeyboard = new List<List<InlineKeyboardButton>>
        {
            new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton { Text = "📝♂️ ثبت‌نام اعضاء باشگاه", CallbackData = "REG_MEMBER" }
            },
            new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton { Text = "✴️منوی اصلی", CallbackData = "MAIN_MENU" }
            },
        }
                    };

                    var gyms = await _db.Gyms
                        .Include(g => g.Cities)
                        .ThenInclude(c => c.Provinces)
                        .Where(g => g.IsActive)
                        .OrderBy(g => g.Name)
                        .ToListAsync();

                    string messagText = "";
                    int i = 0;
                    foreach (var item in gyms)
                    {
                        i++;
                        var user = await _db.UserRoles
                            .Where(r => r.RoleId == 2)
                            .Join(_db.Users,
                                r => r.UserId,
                                u => u.Id,
                                (r, u) => new { r, u })
                            .Where(x => x.u.GymId == item.Id)
                            .FirstOrDefaultAsync();

                        messagText += i + ": " + "🆔کد باشگاه: " + item.Code + "\n";

                        messagText += "➖نام باشگاه: " + item.Name + "\n";
                        messagText += "➖نام مدیر: " + (user?.u?.FullName ?? "ثبت نشده") + "\n";
                        messagText += "➖تلفن مدیر: " + (user?.u?.PhoneNumber ?? "ثبت نشده") + "\n";
                        if (item.Cities?.Provinces?.Name != null || item.Cities?.Name != null)
                        {
                            if (item.Cities?.Provinces?.Name != null && item.Cities?.Name != null)
                                messagText += "➖استان - شهر: " + item.Cities.Provinces.Name + " - " + item.Cities.Name + "\n";
                            else if (item.Cities?.Provinces?.Name != null)
                                messagText += "➖استان: " + item.Cities.Provinces.Name + "\n";
                            else if (item.Cities?.Name != null)
                                messagText += "➖شهر: " + item.Cities.Name + "\n";
                        }



                        if (!string.IsNullOrEmpty(item.Address))
                            messagText += "➖آدرس باشگاه: " + item.Address + "\n";

                        if (!string.IsNullOrEmpty(item.Description))
                            messagText += "➖توضیحات: " + item.Description + "\n";

                        messagText += "\n";
                    }

                    await _baleBotService.SendMessageAsync(chatId, messagText, keyboard);
                    return;
                }

                // ---------------- ثبت نام عضو (بروزرسانی شده) ----------------
                else if (data == "REG_MEMBER")
                {
                    // توقف لودینگ دکمه
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);

                    var currentUser = await _menuService.GetContextUserAsync(chatId);
                    long? currentGymId = currentUser?.GymId;

                    // خواندن باشگاه‌های فعال، به جز باشگاه فعلی کاربر
                    var gyms = await _db.Gyms
                        .Where(g => g.IsActive && g.Id != currentGymId)
                        .OrderBy(g => g.Code)
                        .ToListAsync();

                    if (!gyms.Any())
                    {
                        await _baleBotService.SendMessageAsync(chatId, "❌ باشگاه فعال دیگری در سیستم وجود ندارد.");
                        return;
                    }

                    var rows = gyms.Select(g => new List<InlineKeyboardButton> {
                    new InlineKeyboardButton { Text = "کد: " + g.Code+"-"+g.Name, CallbackData = $"GYM_{g.Id}" }
                }).ToList();

                    var gymKeyboard = new InlineKeyboardMarkup { InlineKeyboard = rows };

                    // ارسال مستقیم پیام و کیبورد
                    await _baleBotService.SendMessageAsync(chatId, "🆔 لطفاً کد باشگاهی که می‌خواهید در آن عضو شوید را انتخاب کنید:", gymKeyboard);

                    // شروع فلو ثبت نام در کش
                    _cache.Set(chatId.ToString(), new BotState { Step = "WAITING_FOR_GYM", RegType = "Member" });
                    return; // خروج تضمین‌شده
                }



                // ---------------- ثبت نام مربی ----------------
                else if (data == "REG_TRAINER")
                {
                    // توقف لودینگ دکمه
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);

                    var currentUser = await _menuService.GetContextUserAsync(chatId);
                    long? currentGymId = currentUser?.GymId;

                    // خواندن باشگاه‌های فعال، به جز باشگاه فعلی کاربر
                    var gyms = await _db.Gyms
                        .Where(g => g.IsActive && g.Id != currentGymId)
                        .OrderBy(g => g.Code)
                        .ToListAsync();

                    if (!gyms.Any())
                    {
                        await _baleBotService.SendMessageAsync(chatId, "❌ باشگاه فعال دیگری در سیستم وجود ندارد.");
                        return;
                    }

                    var rows = gyms.Select(g => new List<InlineKeyboardButton> {
        new InlineKeyboardButton { Text = "کد باشگاه: " + g.Code, CallbackData = $"GYM_{g.Id}" }
    }).ToList();

                    var gymKeyboard = new InlineKeyboardMarkup { InlineKeyboard = rows };

                    // ارسال مستقیم پیام و کیبورد
                    await _baleBotService.SendMessageAsync(chatId, "🏋️‍♂️ لطفاً کد باشگاهی که می‌خواهید در آن به عنوان مربی فعالیت کنید را انتخاب کنید:", gymKeyboard);

                    // شروع فلو ثبت نام در کش با RegType برابر Trainer
                    _cache.Set(chatId.ToString(), new BotState { Step = "WAITING_FOR_GYM", RegType = "Trainer" });
                    return;
                }


                else if (data.StartsWith("GYM_"))
                {
                    // توقف لودینگ دکمه
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);

                    long gymId = long.Parse(data.Split('_')[1]);
                    var state = _cache.Get<BotState>(chatId.ToString());
                    if (state != null)
                    {
                        state.GymId = gymId;
                        state.Step = "WAITING_FOR_NAME";
                        _cache.Set(chatId.ToString(), state);
                    }

                    var gym = _db.Gyms.Where(c => c.Id == gymId).First();
                    var q = _db.UserRoles
                        .Where(r => r.RoleId == 2)
                        .Join(_db.Users,
                            r => r.UserId,
                            u => u.Id,
                            (r, u) => new { r, u })
                        .Where(x => x.u.GymId == gymId).FirstOrDefault();

                    // ==========================================
                    // رفع خطای کرش: چک کردن نال بودن مدیر
                    // ==========================================
                    string adminInfo = (q != null) ? $"{q.u.FullName} - {q.u.PhoneNumber}" : "ثبت نشده";

                    responseText = "ℹ️ اطلاعات باشگاه انتخاب شده:\n" +
                       "🆔 کد باشگاه: " + gym.Code + "\n" +
                       "نام باشگاه: " + gym.Name + "\n" +
                       "مدیرباشگاه: " + adminInfo + "\n\n" +
                       "👱‍♂️ لطفاً نام و نام خانوادگی خود را وارد و ارسال نمائید:";

                    // ارسال مستقیم پیام
                    await _baleBotService.SendMessageAsync(chatId, responseText);
                    return; // خروج تضمین‌شده
                }

                //******************Ali*************

                // ==============================================================
                // انتخاب جنسیت / اصلاح نام
                // ==============================================================
                else if (data == "GENDER_MALE" || data == "GENDER_FEMALE" || data == "EDIT_NAME")
                {
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                    var state = _cache.Get<BotState>(chatId.ToString());

                    if (data == "EDIT_NAME")
                    {
                        if (state != null) { state.FullName = null; state.Step = "WAITING_FOR_NAME"; _cache.Set(chatId.ToString(), state, TimeSpan.FromMinutes(10)); }
                        await SendOrEditAsync(chatId, callbackMessageId, "👤 لطفاً نام و نام خانوادگی خود را تایپ کنید:", null);
                        return;
                    }

                    if (state != null) { state.Gender = data == "GENDER_MALE" ? Gender.Male : Gender.Female; state.Step = "WAITING_FOR_BIRTH_YEAR"; _cache.Set(chatId.ToString(), state, TimeSpan.FromMinutes(10)); }

                    var pc = new PersianCalendar();
                    int currentYear = pc.GetYear(DateTime.Now);
                    int startYear = currentYear - 80;
                    int endYear = currentYear - 10;

                    var rows = new List<List<InlineKeyboardButton>>();
                    var currentRow = new List<InlineKeyboardButton>();
                    for (int y = startYear; y <= endYear; y++)
                    {
                        currentRow.Add(new InlineKeyboardButton { Text = y.ToString(), CallbackData = $"BY_{y}" });
                        if (currentRow.Count == 5 || y == endYear) { rows.Add(currentRow); currentRow = new List<InlineKeyboardButton>(); }
                    }
                    rows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔙 بازگشت به جنسیت", CallbackData = "BACK_TO_GENDER" } });

                    await SendOrEditAsync(chatId, callbackMessageId, "📅 لطفاً سال تولد خود را انتخاب کنید:", new InlineKeyboardMarkup { InlineKeyboard = rows });
                    return;
                }

                else if (data == "BACK_TO_GENDER")
                {
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                    var state = _cache.Get<BotState>(chatId.ToString());
                    if (state != null) { state.BirthYear = null; state.Step = "WAITING_FOR_GENDER"; _cache.Set(chatId.ToString(), state, TimeSpan.FromMinutes(10)); }

                    keyboard = new InlineKeyboardMarkup
                    {
                        InlineKeyboard = new List<List<InlineKeyboardButton>>
                    {
                        new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "👨 مرد", CallbackData = "GENDER_MALE" } },
                        new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "👩 زن", CallbackData = "GENDER_FEMALE" } },
                        new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔙 اصلاح نام", CallbackData = "EDIT_NAME" } }
                    }
                    };
                    await SendOrEditAsync(chatId, callbackMessageId, "👤 لطفاً جنسیت خود را انتخاب کنید:", keyboard);
                    return;
                }

                // ==============================================================
                // انتخاب سال -> تبدیل پیام به ماه ها
                // ==============================================================
                else if (data.StartsWith("BY_"))
                {
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                    int year = int.Parse(data.Substring(3));
                    var state = _cache.Get<BotState>(chatId.ToString());
                    if (state != null) { state.BirthYear = year; state.Step = "WAITING_FOR_BIRTH_MONTH"; _cache.Set(chatId.ToString(), state, TimeSpan.FromMinutes(10)); }

                    var rows = new List<List<InlineKeyboardButton>>();
                    var currentRow = new List<InlineKeyboardButton>();
                    for (int m = 1; m <= 12; m++)
                    {
                        currentRow.Add(new InlineKeyboardButton { Text = ShamsiMonthNames[m - 1], CallbackData = $"BM_{m}" });
                        if (currentRow.Count == 4 || m == 12) { rows.Add(currentRow); currentRow = new List<InlineKeyboardButton>(); }
                    }
                    rows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔙 بازگشت به انتخاب سال", CallbackData = "BACK_TO_YEARS" } });

                    await SendOrEditAsync(chatId, callbackMessageId, $"📅 سال {year} انتخاب شد.\n🔹 تاریخ انتخابی: {year}/??/??\nلطفاً ماه تولد را انتخاب کنید:", new InlineKeyboardMarkup { InlineKeyboard = rows });

                    return;
                }

                else if (data == "BACK_TO_YEARS")
                {
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                    var state = _cache.Get<BotState>(chatId.ToString());
                    if (state != null) { state.BirthMonth = null; _cache.Set(chatId.ToString(), state, TimeSpan.FromMinutes(10)); }

                    var pc = new PersianCalendar();
                    int currentYear = pc.GetYear(DateTime.Now);
                    int startYear = currentYear - 80;
                    int endYear = currentYear - 10;

                    var rows = new List<List<InlineKeyboardButton>>();
                    var currentRow = new List<InlineKeyboardButton>();
                    for (int y = startYear; y <= endYear; y++)
                    {
                        currentRow.Add(new InlineKeyboardButton { Text = y.ToString(), CallbackData = $"BY_{y}" });
                        if (currentRow.Count == 5 || y == endYear) { rows.Add(currentRow); currentRow = new List<InlineKeyboardButton>(); }
                    }
                    rows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔙 بازگشت به جنسیت", CallbackData = "BACK_TO_GENDER" } });

                    await SendOrEditAsync(chatId, callbackMessageId, "📅 لطفاً سال تولد خود را انتخاب کنید:\n🔹 فرمت: ????/??/??", new InlineKeyboardMarkup { InlineKeyboard = rows });

                    return;
                }

                // ==============================================================
                // انتخاب ماه -> تبدیل پیام به روزها
                // ==============================================================
                else if (data.StartsWith("BM_"))
                {
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                    int month = int.Parse(data.Substring(3));
                    var state = _cache.Get<BotState>(chatId.ToString());
                    if (state != null) { state.BirthMonth = month; state.Step = "WAITING_FOR_BIRTH_DAY"; _cache.Set(chatId.ToString(), state, TimeSpan.FromMinutes(10)); }

                    int year = state?.BirthYear ?? 1;
                    var pc = new PersianCalendar();
                    int daysInMonth = pc.GetDaysInMonth(year, month);

                    var rows = new List<List<InlineKeyboardButton>>();
                    var currentRow = new List<InlineKeyboardButton>();
                    for (int d = 1; d <= daysInMonth; d++)
                    {
                        currentRow.Add(new InlineKeyboardButton { Text = d.ToString(), CallbackData = $"BD_{d}" });
                        if (currentRow.Count == 7 || d == daysInMonth) { rows.Add(currentRow); currentRow = new List<InlineKeyboardButton>(); }
                    }
                    rows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔙 بازگشت به انتخاب ماه", CallbackData = "BACK_TO_MONTHS" } });

                    await SendOrEditAsync(chatId, callbackMessageId, $"📅 {ShamsiMonthNames[month - 1]} انتخاب شد.\n🔹 تاریخ انتخابی: {state.BirthYear}/{month:D2}/??\nلطفاً روز تولد را انتخاب کنید:", new InlineKeyboardMarkup { InlineKeyboard = rows });

                    return;
                }

                else if (data == "BACK_TO_MONTHS")
                {
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                    var state = _cache.Get<BotState>(chatId.ToString());
                    if (state != null) { state.BirthDay = null; _cache.Set(chatId.ToString(), state, TimeSpan.FromMinutes(10)); }

                    var rows = new List<List<InlineKeyboardButton>>();
                    var currentRow = new List<InlineKeyboardButton>();
                    for (int m = 1; m <= 12; m++)
                    {
                        currentRow.Add(new InlineKeyboardButton { Text = ShamsiMonthNames[m - 1], CallbackData = $"BM_{m}" });
                        if (currentRow.Count == 4 || m == 12) { rows.Add(currentRow); currentRow = new List<InlineKeyboardButton>(); }
                    }
                    rows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔙 بازگشت به انتخاب سال", CallbackData = "BACK_TO_YEARS" } });

                    await SendOrEditAsync(chatId, callbackMessageId, $"📅 لطفاً ماه تولد را انتخاب کنید:\n🔹 تاریخ انتخابی: {state.BirthYear}/??/??", new InlineKeyboardMarkup { InlineKeyboard = rows });


                    return;
                }

                // ==============================================================
                // انتخاب روز (در اینجا پیام جدید می‌فرستیم چون کیبورد نوعش عوض می‌شود)
                // ==============================================================
                else if (data.StartsWith("BD_"))
                {
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                    int day = int.Parse(data.Substring(3));
                    var state = _cache.Get<BotState>(chatId.ToString());
                    if (state != null) { state.BirthDay = day; state.Step = "WAITING_FOR_PHONE"; _cache.Set(chatId.ToString(), state, TimeSpan.FromMinutes(10)); }

                    string birthDate = $"{state.BirthYear}/{state.BirthMonth.Value:D2}/{state.BirthDay.Value:D2}";
                    string genderText = state.Gender == Gender.Male ? "👨 مرد" : "👩 زن";

                    await _baleBotService.SendMessageAsync(chatId, $"✅ اطلاعات شما:\n👤 جنسیت: {genderText}\n📅 تاریخ تولد: {birthDate}\n\nلطفاً دکمه «ارسال شماره موبایل» را بزنید:");
                    await _baleBotService.SendMessageWithContactKeyboardAsync(chatId, "📱 ارسال شماره موبایل");
                    return;
                }


                // ============================================================== 
                // تایید نهایی ثبت نام عضو (پيشنهاد 2)
                // ============================================================== 
                // ============================================================== 
                // تایید نهایی ثبت نام عضو (پيشنهادهای 1 و 3)
                // ============================================================== 
                else if (data == "CONFIRM_MEMBER_REG" || data == "CANCEL_MEMBER_REG")
                {
                    var state = _cache.Get<BotState>(chatId.ToString());

                    // بررسی اینکه آیا استپ درست است یا کش پاک شده
                    if (state == null || state.Step != "WAITING_FOR_CONFIRM")
                    {
                        await _baleBotService.AnswerCallbackQueryAsync(callbackId, "❌ وضعیت منقضی شده است.", true);
                        await _menuService.ShowUnauthenticatedMenu(chatId, "کاربر");
                        return;
                    }

                    if (data == "CANCEL_MEMBER_REG")
                    {
                        await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                        _cache.Remove(chatId.ToString());
                        await _menuService.ShowUnauthenticatedMenu(chatId, "کاربر");
                        return;
                    }

                    // ===== پیشنهاد 3: جلوگیری از دوبار کلیک =====
                    if (state.Step == "PROCESSING")
                    {
                        // اگر دکمه را دو بار زده بود، بهش اخطار می‌دهیم و کد اجرا نمیشه
                        await _baleBotService.AnswerCallbackQueryAsync(callbackId, "⏳ در حال پردازش ثبت‌نام، لطفاً چند لحظه صبر کنید...", true);
                        return;
                    }

                    // تغییر استپ به پردازش تا درخواست‌های همزمان متوقف شوند
                    state.Step = "PROCESSING";
                    _cache.Set(chatId.ToString(), state, TimeSpan.FromMinutes(10));

                    await _baleBotService.AnswerCallbackQueryAsync(callbackId);

                    // ===== پیشنهاد 1: نمایش انیمیشن تایپ در چت کاربر =====
                    await _baleBotService.SendChatActionAsync(chatId, "typing");


                    try
                    {
                        string phone = state.PhoneNumber;
                        if (!state.GymId.HasValue || state.GymId == 0)
                            throw new Exception("GymId is missing");

                        if (await _db.Users.AnyAsync(u => u.PhoneNumber == phone && u.GymId == state.GymId))
                        {
                            await _menuService.ShowErrorWithMenu(chatId, "❌ شما قبلاً در این باشگاه ثبت نام کرده‌اید.");
                            return;
                        }

                        var newUser = new AppUser
                        {
                            FullName = state.FullName,
                            UserName = $"{phone}_{state.GymId}",
                            PhoneNumber = phone,
                            IsActive = true,
                            GymId = state.GymId.Value,
                            BaleChatId = chatId
                        };

                        var createUser = await _userManager.CreateAsync(newUser, "FitCore@123");
                        if (!createUser.Succeeded)
                            throw new Exception(string.Join("\n", createUser.Errors.Select(e => e.Description)));

                        await _userManager.AddToRoleAsync(newUser, UserRoles.Member);
                        _menuService.SetUserContext(chatId, newUser.Id, state.GymId.Value);

                        string birthDate = "";
                        if (state.BirthYear.HasValue && state.BirthMonth.HasValue && state.BirthDay.HasValue)
                            birthDate = $"{state.BirthYear}/{state.BirthMonth.Value:D2}/{state.BirthDay.Value:D2}";

                        var member = new Member
                        {
                            AppUserId = newUser.Id,
                            IsActive = false,
                            BirthDate = birthDate,
                            Gender = state.Gender.GetValueOrDefault(),
                            Height = state.Height
                        };
                        _db.Members.Add(member);
                        await _db.SaveChangesAsync();

                        if (state.Weight.HasValue)
                        {
                            string todayShamsi = PersianDateCalse.ToShamsi(DateTime.Now);
                            _db.memberBodyMeasurements.Add(new MemberBodyMeasurement
                            {
                                MemberId = member.Id,
                                RecordDate = todayShamsi,
                                Weight = state.Weight
                            });
                            await _db.SaveChangesAsync();
                        }

                        var q = _db.UserRoles.Where(r => r.RoleId == 2).Join(_db.Users, r => r.UserId, u => u.Id, (r, u) => new { r, u }).Where(x => x.u.GymId == state.GymId).FirstOrDefault();

                        if (q?.u != null && q.u.BaleChatId > 0)
                        {
                            string genderText = state.Gender.HasValue ? (state.Gender == Gender.Male ? "\nجنسیت: مرد" : "\nجنسیت: زن") : "";
                            string birthText = !string.IsNullOrEmpty(birthDate) ? $"\nتاریخ تولد: {birthDate}" : "";
                            string heightText = state.Height.HasValue ? $"\nقد: {state.Height} سانتی‌متر" : "";
                            string weightText = state.Weight.HasValue ? $"\nوزن: {state.Weight} کیلوگرم" : "";

                            await _baleBotService.SendMessageAsync((long)q.u.BaleChatId, "🙋‍♂️ ثبت نام جدید انجام شد.\nنام و نام خانوادگی: " + state.FullName + "\nتلفن همراه: " + phone + genderText + birthText + heightText + weightText);
                        }

                        await _baleBotService.SendMessageAsync(chatId, "✅ ثبت نام شما به عنوان عضو باشگاه با موفقیت انجام شد.\nمنتظر تائید ثبت نام از طرف مدیر باشگاه باشید.");
                        await _menuService.ShowMainMenu(chatId, state.FullName);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error confirming member registration");
                        await _menuService.ShowErrorWithMenu(chatId, "❌ خطا در ثبت نهایی. لطفاً دوباره تلاش کنید.");
                    }
                    finally
                    {
                        // پاک کردن کش پردازش
                        _cache.Remove(chatId.ToString());
                    }
                    return;
                }

                //******************Ali*************




                // ---------------- درخواست و دریافت برنامه ----------------
                else if (data == "REQ_PLANS_MENU")
                {
                    keyboard = new InlineKeyboardMarkup
                    {
                        InlineKeyboard = new List<List<InlineKeyboardButton>> { new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🥩 درخواست برنامه غذایی", CallbackData = "SEND_REQ_NUTRITION" } }, new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "💪 درخواست برنامه تمرینی", CallbackData = "SEND_REQ_TRAINING" } }, new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔄 درخواست هر دو برنامه", CallbackData = "SEND_REQ_BOTH" } },
                    new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔙 بازگشت", CallbackData = "MAIN_MENU" } } }
                    };
                    responseText = "لطفاً نوع برنامه‌ای که از مدیر باشگاه خود درخواست دارید را انتخاب کنید:";
                }

                else if (data == "SEND_REQ_NUTRITION" || data == "SEND_REQ_TRAINING" || data == "SEND_REQ_BOTH")
                {
                    string reqType = data == "SEND_REQ_NUTRITION" ? "غذایی" : (data == "SEND_REQ_TRAINING" ? "تمرینی" : "غذایی و تمرینی");

                    // تبدیل دیتای دکمه به عدد ذخیره شده در دیتابیس
                    int requestTypeInt = data switch
                    {
                        "SEND_REQ_NUTRITION" => 1,
                        "SEND_REQ_TRAINING" => 2,
                        "SEND_REQ_BOTH" => 3,
                        _ => 0
                    };

                    var user = await _menuService.GetContextUserAsync(chatId);
                    if (user == null) { await _menuService.ShowErrorWithMenu(chatId, "❌ خطا در شناسایی"); return; }
                    var memberid = _db.Members.Where(c => c.AppUserId == user.Id).First().Id;


                    var today = DateTime.Today;
                    var tomorrow = today.AddDays(1);

                    var q = _db.ProgramRequests.Any(c =>
                        c.MemberId == memberid &&
                        c.GymId == user.GymId &&
                        c.RequestType == (ProgramRequestType)requestTypeInt &&
                        c.Status == ProgramRequestStatus.Pending &&
                        c.InsertTime >= today &&
                        c.InsertTime < tomorrow);

                    if (q)
                    {
                        responseText = "❌ شما امروز این درخواست را ارسال کرده اید." + '\n' + "امکان ارسال مجدد وجود ندارد";
                        await _baleBotService.SendMessageAsync(chatId, responseText);
                        return;
                    }
                    if (user != null)
                    {
                        // ==========================================================
                        // ۱. ثبت قطعی درخواست در دیتابیس (داخل Try Catch)
                        // ==========================================================
                        try
                        {
                            var newRequest = new ProgramRequest
                            {
                                //MemberId = user.Id,
                                MemberId = memberid,
                                GymId = user.GymId.HasValue ? user.GymId.Value : 0, // جلوگیری از کرش در صورت نال بودن
                                RequestType = (ProgramRequestType)requestTypeInt,
                                Status = (ProgramRequestStatus)1,
                                MemberNote = "درخواست ارسال شده از طریق ربات بله",
                            };

                            _db.ProgramRequests.Add(newRequest);
                            await _db.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            // نمایش متن دقیق خطا به کاربر برای دیباگ
                            _logger.LogError(ex, "Error saving program request to DB for user {UserId}", user.Id);
                            responseText = $"❌ خطای پایگاه داده:\n{ex.InnerException?.InnerException?.Message ?? ex.Message}";
                        }
                        // ==========================================================
                        // ۲. ارسال نوتیفیکیشن فوری به مدیر در ربات بله (حتی اگر دیتابیس ارور داد)
                        // ==========================================================
                        try
                        {
                            //if (isMember)
                            long? AdminChatId = 0;


                            AdminChatId = _db.UserRoles
        .Where(r => r.RoleId == 2)
        .Join(_db.Users,
            r => r.UserId,
            u => u.Id,
            (r, u) => new { r, u })
        .Where(x => x.u.GymId == user.GymId.Value)
        .Select(x => x.u.BaleChatId)
        .FirstOrDefault();

                            var adminSetting = await _db.Setings.FirstOrDefaultAsync(s => s.Code == "01");
                            if (adminSetting != null && adminSetting.SuperAdminChatId.HasValue)
                            {
                                string adminMessage = $"🔔 **درخواست برنامه جدید**\n\n" +
                                                     $"👤 کاربر: {user.FullName}\n" +
                                                     $"📱 شماره: {user.PhoneNumber}\n" +
                                                     $"📋 نوع درخواست: {reqType}\n" +
                                                     $"🕒 زمان: {DateTime.Now:HH:mm - yyyy/MM/dd}";

                                _ = _baleBotService.SendMessageAsync((long)AdminChatId, adminMessage);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error sending notification to admin");
                        }

                        if (string.IsNullOrEmpty(responseText))
                            responseText = $"✅ درخواست شما برای دریافت برنامه {reqType} با موفقیت ثبت شد.\nمنتظر تایید و ارسال برنامه توسط مدیر باشد.";
                    }
                    else
                    {
                        responseText = "❌ خطا در شناسایی حساب کاربری شما.";
                    }
                }

                else if (data == "VIEW_PLANS_MENU")
                {
                    var user = await _menuService.GetContextUserAsync(chatId);
                    if (user == null) { await _baleBotService.SendMessageAsync(chatId, "❌ خطا در شناسایی حساب کاربری."); return; }
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
                            string icon_ = "✴️";
                            if (n.IsSeen)
                                icon_ = "";

                            rows.Add(new List<InlineKeyboardButton> {
                            new InlineKeyboardButton { Text = $"{icon_}🥩 برنامه غذایی: {title}", CallbackData = $"DL_NUT_{n.Id}" }
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
                            string icon_ = "✴️";
                            if (t.IsSeen)
                                icon_ = "";
                            rows.Add(new List<InlineKeyboardButton> {
                            new InlineKeyboardButton { Text = $"{icon_}💪 برنامه تمرینی: {displayTitle}", CallbackData = $"DL_TRN_{t.Id}" }
                        });
                        }




                        if (rows.Count == 0)
                        {
                            responseText = "ℹ️ شما هنوز هیچ برنامه‌ای دریافت نکرده‌اید.";
                        }
                        else
                        {
                            rows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔙 بازگشت به منوی اصلی", CallbackData = "MAIN_MENU" } });
                            keyboard = new InlineKeyboardMarkup { InlineKeyboard = rows }; responseText = "📥 لطفاً برنامه مورد نظر خود را برای دانلود انتخاب کنید:";
                        }
                    }
                }

                // ---------------- ارسال اطلاعات کاربر ----------------
                else if (data == "MEMBER_INFO_MENU")
                {
                    var rows = new List<List<InlineKeyboardButton>>();

                    var existingUser = await _menuService.GetContextUserAsync(chatId);

                    if (existingUser == null)
                    {
                        await _menuService.ShowErrorWithMenu(chatId, "❌ خطا در شناسایی حساب کاربری. لطفاً از منوی اصلی دوباره وارد شوید.");
                        return;
                    }


                    var roles = await _userManager.GetRolesAsync(existingUser);
                    bool isSuperAdmin = roles.Contains(UserRoles.SuperAdmin);
                    bool isAdmin = roles.Contains(UserRoles.Admin);
                    bool isMember = roles.Contains(UserRoles.Member);


                    //عضو باشگاه
                    if (isMember)
                    {// ==========================================================
                     // ۱. استخراج اطلاعات عضویت زودتر از موعد برای بررسی اعتبار
                     // ==========================================================
                        bool isMembershipActive = false;
                        int remainingDays = 0; // متغیر جدید برای نگهداری تعداد روزها
                        var memberInfo = isMember ? await _db.Members.FirstOrDefaultAsync(m => m.AppUserId == existingUser.Id) : null;
                        string todayShamsi = PersianDateCalse.ToShamsi(DateTime.Now);
                        if (memberInfo != null)
                        {
                            // اگر تاریخ پایان ثبت نشده باشد یا نامحدود باشد، فعال در نظر می‌گیریم
                            if (string.IsNullOrEmpty(memberInfo.MembershipEndDate) || memberInfo.MembershipEndDate == "نامحدود")
                            {
                                isMembershipActive = false;
                            }
                            else
                            {
                                isMembershipActive = string.Compare(memberInfo.MembershipEndDate.Trim(), todayShamsi, StringComparison.Ordinal) >= 0;



                            }
                            // محاسبه اختلاف روز (تاریخ پایان منهای امروز)
                            remainingDays = PersianDateCalse.GetDaysDifference(memberInfo.MembershipEndDate, todayShamsi);

                            //// اگر بزرگتر مساوی صفر بود یعنی هنوز اعتبار دارد
                            //isMembershipActive = remainingDays >= 0;
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
                        else if (isAdmin) roleName = "مدیر باشگاه";
                        else if (isMember) roleName = "عضو باشگاه";

                        string welcomeText = "";
                        string weightInfo = "";
                        var latestWeight = await _db.memberBodyMeasurements
                            .Where(m => m.Member.AppUserId == existingUser.Id)
                            .OrderByDescending(m => m.Id)
                            .FirstOrDefaultAsync();

                        if (latestWeight != null && latestWeight.Weight.HasValue)
                        {
                            weightInfo = $"⚖️ وزن آخرین اندازه‌گیری(kg): {latestWeight.Weight}\n";
                        }


                        welcomeText = $"👋 سلام مجدد {existingUser.FullName} عزیز!\n\n" +
                                            $"🏢 نام باشگاه: {gymName}\n" +
                                            $"👤 نقش شما: {roleName}\n" +
                                            $"📱 شماره موبایل: {existingUser.PhoneNumber}\n" +
                                            $"🗓️ تاریخ تولد: {existingUser.Member.BirthDate}\n" +
                                            $"🗓️ قد(cm): {existingUser.Member.Height}\n" +
                                            weightInfo +
                                            $"🆔 شناسه چت بله: {existingUser.BaleChatId}\n";

                        if (memberInfo != null && !string.IsNullOrEmpty(memberInfo.MembershipStartDate))
                        {
                            welcomeText += $"\n\n🗓️ وضعیت عضویت شما: {(isMembershipActive && memberInfo.IsActive ? "✅فعال" : "❌غیرفعال")}\n" +
                                $"مدت زمان اعتبار: {(remainingDays > 0 ? remainingDays + " روز باقیمانده" : remainingDays + " روز منقضی شده")}\n" +
                   $"📅 از تاریخ: {memberInfo.MembershipStartDate}\n" +
                   $"📅 تا تاریخ: {memberInfo.MembershipEndDate ?? "نامحدود"}\n\n" +
                   "👈[برای ورود به سایت فیتکور FitCore: کلیک نمائید](https://www.fitcoreapp.ir/Admin/Auth/Login)";

                        }
                        else
                        {//⛔🚫
                            welcomeText += "\n\nℹ️ وضعیت عضویت: 🚫در انتظار تائید. هنوز دوره عضویتی برای شما تعریف نشده است.\n\n" +
                                "👈[برای ورود به سایت فیتکور FitCore: کلیک نمائید](https://www.fitcoreapp.ir/Admin/Auth/Login)";
                        }


                        welcomeText += "\n\n👇 جهت ادامه *بازگشت به منوی اصلی* را کلیک نمائید:";

                        //welcomeText += "\n\n✅ از منوی زیر خدمات مورد نظر خود را انتخاب کنید:";
                        rows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔙 بازگشت به منوی اصلی", CallbackData = "MAIN_MENU" } });
                        keyboard = new InlineKeyboardMarkup { InlineKeyboard = rows };

                        await _baleBotService.SendMessageAsync(chatId, welcomeText, keyboard);
                    }
                    //مدیرباشگاه
                    else if (isAdmin)
                    {
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
                        string welcomeText = "";

                        var activeMembers = await _db.Members
                            .CountAsync(m => m.IsActive && m.AppUser.GymId == existingUser.GymId);

                        var inactiveMembers = await _db.Members
                            .CountAsync(m => !m.IsActive && m.AppUser.GymId == existingUser.GymId);



                        welcomeText = $"👋 سلام مجدد {existingUser.FullName} عزیز!\n\n" +
                                            $"🏢 نام باشگاه: {gymName}\n" +
                                            $"👤 نقش شما: مدیر باشگاه\n" +
                                            $"📱 شماره موبایل: {existingUser.PhoneNumber}\n" +

                                            $"🆔 شناسه چت بله: {existingUser.BaleChatId}\n"
                                            ;


                        int day = PersianDateCalse.GetDaysDifference(existingUser.Gym.SubscriptionExpireDate, PersianDateCalse.ToShamsi(DateTime.Now));

                        if (existingUser.IsActive)
                        {
                            welcomeText += $"\n\n🗓️ وضعیت عضویت شما: {(day > 0 ? "✅فعال" : "❌غیرفعال")}\n" +
                                $"مدت زمان اعتبار: {(day > 0 ? day + " روز باقیمانده" : day + " روز منقضی شده")}\n" +
                   $"📅 تا تاریخ: {existingUser.Gym.SubscriptionExpireDate}\n" +

                   $"📅 تعداد عضوهای فعال باشگاه: {activeMembers}\n" +
                   $"📅 تعداد عضوهای غیرفعال باشگاه: {inactiveMembers}\n" +
                   "👈[برای ورود به سایت فیتکور FitCore: کلیک نمائید](https://www.fitcoreapp.ir/Admin/Auth/Login)";

                        }
                        else
                        {//⛔🚫
                            welcomeText += "\n\nℹ️ وضعیت عضویت: 🚫در انتظار تائید. هنوز دوره عضویتی برای شما تعریف نشده است.";
                        }
                        await _baleBotService.SendMessageAsync(chatId, welcomeText, keyboard);
                    }
                    //مدیرسایت
                    else if (isSuperAdmin)
                    {
                        await _baleBotService.SendMessageAsync(chatId, "شما به عنوان مدیر سایت وارد شده اید", keyboard);

                    }
                }
                //دانلود برنامه های غذایی و تمرینی
                else if (data.StartsWith("DL_NUT_") || data.StartsWith("DL_TRN_"))
                {
                    await _baleBotService.AnswerCallbackQueryAsync(callbackId, "در حال ساخت فایل PDF...");
                    await _baleBotService.SendChatActionAsync(chatId, "upload_document");

                    bool isNutrition = data.StartsWith("DL_NUT_");
                    int planId = int.Parse(data.Split('_')[2]);

                    try
                    {
                        if (isNutrition)
                        {
                            var plan = await _db.NutritionPrograms
                                .Include(n => n.ProgramType)
                                .Include(n => n.GoalType)
                                .FirstOrDefaultAsync(n => n.Id == planId);

                            if (plan == null) throw new Exception("برنامه یافت نشد");

                            // ===== شماره‌گذاری هوشمند: محاسبه ترتیب فقط برای این کاربر و فقط برای برنامه‌های غذایی =====
                            int planNumber = await _db.NutritionPrograms
                                .Where(n => n.MemberId == plan.MemberId && n.Id <= plan.Id)
                                .CountAsync();

                            byte[] pdfBytes = _nutritionPdfService.Execute(planId);

                            // ===== ساخت نام فایل با ترتیب دقیق =====
                            var details = new List<string>();
                            if (!string.IsNullOrEmpty(plan.GoalType?.Name)) details.Add(plan.GoalType.Name);
                            if (!string.IsNullOrEmpty(plan.ProgramType?.Name)) details.Add(plan.ProgramType.Name);

                            string detailsStr = string.Join("_", details);

                            // فرمت نهایی: برنامه_غذایی_شماره_01_کاهش_وزن_رژیم.pdf
                            string fileName = !string.IsNullOrWhiteSpace(detailsStr)
                                ? $"برنامه_غذایی_شماره_{planNumber:D2}_{detailsStr}.pdf"
                                : $"برنامه_غذایی_شماره_{planNumber:D2}.pdf";

                            fileName = SanitizeFileName(fileName);

                            // ===== ساخت کپشن خوانا و منظم =====
                            string caption = $"📄 *برنامه غذایی شما (نسخه {planNumber})*\n\n";
                            if (!string.IsNullOrEmpty(plan.GoalType?.Name)) caption += $"🎯 هدف: {plan.GoalType.Name}\n";
                            if (!string.IsNullOrEmpty(plan.ProgramType?.Name)) caption += $"📋 نوع برنامه: {plan.ProgramType.Name}\n";
                            caption += "\n📥 فایل پیوست آماده دانلود است.";

                            await _baleBotService.SendDocumentAsync(chatId, pdfBytes, fileName, caption);
                        }
                        else
                        {
                            var plan = await _db.TrainingPrograms
                                .Include(t => t.TrainingProgramType)
                                .Include(t => t.TrainingGoalType)
                                .FirstOrDefaultAsync(t => t.Id == planId);

                            if (plan == null) throw new Exception("برنامه یافت نشد");

                            // ===== شماره‌گذاری هوشمند: محاسبه ترتیب فقط برای این کاربر و فقط برای برنامه‌های تمرینی =====
                            int planNumber = await _db.TrainingPrograms
                                .Where(t => t.MemberId == plan.MemberId && t.Id <= plan.Id)
                                .CountAsync();

                            byte[] pdfBytes = _trainingPdfService.Execute(planId);

                            // ===== ساخت نام فایل با ترتیب دقیق =====
                            var details = new List<string>();
                            if (!string.IsNullOrEmpty(plan.TrainingGoalType?.Name)) details.Add(plan.TrainingGoalType.Name);
                            if (!string.IsNullOrEmpty(plan.TrainingProgramType?.Name)) details.Add(plan.TrainingProgramType.Name);

                            string detailsStr = string.Join("_", details);
                            string baseTitle = !string.IsNullOrWhiteSpace(plan.Title) ? SanitizeFileName(plan.Title) : "برنامه_تمرینی";

                            // فرمت نهایی: برنامه_تمرینی_شماره_01_قدرتی_افزایش_حجم.pdf
                            string fileName = !string.IsNullOrWhiteSpace(detailsStr)
                                ? $"{baseTitle}_شماره_{planNumber:D2}_{detailsStr}.pdf"
                                : $"{baseTitle}_شماره_{planNumber:D2}.pdf";

                            // ===== ساخت کپشن خوانا و منظم =====
                            string caption = $"📄 *برنامه تمرینی شما (نسخه {planNumber})*\n\n";
                            if (!string.IsNullOrWhiteSpace(plan.Title)) caption += $"🏆 عنوان: {plan.Title}\n";
                            if (!string.IsNullOrEmpty(plan.TrainingGoalType?.Name)) caption += $"🎯 هدف: {plan.TrainingGoalType.Name}\n";
                            if (!string.IsNullOrEmpty(plan.TrainingProgramType?.Name)) caption += $"📋 نوع برنامه: {plan.TrainingProgramType.Name}\n";
                            caption += "\n📥 فایل پیوست آماده دانلود است.";

                            await _baleBotService.SendDocumentAsync(chatId, pdfBytes, fileName, caption);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error generating or sending PDF for Bale");
                        await _baleBotService.SendMessageAsync(chatId, "❌ خطایی در ساخت یا ارسال فایل PDF رخ داد.");
                    }
                    return;
                }

                //لیست درخواست ها
                else if (data == "Program_Request")
                {



                    await _baleBotService.SendMessageAsync(chatId, "لیست درخواست ها");
                }

                //تیکت ها
                else if (data == "Tickets")
                {
                    await _baleBotService.SendMessageAsync(chatId, "لیست تیکت ها");

                }

                //لیست اعضاء
                else if (data == "Member_List")
                {
                    await _baleBotService.SendMessageAsync(chatId, "لیست اعضاء باشگاه");

                }

                await _baleBotService.AnswerCallbackQueryAsync(callbackId);

                if (keyboard != null)
                    await _baleBotService.SendMessageAsync(chatId, responseText, keyboard);
                else await _baleBotService.SendMessageAsync(chatId, responseText);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in BaleCallbackHandler for ChatId: {ChatId}, Data: {Data}", chatId, data);
                try
                {
                    await _baleBotService.SendMessageAsync(chatId, "❌ خطای سیستمی رخ داد. لطفاً کمی بعد تلاش کنید.");
                }
                catch { /* جلوگیری از کرش در صورت قطع بودن ارتباط ربات */ }
            }
        }

        /// <summary>
        /// پاکسازی کاراکترهای غیرمجاز در نام فایل برای جلوگیری از ارور در دانلود موبایل
        /// </summary>
        private string SanitizeFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "file.pdf";

            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                name = name.Replace(c, '_');
            }
            return name;
        }

        /// <summary>
        /// پیشنهاد 5: اگر پیام آیدی وجود داشت پیام قبلی را ادیت می‌کند، در غیر این صورت پیام جدید می‌فرستد
        /// </summary>
        private async Task SendOrEditAsync(long chatId, long? messageId, string text, InlineKeyboardMarkup keyboard)
        {
            if (messageId.HasValue)
            {
                await _baleBotService.EditMessageTextAsync(chatId, messageId.Value, text, keyboard);
            }
            else
            {
                await _baleBotService.SendMessageAsync(chatId, text, keyboard);
            }
        }
    }
}