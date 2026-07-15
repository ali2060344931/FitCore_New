using FitCore.Application.Contexts;
using FitCore.Application.Services.Auth;
using FitCore.Application.Services.Auth.Dto;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AuthController : Controller
    {
        private readonly SendOtpService _sendOtpService;
        private readonly VerifyOtpService _verifyOtpService;
        private readonly RegisterUserService _registerUserService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RegisterManagerService _registerManagerService; // اضافه شده
        private readonly IDataBaseContext _db;

        public AuthController(
            SendOtpService sendOtpService,
            VerifyOtpService verifyOtpService,
            RegisterUserService registerUserService,
            IDataBaseContext db,
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            RegisterManagerService registerManagerService)
        {
            _sendOtpService = sendOtpService;
            _verifyOtpService = verifyOtpService;
            _registerUserService = registerUserService;
            _signInManager = signInManager;
            _db = db;
            _userManager = userManager;
            _registerManagerService = registerManagerService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> SendOtpLogin(string mobile)
        {
            var result = await _sendOtpService.Execute(mobile);
            return Json(result); // فقط نتیجه سرویس را برمی‌گرداند
        }

        [HttpPost]
        public async Task<IActionResult> SendOtpReg(string FullName, string mobile, int gymId)
        {
            var result = await _sendOtpService.Execute(FullName, mobile, gymId);
            return Json(result); // فقط نتیجه سرویس را برمی‌گرداند
        }



        [HttpPost]
        public async Task<IActionResult> VerifyOtp(string mobile, string code)
        {
            var result = await _verifyOtpService.Execute(mobile, code);

            if (result.IsSuccess)
            {
                // ==========================================================
                // اصلاح اصلی: اگر نیاز به انتخاب باشگاه است، دقیقاً همون 
                // دیتوی مدل رو بفرست تا فرانت لیست باشگاه‌ها رو ببینه
                // ==========================================================
                //if (result.NeedGymSelection)
                //{
                //    return Json(result);
                //}

                // ==========================================================
                // اگر فقط یک باشگاه بود، لاگین انجام شده و مسیر را مشخص کن
                // ==========================================================
                string redirectUrl = "";

                if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
                {
                    redirectUrl = Url.Action("Index", "Home", new { area = "Admin" });
                }
                else if (User.IsInRole("Member"))
                {
                    redirectUrl = Url.Action("Index", "MemberDashboard", new { area = "Admin" });
                }

                // در صورت نبود نقش مشخص، مسیر پیش‌فرض
                if (string.IsNullOrEmpty(redirectUrl))
                {
                    redirectUrl = Url.Action("Index", "Home", new { area = "Admin" });
                }

                return Json(new
                {
                    isSuccess = true,
                    redirectUrl = redirectUrl
                });
            }

            return Json(result);
        }



        [HttpPost]
        public async Task<IActionResult> CompleteLogin(CompleteLoginRequestDto request)
        {
            var result = await _verifyOtpService.CompleteLogin(request.LoginToken, request.GymId);

            if (result.IsSuccess)
            {
                // تعیین مسیر هدایت بر اساس نقش کاربر لاگین شده
                string redirectUrl = Url.Action("Index", "Home", new { area = "Admin" }); // پیش‌فرض

                if (User.IsInRole("Member"))
                {
                    redirectUrl = Url.Action("Index", "MemberDashboard", new { area = "Admin" });
                }
                else if (User.IsInRole("SuperAdmin") || User.IsInRole("Admin"))
                {
                    redirectUrl = Url.Action("Index", "Home", new { area = "Admin" });
                }

                return Json(new
                {
                    isSuccess = true,
                    redirectUrl = redirectUrl
                });
            }

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var gyms = await _db.Gyms.Where(g => g.IsActive).Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name }).ToListAsync();

            // لود کردن استان‌ها
            var provinces = await _db.Provinces.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToListAsync();

            var model = new RegisterUserViewModel
            {
                Gyms = gyms,
                Provinces = provinces // اضافه شده
            };

            return View(model);
        }

        // اکشن جدید برای گرفتن لیست شهرها بر اساس آیدی استان (برای Dropdown وابسته)
        [HttpGet]
        public async Task<IActionResult> GetCities(int provinceId)
        {
            var cities = await _db.Cities
                .Where(c => c.ProvincesId == provinceId)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();
            return Json(cities);
        }

        // اکشن جدید برای ارسال کد پیامک به مدیر
        [HttpPost]
        public async Task<IActionResult> SendOtpForManager(string FullName, string GymName, string mobile, int provinceId, int cityId)
        {
            // فقط شماره موبایل برای ارسال پیامک مهم است، بقیه اطلاعات بعد از تایید ثبت می‌شوند
            var result = await _sendOtpService.Execute(mobile);
            return Json(result);
        }

        // اکشن جدید برای ثبت نهایی مدیر
        [HttpPost]
        public async Task<IActionResult> RegisterManager(RequestRegisterManagerDto request)
        {
            var result = await _registerManagerService.Execute(request);
            return Json(result);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            // پاک کردن دستی کوکی‌ها در صورت نیاز (اگر نام کوکی سفارشی است)
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            //return RedirectToAction("Login", "Auth");
            //return RedirectToAction("Login", "Auth", new { area = "Admin" });
            return RedirectToAction("Index", "Home", new { area = "" });

        }




    }
}