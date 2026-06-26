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
            var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.BaleChatId == chatId);

            if (existingUser == null)
            {
                var userByPhone = await _db.Users.FirstOrDefaultAsync(u => u.PhoneNumber == NormalizePhoneNumber(rawPhone));

                if (userByPhone != null)
                {
                    userByPhone.BaleChatId = chatId;
                    await _db.SaveChangesAsync();
                    _cache.Remove(chatId.ToString());
                    await _baleBotService.SendMessageAsync(chatId, "✅ حساب کاربری شما با موفقیت در ربات متصل شد.");
                    await _menuService.ShowMainMenu(chatId, userByPhone.FullName ?? "کاربر");
                    return;
                }
                else
                {
                    if (state == null || state.Step != "WAITING_FOR_PHONE")
                    {
                        _cache.Remove(chatId.ToString());

                        // تغییر اصلی اینجا است: بجای ShowErrorWithMenu، منوی ثبت نام نمایش داده می‌شود
                        await _menuService.ShowUnauthenticatedMenu(chatId, "کاربر");
                        return;
                    }
                }
            }

            if (existingUser != null && (state == null || state.Step != "WAITING_FOR_PHONE"))
            {
                await _menuService.ShowErrorWithMenu(chatId, "❌ درخواست ارسال شماره وجود ندارد. لطفاً از منوی اصلی اقدام کنید.");
                return;
            }

            string standardPhone = NormalizePhoneNumber(rawPhone);

            try
            {
                if (state.RegType == "Member")
                    await RegisterMemberDirectly(chatId, standardPhone, state);
                else if (state.RegType == "Manager")
                    await RegisterManagerDirectly(chatId, standardPhone, state);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in direct Bale registration");
                await _menuService.ShowErrorWithMenu(chatId, $"❌ خطای جزئی:\n{ex.Message}");
            }
            finally
            {
                _cache.Remove(chatId.ToString());
            }
        }

        private async Task RegisterMemberDirectly(long chatId, string phone, BotState state)
        {
            if (!state.GymId.HasValue || state.GymId == 0) throw new Exception("GymId is missing");
            if (await _db.Users.AnyAsync(u => u.PhoneNumber == phone && u.GymId == state.GymId))
            {
                await _menuService.ShowErrorWithMenu(chatId, "❌ شما قبلاً در این باشگاه ثبت نام کرده‌اید.");
                return;
            }

            var newUser = new AppUser { FullName = state.FullName, UserName = $"{phone}_{state.GymId}", PhoneNumber = phone, IsActive = true, GymId = state.GymId.Value, BaleChatId = chatId };
            var createUser = await _userManager.CreateAsync(newUser, "FitCore@123");
            if (!createUser.Succeeded) throw new Exception(string.Join("\n", createUser.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(newUser, UserRoles.Member);
            _db.Members.Add(new Member { AppUserId = newUser.Id, IsActive = true });
            await _db.SaveChangesAsync();

            await _baleBotService.SendMessageAsync(chatId, "✅ ثبت نام شما به عنوان عضو باشگاه با موفقیت انجام شد.");
            
            
            await _menuService.ShowMainMenu(chatId, state.FullName);
        }

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
    }
}