using FitCore.Application.Contexts;
using FitCore.Application.Services.Auth;
using FitCore.Common;
using FitCore.Common.Roles;
using FitCore.Domain.Entities.Users;

using GymBot.Models;
using GymBot.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using NuGet.DependencyResolver;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.BaleBot.Services
{
    public interface IBaleMenuService
    {
        Task ShowMainMenu(long chatId, string userName,bool IsStert=false);
        Task ShowErrorWithMenu(long chatId, string errorMessage);
        Task SendSurveyToBale(long chatId);
        Task ShowUnauthenticatedMenu(long chatId, string userName);
        Task EditMemberInfoSend(long memberId);
    }

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
        public async Task ShowMainMenu(long chatId, string userName, bool isStart=false)
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

                // ==========================================================
                // ۱. استخراج اطلاعات عضویت زودتر از موعد برای بررسی اعتبار
                // ==========================================================
                bool isMembershipActive = false;
                int remainingDays = 0; // متغیر جدید برای نگهداری تعداد روزها
                var memberInfo = isMember ? await _db.Members.FirstOrDefaultAsync(m => m.AppUserId == existingUser.Id) : null;
                string todayShamsi = DateConverterMlladiToShamsi.ToShamsi(DateTime.Now);
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
                    remainingDays = DateConverterMlladiToShamsi.GetDaysDifference(memberInfo.MembershipEndDate, todayShamsi);

                    //// اگر بزرگتر مساوی صفر بود یعنی هنوز اعتبار دارد
                    //isMembershipActive = remainingDays >= 0;
                }

                // ==========================================================
                // ۲. نمایش دکمه‌ها فقط در صورت فعال بودن عضویت
                // ==========================================================
                if (isMember && isMembershipActive && memberInfo.IsActive )
                {
                    loggedKeyboardRows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🧑‍💻 اطلاعات کاربر", CallbackData = "MEMBER_INFO_MENU" } });
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


                if(!memberInfo.IsActive)
                
                {
                
                }

                string welcomeText = "";
                if (isStart)

                { // ساخت بدنه پیام
                     welcomeText = $"👋 سلام مجدد {existingUser.FullName} عزیز!\n\n" +
                                         $"👤 نقش شما: {roleName}\n" +
                                         $"🏢 نام باشگاه: {gymName}\n" +
                                         $"📱 شماره موبایل: {existingUser.PhoneNumber}\n" +
                                         $"🗓️ تاریخ تولد: {existingUser.Member.BirthDate}\n" +
                                         $"🗓️ قد(cm): {existingUser.Member.Height}\n" +
                                         $"🆔 شناسه چت بله: {existingUser.BaleChatId}\n"
                                         ;

                    // اطلاعات عضویت
                    if (isMember)
                    {
                        if (memberInfo != null && !string.IsNullOrEmpty(memberInfo.MembershipStartDate))
                        {
                            welcomeText += $"\n\n🗓️ وضعیت عضویت شما: {(isMembershipActive && memberInfo.IsActive ? "✅فعال" : "❌غیرفعال")}\n" +
                                $"مدت زمان اعتبار: {(remainingDays > 0 ? remainingDays + " روز باقیمانده" : remainingDays + " روز منقضی شده")}\n" +
                   $"📅 از تاریخ: {memberInfo.MembershipStartDate}\n" +
                   $"📅 تا تاریخ: {memberInfo.MembershipEndDate ?? "نامحدود"}\n\n" +
                   "👈[برای ورود به سایت فیتکور FitCore: کلیک نمائید](https://www.fitcoreapp.ir/Admin/Auth/Login)";

                        }
                        else
                        {
                            welcomeText += "\n\nℹ️ وضعیت عضویت: هنوز دوره عضویتی برای شما تعریف نشده است.";
                        }
                    }

                    welcomeText += "\n\n✅ از منوی زیر خدمات مورد نظر خود را انتخاب کنید:";
                }
                else
                {
                    welcomeText = "✅ از منوی زیر خدمات مورد نظر خود را انتخاب کنید:";
                }


                loggedKeyboardRows.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "منوی اصلی", CallbackData = "MAIN_MENU" } });

                var loggedKeyboard = new InlineKeyboardMarkup { InlineKeyboard = loggedKeyboardRows };
                
                
                await _baleBotService.SendMessageAsync(chatId, welcomeText, loggedKeyboard);
            }
            else
            {
                // ==========================================================
                // کاربر پیدا نشد -> درخواست وصل شدن با شماره موبایل
                // ==========================================================
                _cache.Set(chatId.ToString(), new BaleBot.Models.BotState { Step = "WAITING_FOR_LINK_PHONE" });

                //await _baleBotService.SendMessageAsync(chatId, $"به سیستم مدیریت باشگاهای فیتکور FitCore خوش آمدید {userName} عزیز.\nبرای اتصال به حساب کاربری خود در سایت، لطفاً شماره موبایلی که با آن ثبت نام کرده‌اید را ارسال کنید:");
                //await _baleBotService.SendMessageWithContactKeyboardAsync(chatId, "📱 ارسال شماره موبایل");

                await ShowUnauthenticatedMenu(chatId, userName);
            }
        }

        public async Task ShowUnauthenticatedMenu(long chatId, string userName)
        {
            var keyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🏢 ثبت‌نام مدیر باشگاه", CallbackData = "REG_MANAGER" } },
                new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🤸‍♂️ ثبت‌نام عضو باشگاه", CallbackData = "REG_MEMBER" } },
                // دکمه زیر اختیاری است اما پیشنهاد می‌شود تا کاربرانی که قبلا در سایت ثبت نام کرده‌اند بتوانند اکانت خود را متصل کنند
                new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🔗 اتصال به حساب کاربری قبلی (ارسال شماره)", CallbackData = "REQ_LINK_PHONE" } }
            }
            };

            await _baleBotService.SendMessageAsync(chatId, $"به سیستم مدیریت باشگاه‌های فیتکور FitCore خوش آمدید {userName} عزیز.\nلطفاً یکی از گزینه‌های زیر را انتخاب کنید:", keyboard);
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
            string TextMessge = "نظر سنجی\nکاربر محترم، با سلام\nلطفا نظر خود را نسبت به عملکرد این نرم افزار  را اعلام بفرمائید.\nبا تشکر- مدیریت سایت فیتکور\n";

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

        public async Task EditMemberInfoSend(long memberId)
        {
            var loggedKeyboardRows = new List<List<InlineKeyboardButton>>
                {
                new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "🧑‍💻 اطلاعات کاربر", CallbackData = "MEMBER_INFO_MENU" } },
                    new List<InlineKeyboardButton> { new InlineKeyboardButton { Text = "منو اصلی", CallbackData = "MAIN_MENU" } }
                };
            var loggedKeyboard = new InlineKeyboardMarkup { InlineKeyboard = loggedKeyboardRows };
            var member = _db.Members
                           .Include(x => x.AppUser)
                           .FirstOrDefault(x => x.Id == memberId);

            await _baleBotService.SendMessageAsync((long)member.AppUser.BaleChatId, "تغییراتی در پنل کاربری از طرف مدیر باشگاه انجام شد"+'\n'+"لطفا بخش اطلاعات کاربر، موارد ویرایش شده را مشاهده نمائید", loggedKeyboard);
        }


    }
}