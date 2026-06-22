using FitCore.Application.Contexts;
using FitCore.Application.Services.Auth;
using FitCore.Common.Roles;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Users;
using FitCore.Application.Services.TrainingProgramReports.Queries;
using FitCore.Application.Services.NutritionProgramReports.Queries;
using GymBot.Models;
using GymBot.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class BaleWebhookController : ControllerBase
    {
        private readonly IBaleBotService _baleBotService;
        private readonly ILogger<BaleWebhookController> _logger;
        private readonly IDataBaseContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGetTrainingProgramPdfService _trainingPdfService;
        private readonly IGetNutritionProgramPdfService _nutritionPdfService;

        public BaleWebhookController(
            IBaleBotService baleBotService,
            ILogger<BaleWebhookController> logger,
            IDataBaseContext db,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IMemoryCache cache,
            IHttpContextAccessor httpContextAccessor, IGetTrainingProgramPdfService trainingPdfService, IGetNutritionProgramPdfService nutritionPdfService)
        {
            _baleBotService = baleBotService;
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _trainingPdfService = trainingPdfService;
            _nutritionPdfService= nutritionPdfService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BaleWebhookPayload payload)
        {
            if (!string.IsNullOrEmpty(payload.Challenge))
                return Ok(payload.Challenge);

            long chatId = 0;
            string userText = "";
            BaleCallbackQuery callback = null;
            BaleContact contact = null;
            string userName = "کاربر";

            if (payload?.Message != null)
            {
                chatId = payload.Message.Chat.Id;
                userText = payload.Message.Text?.Trim() ?? "";
                contact = payload.Message.Contact;
                userName = payload.Message.From?.Name ?? "کاربر";
            }
            else if (payload?.CallbackQuery != null)
            {
                callback = payload.CallbackQuery;
                chatId = callback.From.Id;
                userText = callback.Data ?? "";
                userName = callback.From?.Name ?? "کاربر";
            }

            if (chatId == 0) return Ok();

            // 1. هندل کردن کلیک روی دکمه‌ها
            if (callback != null)
            {
                await HandleCallbacks(chatId, userText, callback.Id, userName);
                return Ok();
            }

            // 2. هندل کردن دریافت شماره تماس
            if (contact != null)
            {
                await HandleContactReceived(chatId, contact.PhoneNumber);
                return Ok();
            }

            // 3. هندل کردن پیام‌های متنی
            if (!string.IsNullOrEmpty(userText))
            {
                if (userText.ToLower() == "/start" || userText.ToLower() == "منوی اصلی")
                {
                    await ShowMainMenu(chatId, userName);
                }
                else
                {
                    await HandleTextReceived(chatId, userText);
                }
            }

            return Ok();
        }

        // -----------------------------------------------------------------
        // متدهای کمکی
        // -----------------------------------------------------------------

        // این متد منوی اصلی کامل را با تمام دکمه‌ها نشان می‌دهد
        private async Task ShowMainMenu(long chatId, string userName)
        {
            var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.BaleChatId == chatId);

            if (existingUser != null)
            {
                var roles = await _userManager.GetRolesAsync(existingUser);
                bool isSuperAdmin = roles.Contains(UserRoles.SuperAdmin);
                bool isMember = roles.Contains(UserRoles.Member); // اضافه شده

                var loggedKeyboardRows = new List<List<InlineKeyboardButton>>
        {
            new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "📋 اطلاعات کلاس‌ها", CallbackData = "INFO_CLASSES" } },
            new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "📊 نظرسنجی", CallbackData = "SRV_SHOW" } }
        };

                // ================= دکمه‌های مخصوص اعضا =================
                if (isMember)
                {
                    loggedKeyboardRows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "📝 درخواست برنامه جدید از مدیر", CallbackData = "REQ_PLANS_MENU" } });
                    loggedKeyboardRows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "📥 دریافت لیست برنامه‌های من", CallbackData = "VIEW_PLANS_MENU" } });
                }
                // =======================================================

                if (isSuperAdmin)
                {
                    loggedKeyboardRows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔑 ثبت چت آیدی مدیر", CallbackData = "MYCHATID" } });
                }

                var loggedKeyboard = new InlineKeyboardMarkup { InlineKeyboard = loggedKeyboardRows };
                await _baleBotService.SendMessageAsync(chatId, $"سلام مجدد {userName} عزیز!\nشما قبلاً ثبت نام کرده‌اید. از منوی زیر خدمات را انتخاب کنید:", loggedKeyboard);
                return;





            }

            // منوی کاربران مهمان (ثبت نشده) - بدون دکمه ثبت آیدی چون قطعاً ادمین نیستند
            var keyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "👑 ثبت نام مدیران باشگاه", CallbackData = "REG_MANAGER" } },
                    new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🏋️ ثبت نام اعضاء باشگاه", CallbackData = "REG_MEMBER" } },
                    new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "📋 اطلاعات کلاس‌ها", CallbackData = "INFO_CLASSES" } },
                    new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "📊 نظرسنجی", CallbackData = "SRV_SHOW" } }
                }
            };
            await _baleBotService.SendMessageAsync(chatId, $"به سیستم باشگاه FitCore خوش آمدید {userName} عزیز.\nلطفاً گزینه مورد نظر را انتخاب کنید:", keyboard);
        }
        // این متد پیام خطا را به همراه دکمه بازگشت به منو نشان می‌دهد
        private async Task ShowErrorWithMenu(long chatId, string errorMessage)
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


        private async Task HandleCallbacks(long chatId, string data, string callbackId, string userName)
        {
            string responseText = "";
            InlineKeyboardMarkup keyboard = null;

            // بازگشت به منوی اصلی
            if (data == "MAIN_MENU")
            {
                _cache.Remove(chatId.ToString());
                await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                await ShowMainMenu(chatId, userName);
                return;
            }

            // ================= ابزارهای عمومی =================
            else if (data == "INFO_CLASSES")
            {
                await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                await _baleBotService.SendMessageAsync(chatId, "زمانبندی کلاس‌ها:\n- ایروبیک: شنبه و سه‌شنبه ۱۸:۰۰\n- بدنسازی: روزهای زوج ۱۹:۰۰\n- یوگا: پنجشنبه ۱۰:۰۰");
                return;
            }
            else if (data == "SRV_SHOW")
            {
                await _baleBotService.AnswerCallbackQueryAsync(callbackId);
                await SendSurveyToBale(chatId);
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
                    else
                    {
                        await _baleBotService.SendMessageAsync(chatId, "❌ خطا: رکورد تنظیمات با کد 01 در دیتابیس یافت نشد.");
                    }
                }
                catch (Exception ex) { _logger.LogError(ex, "Error saving SuperAdminChatId"); }
                return;
            }

            // ================= ثبت نام مدیر ----------------
            else if (data == "REG_MANAGER")
            {
                _cache.Set(chatId.ToString(), new BotState { Step = "WAITING_FOR_PROVINCE", RegType = "Manager" });
                var provinces = await _db.Provinces.OrderBy(p => p.Name).ToListAsync();
                var rows = provinces.Select(p => new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton { Text = p.Name, CallbackData = $"PROV_{p.Id}" }
                }).ToList();
                keyboard = new InlineKeyboardMarkup { InlineKeyboard = rows };
                responseText = "لطفاً استان محل باشگاه خود را انتخاب کنید:";
            }
            else if (data.StartsWith("PROV_"))
            {
                int provId = int.Parse(data.Split('_')[1]);
                var state = _cache.Get<BotState>(chatId.ToString());
                if (state != null) { state.ProvinceId = provId; state.Step = "WAITING_FOR_CITY"; _cache.Set(chatId.ToString(), state); }
                var cities = await _db.Cities.Where(c => c.ProvincesId == provId).OrderBy(c => c.Name).ToListAsync();
                var rows = cities.Select(c => new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton { Text = c.Name, CallbackData = $"CITY_{c.Id}" }
                }).ToList();
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
                var rows = gyms.Select(g => new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton { Text = g.Name, CallbackData = $"GYM_{g.Id}" }
                }).ToList();
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


            // ====================================================================================
            // ================= کدهای جدید اعضا (داینامیک با byte[]) ========================
            // ====================================================================================

            // ---------------- منوی درخواست برنامه ----------------
            else if (data == "REQ_PLANS_MENU")
            {
                keyboard = new InlineKeyboardMarkup
                {
                    InlineKeyboard = new List<List<InlineKeyboardButton>>
                    {
                        new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🥩 درخواست برنامه غذایی", CallbackData = "SEND_REQ_NUTRITION" } },
                        new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "💪 درخواست برنامه تمرینی", CallbackData = "SEND_REQ_TRAINING" } },
                        new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔄 درخواست هر دو برنامه", CallbackData = "SEND_REQ_BOTH" } },
                        new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔙 بازگشت", CallbackData = "MAIN_MENU" } }
                    }
                };
                responseText = "لطفاً نوع برنامه‌ای که از مدیر باشگاه خود درخواست دارید را انتخاب کنید:";
            }
            else if (data == "SEND_REQ_NUTRITION" || data == "SEND_REQ_TRAINING" || data == "SEND_REQ_BOTH")
            {
                string reqType = data == "SEND_REQ_NUTRITION" ? "غذایی" : (data == "SEND_REQ_TRAINING" ? "تمرینی" : "غذایی و تمرینی");
                responseText = $"✅ درخواست شما برای دریافت برنامه {reqType} ثبت شد.\nمنتظر تایید و ارسال برنامه توسط مدیر باشید.";
            }

            // ---------------- منوی دریافت لیست برنامه‌ها ----------------
            else if (data == "VIEW_PLANS_MENU")
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.BaleChatId == chatId);
                var member = await _db.Members.FirstOrDefaultAsync(m => m.AppUserId == user.Id);

                if (member == null)
                {
                    responseText = "❌ پروفایل عضویت شما یافت نشد.";
                }
                else
                {
                    var rows = new List<List<InlineKeyboardButton>>();

                    // خواندن برنامه‌های غذایی
                    var nutritions = await _db.NutritionPrograms
                        .Where(n => n.MemberId == member.Id)
                        .Select(n => new { n.Id, n.Description }) // Description جایگزین Title شد
                        .ToListAsync();

                    foreach (var n in nutritions)
                    {
                        // اگر توضیحات طولانی بود، آن را کوتاه می‌کنیم تا دکمه زشت نشود
                        string desc = string.IsNullOrEmpty(n.Description) ? "بدون عنوان" : n.Description;
                        if (desc.Length > 30) desc = desc.Substring(0, 30) + "...";

                        rows.Add(new List<InlineKeyboardButton> {
                            new InlineKeyboardButton { Text = $"🥩 برنامه غذایی: {desc}", CallbackData = $"DL_NUT_{n.Id}" }
                        });
                    }                    // خواندن برنامه‌های تمرینی
                    var trainings = await _db.TrainingPrograms
                        .Where(t => t.MemberId == member.Id)
                        .Select(t => new { t.Id, t.Title })
                        .ToListAsync();

                    foreach (var t in trainings)
                    {
                        rows.Add(new List<InlineKeyboardButton> {
                            new InlineKeyboardButton { Text = $"💪 برنامه تمرینی: {t.Title}", CallbackData = $"DL_TRN_{t.Id}" }
                        });
                    }

                    if (rows.Count == 0)
                    {
                        responseText = "ℹ️ شما هنوز هیچ برنامه‌ای دریافت نکرده‌اید.";
                    }
                    else
                    {
                        rows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔙 بازگشت به منوی اصلی", CallbackData = "MAIN_MENU" } });
                        keyboard = new InlineKeyboardMarkup { InlineKeyboard = rows };
                        responseText = "📥 لطفاً برنامه مورد نظر خود را برای دانلود انتخاب کنید:";
                    }
                }
            }

            // ---------------- ساخت PDF به صورت داینامیک و ارسال به بله ----------------
            else if (data.StartsWith("DL_NUT_") || data.StartsWith("DL_TRN_"))
            {
                await _baleBotService.AnswerCallbackQueryAsync(callbackId, "در حال ساخت فایل PDF..."); // نمایش لودینگ در بله

                bool isNutrition = data.StartsWith("DL_NUT_");
                int planId = int.Parse(data.Split('_')[2]);

                try
                {
                    if (isNutrition)
                    {
                        var plan = await _db.NutritionPrograms.FirstOrDefaultAsync(n => n.Id == planId);
                        if (plan == null) throw new Exception("برنامه یافت نشد");

                        // استفاده از Description به جای Title
                        string title = string.IsNullOrEmpty(plan.Description) ? "بدون عنوان" : plan.Description;

                        byte[] pdfBytes = _nutritionPdfService.Execute(planId);
                        string fileName = $"Nutrition_{title}.pdf";

                        // ارسال مستقیم فایل بایتی به ربات
                        await _baleBotService.SendDocumentAsync(chatId, pdfBytes, fileName, $"📄 برنامه غذایی شما: {title}");
                    }
                    else
                    {
                        var plan = await _db.TrainingPrograms.FirstOrDefaultAsync(t => t.Id == planId);
                        if (plan == null) throw new Exception("برنامه یافت نشد");

                        string title = plan.Title ?? "بدون عنوان";

                        byte[] pdfBytes = _trainingPdfService.Execute(planId);
                        string fileName = $"Training_{title}.pdf";

                        await _baleBotService.SendDocumentAsync(chatId, pdfBytes, fileName, $"📄 برنامه تمرینی شما: {title}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating or sending PDF for Bale");
                    await _baleBotService.SendMessageAsync(chatId, "❌ خطایی در ساخت یا ارسال فایل PDF رخ داد.");
                }

                return;
            }
            // ====================================================================================
            // ================= پایان کدهای جدید ==========================================
            // ====================================================================================


            // ارسال پاسخ نهایی برای کدهایی که بالاتر return نشده‌اند
            await _baleBotService.AnswerCallbackQueryAsync(callbackId);
            if (keyboard != null)
                await _baleBotService.SendMessageAsync(chatId, responseText, keyboard);
            else
                await _baleBotService.SendMessageAsync(chatId, responseText);
        }


        private async Task HandleTextReceived(long chatId, string text)
        {
            var state = _cache.Get<BotState>(chatId.ToString());

            if (state != null)
            {
                if (state.Step == "WAITING_FOR_GYM_NAME")
                {
                    state.GymName = text;
                    state.Step = "WAITING_FOR_NAME";
                    _cache.Set(chatId.ToString(), state);
                    await _baleBotService.SendMessageAsync(chatId, "نام باشگاه ثبت شد.\nحالا لطفاً نام و نام خانوادگی خود را تایپ کنید:");
                    return;
                }

                if (state.Step == "WAITING_FOR_NAME")
                {
                    state.FullName = text;
                    state.Step = "WAITING_FOR_PHONE";
                    _cache.Set(chatId.ToString(), state);
                    await _baleBotService.SendMessageWithContactKeyboardAsync(chatId, "نام شما ثبت شد.\nلطفاً دکمه «ارسال شماره موبایل» را بزنید:");
                }
            }
            else
            {
                // اگر کاربر بدون شروع فرآیند، متنی رندوم فرستاد
                await ShowErrorWithMenu(chatId, "❌ متوجه نشدم. لطفاً از منوی اصلی اقدام کنید.");
            }
        }

        private async Task HandleContactReceived(long chatId, string rawPhone)
        {
            var state = _cache.Get<BotState>(chatId.ToString());
            if (state == null || state.Step != "WAITING_FOR_PHONE")
            {
                // اگر شماره فرستاد ولی در مرحله دریافت شماره نبود
                await ShowErrorWithMenu(chatId, "❌ درخواست ارسال شماره وجود ندارد. لطفاً از منوی اصلی اقدام کنید.");
                return;
            }

            string phone = rawPhone;
            if (phone.StartsWith("+98")) phone = "0" + phone.Substring(3);
            else if (phone.StartsWith("98") && phone.Length == 12) phone = "0" + phone.Substring(2);

            try
            {
                if (state.RegType == "Member")
                    await RegisterMemberDirectly(chatId, phone, state);
                else if (state.RegType == "Manager")
                    await RegisterManagerDirectly(chatId, phone, state);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in direct Bale registration");

                // ارسال متن دقیق خطا به کاربر/ادمین برای دیباگ راحت‌تر
                await ShowErrorWithMenu(chatId, $"❌ خطای جزئی:\n{ex.Message}");
            }
            finally
            {
                _cache.Remove(chatId.ToString());
            }
        }

        // -----------------------------------------------------------------
        // متدهای ثبت نام مستقیم در دیتابیس
        // -----------------------------------------------------------------

        private async Task RegisterMemberDirectly(long chatId, string phone, BotState state)
        {
            if (!state.GymId.HasValue || state.GymId == 0)
                throw new Exception("GymId is missing");

            if (await _db.Users.AnyAsync(u => u.PhoneNumber == phone && u.GymId == state.GymId))
            {
                await ShowErrorWithMenu(chatId, "❌ شما قبلاً در این باشگاه ثبت نام کرده‌اید.");
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

            _db.Members.Add(new Member { AppUserId = newUser.Id, IsActive = true });
            await _db.SaveChangesAsync();

            // خط زیر حذف شد چون در وب‌هوک نباید لاگین کرد
            // await _signInManager.SignInAsync(newUser, isPersistent: true);

            // پیام موفقیت و نمایش منوی اصلی
            await _baleBotService.SendMessageAsync(chatId, "✅ ثبت نام شما به عنوان عضو باشگاه با موفقیت انجام شد.\nرمز عبور پیش‌فرض شما: FitCore@123");
            await ShowMainMenu(chatId, state.FullName);
        }

        private async Task RegisterManagerDirectly(long chatId, string phone, BotState state)
        {
            bool gymExists = await _db.Gyms.AnyAsync(g => g.Name == state.GymName);
            if (gymExists)
            {
                await ShowErrorWithMenu(chatId, "❌ متاسفانه نام این باشگاه قبلاً در سیستم ثبت شده است.");
                return;
            }

            Random rnd = new Random();
            string uniqueGymCode = rnd.Next(100000, 999999).ToString();
            while (await _db.Gyms.AnyAsync(g => g.Code == uniqueGymCode))
                uniqueGymCode = rnd.Next(100000, 999999).ToString();

            var newGym = new Gym
            {
                Name = state.GymName,
                CitiesId = state.CityId,
                MobileNumber = phone,
                Code = uniqueGymCode,
                IsActive = false
            };
            _db.Gyms.Add(newGym);
            await _db.SaveChangesAsync();

            var newUser = new AppUser
            {
                FullName = state.FullName,
                UserName = $"{phone}_{newGym.Id}",
                PhoneNumber = phone,
                IsActive = true,
                GymId = newGym.Id,
                BaleChatId = chatId
            };

            var createUser = await _userManager.CreateAsync(newUser, "FitCore@123");
            if (!createUser.Succeeded)
                throw new Exception(string.Join("\n", createUser.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(newUser, "Admin");

            // خط زیر حذف شد
            // await _signInManager.SignInAsync(newUser, isPersistent: true);

            // پیام موفقیت و نمایش منوی اصلی
            await _baleBotService.SendMessageAsync(chatId, "✅ ثبت نام شما به عنوان مدیر باشگاه انجام شد.\nحساب شما توسط ادمین سیستم بررسی و تایید نهایی می‌شود.\nرمز عبور پیش‌فرض: FitCore@123");
            await ShowMainMenu(chatId, state.FullName);
        }
        // -----------------------------------------------------------------
        // متد نظرسنجی
        // -----------------------------------------------------------------
        private async Task SendSurveyToBale(long chatId)
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

        // -----------------------------------------------------------------
        // کلاس موقت برای ذخیره مراحل کاربر در حافظه
        // -----------------------------------------------------------------
        public class BotState
        {
            public string Step { get; set; }
            public string RegType { get; set; }
            public int? ProvinceId { get; set; }
            public int? CityId { get; set; }
            public long? GymId { get; set; }
            public string FullName { get; set; }
            public string GymName { get; set; }
        }
    }
}