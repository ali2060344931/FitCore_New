using EndPoint.Site.BaleBot.Models;

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
        Task ShowMainMenu(long chatId, string userName, bool IsStert = false);
        Task ShowErrorWithMenu(long chatId, string errorMessage);
        Task SendSurveyToBale(long chatId);
        Task ShowUnauthenticatedMenu(long chatId, string userName);
        Task EditMemberInfoSend(long BaleChatId, string massege);
        Task ShowMainMenu(long memberId);


        // === متدهای جدید: مدیریت چند باشگاهی ===

        /// <summary>
        /// نمایش منوی انتخاب باشگاه وقتی کاربر در چند باشگاه عضو است
        /// </summary>
        Task ShowGymSelectionMenu(long chatId, List<AppUser> users, string title = null);

        /// <summary>
        /// دریافت کاربر فعلی بر اساس context کش‌شده.
        /// اول از SelectedUserId در کش می‌خواند، سپس از BaleChatId در دیتابیس.
        /// اگر چند کاربر پیدا کرد null برمی‌گرداند (فراخوانی‌کننده باید منوی انتخاب را نشان دهد)
        /// </summary>
        Task<AppUser> GetContextUserAsync(long chatId);

        /// <summary>
        /// بررسی آیا کاربر در بیش از یک باشگاه ثبت‌نام دارد
        /// </summary>
        Task<bool> HasMultipleGymsAsync(long chatId);

        /// <summary>
        /// تنظیم context کاربر در کش
        /// </summary>
        void SetUserContext(long chatId, long? userId, long? gymId);
    }


    public class BaleMenuService : IBaleMenuService
    {
        private readonly IBaleBotService _baleBotService;
        private readonly IDataBaseContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMemoryCache _cache;

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

        #region === متدهای جدید: مدیریت Context چند باشگاهی ===

        /// <inheritdoc />
        /// <inheritdoc />
        public async Task<AppUser> GetContextUserAsync(long chatId)
        {
            // ۱. اول از کش بخوان
            var state = _cache.Get<BotState>(chatId.ToString());

            if (state != null && state.SelectedUserId.HasValue)
            {
                var cachedUser = await _db.Users
                    .Include(u => u.Gym)
                    .FirstOrDefaultAsync(u => u.Id == state.SelectedUserId.Value);

                if (cachedUser != null)
                    return cachedUser;

                // کاربر حذف شده بود → کش پاک شود
                _cache.Remove(chatId.ToString());
            }

            // ۲. از دیتابیس با BaleChatId بگرد
            var users = await _db.Users
                .Include(u => u.Gym)
                .Where(u => u.BaleChatId == chatId)
                .ToListAsync();

            if (users.Count == 0)
                return null;

            if (users.Count == 1)
            {
                var singleUser = users.First();
                SetUserContext(chatId, singleUser.Id, singleUser.GymId);
                return singleUser;
            }

            // ۳. چند کاربر → null
            return null;
        }

        /// <inheritdoc />
        public void SetUserContext(long chatId, long? userId, long? gymId)
        {
            var existingState = _cache.Get<BotState>(chatId.ToString());
            var newState = existingState ?? new BotState();
            newState.SelectedUserId = userId;
            newState.SelectedGymId = gymId;
            _cache.Set(chatId.ToString(), newState);
        }

        /// <inheritdoc />
        public async Task ShowGymSelectionMenu(long chatId, List<AppUser> users, string title = null)
        {
            var rows = new List<List<InlineKeyboardButton>>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                string roleLabel = GetRoleLabel(roles);
                string gymName = u.Gym?.Name ?? "باشگاه نامشخص";

                rows.Add(new List<InlineKeyboardButton>
        {
            new InlineKeyboardButton
            {
                Text = $"🏢 {gymName}  ({roleLabel})",
                CallbackData = $"SEL_GYM_{u.Id}"   // u.Id از نوع long است → تبدیل خودکار به string
            }
        });
            }

            rows.Add(new List<InlineKeyboardButton>
    {
        new InlineKeyboardButton
        {
            Text = "🔙 بازگشت به منوی اولیه",
            CallbackData = "BACK_TO_UNAUTH"
        }
    });

            var keyboard = new InlineKeyboardMarkup { InlineKeyboard = rows };
            string message = title ?? "🏢 شما در چند باشگاه عضو هستید.\nلطفاً باشگاهی که می‌خواهید در آن فعالیت کنید را انتخاب کنید:";

            await _baleBotService.SendMessageAsync(chatId, message, keyboard);
        }
        /// <inheritdoc />
        public async Task<bool> HasMultipleGymsAsync(long chatId)
        {
            var count = await _db.Users
                .Where(u => u.BaleChatId == chatId)
                .CountAsync();
            return count > 1;
        }




        /// <summary>
        /// تبدیل لیست نقش‌ها به برچسب فارسی
        /// </summary>
        private string GetRoleLabel(IList<string> roles)
        {
            if (roles.Contains(UserRoles.SuperAdmin)) return "سوپر ادمین";
            if (roles.Contains("Admin")) return "مدیر";
            if (roles.Contains(UserRoles.Trainer)) return "مربی";
            if (roles.Contains(UserRoles.Member)) return "عضو";
            return "کاربر";
        }
        #endregion

        #region === متدهای اصلی (بروزرسانی‌شده) ===

        /// <summary>
        /// منوی اصلی - بروزرسانی شده برای پشتیبانی از چند باشگاهی
        /// </summary>
        public async Task ShowMainMenu(long chatId, string userName, bool isStart = false)
        {
            // ==========================================================
            // مرحله ۱: دریافت کاربر با context چند باشگاهی
            // ==========================================================
            var existingUser = await GetContextUserAsync(chatId);

            if (existingUser == null)
            {
                // بررسی آیا اصلاً کاربری وجود دارد یا نه
                var anyUser = await _db.Users
                    .AnyAsync(u => u.BaleChatId == chatId);

                if (anyUser)
                {
                    // کاربر وجود دارد اما چند باشگاهی است و باید انتخاب کند
                    var allUsers = await _db.Users
                        .Include(u => u.Gym)
                        .Where(u => u.BaleChatId == chatId)
                        .ToListAsync();

                    await ShowGymSelectionMenu(chatId, allUsers);
                    return;
                }

                // ==========================================================
                // کاربر پیدا نشد → درخواست وصل شدن / ثبت‌نام
                // ==========================================================
                await ShowUnauthenticatedMenu(chatId, userName);
                return;
            }

            // ==========================================================
            // مرحله ۲: کاربر واحد پیدا شد → ساخت منو
            // ==========================================================
            var roles = await _userManager.GetRolesAsync(existingUser);
            bool isSuperAdmin = roles.Contains(UserRoles.SuperAdmin);
            bool isMember = roles.Contains(UserRoles.Member);
            bool isAdmin = roles.Contains(UserRoles.Admin);

            var loggedKeyboardRows = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton { Text = "🧑‍💻 اطلاعات کاربر", CallbackData = "MEMBER_INFO_MENU" }
                },
                //new List<InlineKeyboardButton>
                //{
                //    new InlineKeyboardButton { Text = "📋 اطلاعات کلاس‌ها", CallbackData = "INFO_CLASSES" }
                //},
                //new List<InlineKeyboardButton>
                //{
                //    new InlineKeyboardButton { Text = "📊 نظرسنجی", CallbackData = "SRV_SHOW" }
                //}
            };

            // ==========================================================
            // استخراج اطلاعات عضویت
            // ==========================================================
            bool isMembershipActive = false;
            int remainingDays = 0;
            var memberInfo = isMember
                ? await _db.Members.FirstOrDefaultAsync(m => m.AppUserId == existingUser.Id)
                : null;
            string todayShamsi = PersianDateCalse.ToShamsi(DateTime.Now);

            if (memberInfo != null)
            {
                if (string.IsNullOrEmpty(memberInfo.MembershipEndDate) || memberInfo.MembershipEndDate == "نامحدود")
                {
                    isMembershipActive = false;
                }
                else
                {
                    isMembershipActive = string.Compare(memberInfo.MembershipEndDate.Trim(), todayShamsi, StringComparison.Ordinal) >= 0;
                }
                remainingDays = PersianDateCalse.GetDaysDifference(memberInfo.MembershipEndDate, todayShamsi);
            }

            // ==========================================================
            // دکمه‌های اختصاصی عضو (فقط در صورت فعال بودن عضویت)
            // ==========================================================
            if (isMember && isMembershipActive && memberInfo != null && memberInfo.IsActive)
            {
                loggedKeyboardRows.Add(new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton { Text = "📝 درخواست برنامه جدید از مدیر", CallbackData = "REQ_PLANS_MENU" }
                });
                loggedKeyboardRows.Add(new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton { Text = "📥 دریافت لیست برنامه‌های من", CallbackData = "VIEW_PLANS_MENU" }
                });
            }

            // ==========================================================
            // دکمه‌های سوپر ادمین
            // ==========================================================
            if (isSuperAdmin)
            {
                loggedKeyboardRows.Add(new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton { Text = "🔑 ثبت چت آیدی مدیر", CallbackData = "MYCHATID" }
                });
            }
            // ==========================================================
            // دکمه‌های ادمین
            // ==========================================================
            if (isAdmin)
            {
                loggedKeyboardRows.Add(new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton { Text = "🫲 درخواست های برنامه", CallbackData = "Program_Request" }
                });
                loggedKeyboardRows.Add(new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton { Text = "📨 تیکت ها", CallbackData = "Tickets" }
                });
                loggedKeyboardRows.Add(new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton { Text = "📜 لیست اعضاء", CallbackData = "Member_List" }
                });
            }



            bool isTrainer = roles.Contains(UserRoles.Trainer);
            if (isTrainer)
            {
                loggedKeyboardRows.Add(new List<InlineKeyboardButton>
    {
        new InlineKeyboardButton { Text = "📋 لیست شاگردان من", CallbackData = "TRAINER_MENU" }
    });
            }




            // ==========================================================
            // دکمه‌های چند باشگاهی
            // ==========================================================
            bool hasMultipleGyms = await HasMultipleGymsAsync(chatId);

            loggedKeyboardRows.Add(new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton
                {
                    Text = "📝 ثبت‌نام در باشگاه دیگر",
                    CallbackData = "REG_MEMBER"
                }
            });

            if (hasMultipleGyms)
            {
                string currentGymName = existingUser.Gym?.Name ?? "نامشخص";
                loggedKeyboardRows.Add(new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton
                    {
                        Text = $"🔄 تغییر باشگاه (فعلی: {currentGymName})",
                        CallbackData = "SWITCH_GYM"
                    }
                });
            }

            // ==========================================================
            // دکمه منوی اصلی
            // ==========================================================
            loggedKeyboardRows.Add(new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton { Text = minemenu, CallbackData = "MAIN_MENU" }
            });

            string welcomeText = "✅ از منوی زیر خدمات مورد نظر خود را انتخاب کنید:";
            var loggedKeyboard = new InlineKeyboardMarkup { InlineKeyboard = loggedKeyboardRows };

            await _baleBotService.SendMessageAsync(chatId, welcomeText, loggedKeyboard);
        }

        /// <summary>
        /// بخش ثبت‌نام کاربران (بدون تغییر)
        /// </summary>
        public async Task ShowUnauthenticatedMenu(long chatId, string userName)
        {
            var keyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>
        {
            new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton { Text = "👥 ثبت‌نام مدیران باشگاه", CallbackData = "REG_MANAGER" }
            },


            new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton { Text = "📜♂️ لیست باشگاه‌ها جهت ثبت‌نام مربیان و اعضاء", CallbackData = "GYM_LIST" }
            },


            new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton { Text = "🏋️‍♂️ ثبت‌نام مربیان باشگاه", CallbackData = "REG_TRAINER" }
            },

            new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton { Text = "📝♂️ ثبت‌نام اعضاء باشگاه", CallbackData = "REG_MEMBER" }
            },
            new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton { Text = minemenu, CallbackData = "MAIN_MENU" }
            },

        }
            };

            await _baleBotService.SendMessageAsync(
                chatId,
                $"به سیستم مدیریت باشگاه‌های فیتکور FitCore خوش آمدید {userName} عزیز.\nلطفاً جهت ثبت‌نام در سایت فیتکور یکی از گزینه‌های زیر را انتخاب نمائید:",
                keyboard);
        }




        string minemenu = "🏢 منوی اصلی";
        /// <summary>
        /// منوی ساده (بدون تغییر)
        /// </summary>
        public async Task ShowMainMenu(long chatId)
        {
            var keyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton>
                    {
                        new InlineKeyboardButton { Text = minemenu, CallbackData = "MAIN_MENU_2" }
                    },
                }
            };

            await _baleBotService.SendMessageAsync(chatId, minemenu, keyboard);
        }

        /// <summary>
        /// نمایش خطا (بدون تغییر)
        /// </summary>
        public async Task ShowErrorWithMenu(long chatId, string errorMessage)
        {
            var keyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton>
                    {
                        new InlineKeyboardButton { Text = "🔙 بازگشت به منوی اصلی", CallbackData = "MAIN_MENU" }
                    }
                }
            };
            await _baleBotService.SendMessageAsync(chatId, errorMessage, keyboard);
        }

        /// <summary>
        /// نظرسنجی (بدون تغییر)
        /// </summary>
        public async Task SendSurveyToBale(long chatId)
        {
            string textMessage = "نظر سنجی\nکاربر محترم، با سلام\nلطفا نظر خود را نسبت به عملکرد این نرم افزار را اعلام بفرمائید.\nبا تشکر- مدیریت سایت فیتکور\n";

            var btn1 = new InlineKeyboardButton { Text = "ضعیف", CallbackData = "SRV_Survey1" };
            var btn2 = new InlineKeyboardButton { Text = "متوسط", CallbackData = "SRV_Survey2" };
            var btn3 = new InlineKeyboardButton { Text = "خوب", CallbackData = "SRV_Survey3" };
            var btn4 = new InlineKeyboardButton { Text = "عالی", CallbackData = "SRV_Survey4" };

            var row = new List<InlineKeyboardButton> { btn1, btn2, btn3, btn4 };
            var keyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>> { row }
            };

            await _baleBotService.SendMessageAsync(chatId, textMessage, keyboard);
        }

        /// <summary>
        /// ارسال پیام به کاربران پس از ویرایش اطلاعات عضو توسط مدیر
        /// </summary>
        public async Task EditMemberInfoSend(long BaleChatId, string massege)
        {
            var loggedKeyboardRows = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton { Text = "🧑‍💻 اطلاعات کاربر", CallbackData = "MEMBER_INFO_MENU" }
                },
                new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton { Text = minemenu, CallbackData = "MAIN_MENU" }
                }
            };
            var loggedKeyboard = new InlineKeyboardMarkup { InlineKeyboard = loggedKeyboardRows };
            //var member = _db.Members
            //               .Include(x => x.AppUser)
            //               .FirstOrDefault(x => x.Id == memberId);



            await _baleBotService.SendMessageAsync(
                (long)BaleChatId,
                massege,
                loggedKeyboard);
        }
        #endregion






    }





}