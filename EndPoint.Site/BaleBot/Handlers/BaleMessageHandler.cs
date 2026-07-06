using EndPoint.Site.BaleBot.Models;
using EndPoint.Site.BaleBot.Services;

using FitCore.Application.Contexts;
using FitCore.Application.Services.Auth;
using FitCore.Common.Roles;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Users;

using GymBot.Models;
using GymBot.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.BaleBot.Handlers
{
    public interface IBaleMessageHandler
    {
        Task HandleTextAsync(long chatId, string text);
        Task HandleContactAsync(long chatId, string rawPhone);
    }

    public class BaleMessageHandler : IBaleMessageHandler
    {
        private readonly IDataBaseContext _db;
        private readonly IBaleBotService _baleBotService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMemoryCache _cache;
        private readonly IBaleMenuService _menuService;
        private readonly ILogger<BaleMessageHandler> _logger;

        public BaleMessageHandler(
            IDataBaseContext db,
            IBaleBotService baleBotService,
            UserManager<AppUser> userManager,
            IMemoryCache cache,
            IBaleMenuService menuService,
            ILogger<BaleMessageHandler> logger)
        {
            _db = db;
            _baleBotService = baleBotService;
            _userManager = userManager;
            _cache = cache;
            _menuService = menuService;
            _logger = logger;
        }

        public async Task HandleTextAsync(long chatId, string text)
        {
            var state = _cache.Get<BotState>(chatId.ToString());

            if (state != null)
            {
                if (state.Step == "WAITING_FOR_GYM_NAME")
                {
                    state.GymName = text;
                    state.Step = "WAITING_FOR_NAME";
                    _cache.Set(chatId.ToString(), state);
                    await _baleBotService.SendMessageAsync(chatId, "نام باشگاه ثبت شد.\n لطفاً نام و نام خانوادگی خود را تایپ کنید:");
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
                await _menuService.ShowErrorWithMenu(chatId, "❌ متوجه نشدم. لطفاً از منوی اصلی اقدام کنید.");
            }
        }

        public async Task HandleContactAsync(long chatId, string rawPhone)
        {
            var state = _cache.Get<BotState>(chatId.ToString());
            string standardPhone = NormalizePhoneNumber(rawPhone);

            // ==============================================================
            // حالت ۱: در مرحله ثبت‌نام است
            // ==============================================================
            if (state != null && state.Step == "WAITING_FOR_PHONE")
            {
                try
                {
                    if (state.RegType == "Member")
                        await RegisterMemberDirectly(chatId, standardPhone, state);
                    else if (state.RegType == "Manager")
                        await RegisterManagerDirectly(chatId, standardPhone, state);
                    else if (state.RegType == "Trainer")
                        await RegisterTrainerDirectly(chatId, standardPhone, state);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in direct Bale registration");
                    await _menuService.ShowErrorWithMenu(chatId, $"❌ خطای جزئی:\n{ex.Message}");
                    _cache.Remove(chatId.ToString());
                }
                return;
            }

            // ==============================================================
            // حالت ۲: اتصال اکانت قبلی (WAITING_FOR_LINK_PHONE)
            // ==============================================================
            if (state != null && state.Step == "WAITING_FOR_LINK_PHONE")
            {
                // تمام کاربران با این شماره را پیدا کن (چند باشگاهی)
                var usersByPhone = await _db.Users
                    .Include(u => u.Gym)
                    .Where(u => u.PhoneNumber == standardPhone)
                    .ToListAsync();

                if (!usersByPhone.Any())
                {
                    _cache.Remove(chatId.ToString());
                    await _menuService.ShowUnauthenticatedMenu(chatId, "کاربر");
                    return;
                }

                // BaleChatId را روی همه تنظیم کن
                foreach (var u in usersByPhone)
                {
                    if (u.BaleChatId != chatId)
                        u.BaleChatId = chatId;
                }
                await _db.SaveChangesAsync();
                _cache.Remove(chatId.ToString());

                if (usersByPhone.Count == 1)
                {
                    var singleUser = usersByPhone.First();
                    _menuService.SetUserContext(chatId, singleUser.Id, singleUser.GymId);
                    await _baleBotService.SendMessageAsync(chatId, "✅ حساب کاربری شما با موفقیت در ربات متصل شد.");
                    await _menuService.ShowMainMenu(chatId, singleUser.FullName ?? "کاربر");
                    return;
                }

                // چند اکانت → منوی انتخاب
                await _baleBotService.SendMessageAsync(chatId, "✅ حساب‌های شما پیدا شد:");
                await _menuService.ShowGymSelectionMenu(chatId, usersByPhone);
                return;
            }

            // ==============================================================
            // حالت ۳: شماره ارسال شده ولی state معتبری نیست
            // ==============================================================
            _cache.Remove(chatId.ToString());
            await _menuService.ShowUnauthenticatedMenu(chatId, "کاربر");
        }


        /// <summary>
        /// ثبت نام نهایی مربیان باشگاه‌ها (بدون جدول مجزا - فقط در AppUser)
        /// </summary>
        private async Task RegisterTrainerDirectly(long chatId, string phone, BotState state)
        {
            if (!state.GymId.HasValue || state.GymId == 0)
                throw new Exception("GymId is missing");

            // جلوگیری از ثبت نام تکراری مربی با همین شماره در همین باشگاه
            if (await _db.Users.AnyAsync(u => u.PhoneNumber == phone && u.GymId == state.GymId && u.UserName.Contains("_T_")))
            {
                await _menuService.ShowErrorWithMenu(chatId, "❌ شما قبلاً به عنوان مربی در این باشگاه ثبت نام کرده‌اید.");
                return;
            }

            // ساخت یوزرنیم یکتا با پسوند _T_ تا با نام کاربری Member تداخلی نداشته باشد
            var newUser = new AppUser
            {
                FullName = state.FullName,
                UserName = $"{phone}_T_{state.GymId}",
                PhoneNumber = phone,
                IsActive = false, // نیاز به تایید مدیر دارد
                GymId = state.GymId.Value,
                BaleChatId = chatId
            };

            var createUser = await _userManager.CreateAsync(newUser, "FitCore@123");
            if (!createUser.Succeeded) throw new Exception(string.Join("\n", createUser.Errors.Select(e => e.Description)));

            // اختصاص نقش مربی
            await _userManager.AddToRoleAsync(newUser, UserRoles.Trainer);

            // ذخیره تغییرات (بدون هیچ رفرنسی به جدول Trainers)
            await _db.SaveChangesAsync();

            // تنظیم کنتکست فعلی ربات روی این کاربر جدید
            _menuService.SetUserContext(chatId, newUser.Id, state.GymId.Value);

            // پیدا کردن مدیر باشگاه جهت اطلاع رسانی
            var q = _db.UserRoles
              .Where(r => r.RoleId == 2)
              .Join(_db.Users,
                  r => r.UserId,
                  u => u.Id,
                  (r, u) => new { r, u })
              .Where(x => x.u.GymId == state.GymId).FirstOrDefault();

            // ارسال پیام به مدیر باشگاه جهت اطلاع رسانی ثبت نام جدید مربی
            if (q?.u != null && q.u.BaleChatId > 0)
            {
                await _baleBotService.SendMessageAsync((long)q.u.BaleChatId,
                    "🏋️‍♂️ ثبت نام مربی جدید انجام شد.\n" +
                    "نام و نام خانوادگی: " + state.FullName + "\n" +
                    "تلفن همراه: " + phone + "\n\n" +
                    "⚠️ لطفاً از پنل مدیریت وضعیت او را بررسی و تایید کنید.");
            }

            // ارسال پیام تاییدیه به خود مربی
            await _baleBotService.SendMessageAsync(chatId,
                "✅ ثبت نام شما به عنوان مربی باشگاه با موفقیت انجام شد.\nمنتظر تائید ثبت نام از طرف مدیر باشگاه باشید.");

            // نمایش منوی اصلی
            await _menuService.ShowMainMenu(chatId, state.FullName);
        }



        /// <summary>
        /// ثبت نام اعضاء باشگاه
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="phone"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task RegisterMemberDirectly(long chatId, string phone, BotState state)
        {
            if (!state.GymId.HasValue || state.GymId == 0)
                throw new Exception("GymId is missing");

            if (await _db.Users.AnyAsync(u => u.PhoneNumber == phone && u.GymId == state.GymId))
            {
                await _menuService.ShowErrorWithMenu(chatId, "❌ شما قبلاً در این باشگاه ثبت نام کرده‌اید.");
                return;
            }

            var newUser = new AppUser { FullName = state.FullName, UserName = $"{phone}_{state.GymId}", PhoneNumber = phone, IsActive = true, GymId = state.GymId.Value, BaleChatId = chatId };
            var createUser = await _userManager.CreateAsync(newUser, "FitCore@123");
            if (!createUser.Succeeded) throw new Exception(string.Join("\n", createUser.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(newUser, UserRoles.Member);
            _menuService.SetUserContext(chatId, newUser.Id, state.GymId.Value);
            _db.Members.Add(new Member { AppUserId = newUser.Id, IsActive = false });


            await _db.SaveChangesAsync();

            _menuService.SetUserContext(chatId, newUser.Id, state.GymId.Value);

            var q = _db.UserRoles
  .Where(r => r.RoleId == 2)
  .Join(_db.Users,
      r => r.UserId,
      u => u.Id,
      (r, u) => new { r, u })
  .Where(x => x.u.GymId == state.GymId).FirstOrDefault();



            //ارسال پیام به مدیر باشگاه جهت اطلاع رسانی ثبت نام جدید ورزشکار
            if (q?.u != null && q.u.BaleChatId > 0)
            {
                await _baleBotService.SendMessageAsync((long)q.u.BaleChatId, "🙋‍♂️ ثبت نام جدید انجام شد.\nنام و نام خانوادگی: " + state.FullName + "\nتلفن همراه: " + phone);
            }


            await _baleBotService.SendMessageAsync(chatId, "✅ ثبت نام شما به عنوان عضو باشگاه با موفقیت انجام شد.'\n' منتظر تائید ثبت نام از طرف مدیر باشگاه باشید");
            
            
            await _menuService.ShowMainMenu(chatId, state.FullName);
        }

        /// <summary>
        /// ثبت نام نهایی مدیران باشگاها
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="phone"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task RegisterManagerDirectly(long chatId, string phone, BotState state)
        {
            if (await _db.Gyms.AnyAsync(g => g.Name == state.GymName))
            {
                await _menuService.ShowErrorWithMenu(chatId, "❌ متاسفانه نام این باشگاه قبلاً در سیستم ثبت شده است.");
                return;
            }

            Random rnd = new Random();
            string uniqueGymCode = rnd.Next(100000, 999999).ToString();
            while (await _db.Gyms.AnyAsync(g => g.Code == uniqueGymCode))
                uniqueGymCode = rnd.Next(100000, 999999).ToString();

            var newGym = new Gym { Name = state.GymName, CitiesId = state.CityId, MobileNumber = phone, Code = uniqueGymCode, IsActive = false };
            _db.Gyms.Add(newGym);
            await _db.SaveChangesAsync();

            var newUser = new AppUser { FullName = state.FullName, UserName = $"{phone}_{newGym.Id}", PhoneNumber = phone, IsActive = true, GymId = newGym.Id, BaleChatId = chatId };
            var createUser = await _userManager.CreateAsync(newUser, "FitCore@123");
            if (!createUser.Succeeded) throw new Exception(string.Join("\n", createUser.Errors.Select(e => e.Description)));



            await _userManager.AddToRoleAsync(newUser, "Admin");
            _menuService.SetUserContext(chatId, newUser.Id, newGym.Id);
            await _baleBotService.SendMessageAsync(chatId, "✅ ثبت نام شما به عنوان مدیر باشگاه انجام شد.\nحساب شما توسط ادمین سیستم بررسی و تایید نهایی می‌شود.");
            
            
            
            await _menuService.ShowMainMenu(chatId, state.FullName);
        }

        private string NormalizePhoneNumber(string rawPhone)
        {
            if (string.IsNullOrEmpty(rawPhone)) return rawPhone;
            if (rawPhone.StartsWith("+98")) return "0" + rawPhone.Substring(3);
            if (rawPhone.StartsWith("98") && rawPhone.Length == 12) return "0" + rawPhone.Substring(2);
            return rawPhone;
        }


        /// <summary>
        /// پردازش فلو اتصال اکانت قبلی
        /// </summary>
        private async Task ProcessLinkAccountFlow(long chatId, string phone, BotState state)
        {
            var usersByPhone = await _db.Users
                .Include(u => u.Gym)
                .Where(u => u.PhoneNumber == phone)
                .ToListAsync();

            if (!usersByPhone.Any())
            {
                _cache.Remove(chatId.ToString());
                await _menuService.ShowErrorWithMenu(chatId,
                    "❌ شماره موبایل وارد شده در سیستم یافت نشد.\nلطفاً ابتدا در سایت ثبت‌نام کنید یا از گزینه‌های ثبت‌نام ربات استفاده کنید.");
                return;
            }

            // BaleChatId را روی تمام اکانت‌ها تنظیم کن
            foreach (var u in usersByPhone)
            {
                if (u.BaleChatId != chatId)
                {
                    u.BaleChatId = chatId;
                }
            }
            await _db.SaveChangesAsync();

            _cache.Remove(chatId.ToString());

            // فقط یک اکانت
            if (usersByPhone.Count == 1)
            {
                var singleUser = usersByPhone.First();
                _menuService.SetUserContext(chatId, singleUser.Id, singleUser.GymId);

                await _baleBotService.SendMessageAsync(chatId,
                    "✅ حساب کاربری شما با موفقیت در ربات متصل شد.");
                await _menuService.ShowMainMenu(chatId, singleUser.FullName ?? "کاربر");
                return;
            }

            // چند اکانت → منوی انتخاب
            await _baleBotService.SendMessageAsync(chatId,
                "✅ حساب‌های کاربری شما پیدا شد.\nشما در چند باشگاه ثبت‌نام کرده‌اید:");
            await _menuService.ShowGymSelectionMenu(chatId, usersByPhone);
        }
    }
}