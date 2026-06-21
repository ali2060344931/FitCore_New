using FitCore.Application.Contexts;
using FitCore.Application.Services.Auth;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Security.Claims;
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
        private readonly IDataBaseContext _db;

        public AuthController(
            SendOtpService sendOtpService,
            VerifyOtpService verifyOtpService,
            RegisterUserService registerUserService,
            IDataBaseContext db,
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager)
        {
            _sendOtpService = sendOtpService;
            _verifyOtpService = verifyOtpService;
            _registerUserService = registerUserService;
            _signInManager = signInManager;
            _db = db;
            _userManager = userManager;
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
            return Json(result);
        }


        //[HttpPost]
        //public async Task<IActionResult> CompleteLogin(CompleteLoginRequestDto request)
        //{
        //    var result = await _verifyOtpService.CompleteLogin(request.LoginToken, request.GymId);
        //    return Json(result);
        //}
        [HttpPost]
        public async Task<IActionResult> CompleteLogin(CompleteLoginRequestDto request)
        {
            // ---------------------------------------------------------
            // تعیین مسیر هدایت بر اساس نقش کاربر
            // ---------------------------------------------------------
            string redirectUrl = "/"; // مسیر پیش‌فرض

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                // استفاده از await به جای .Result برای جلوگیری از Deadlock
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("Member"))
                    {
                        redirectUrl = "/Admin/MemberDashboard";
                    }
                    else if (roles.Contains("SuperAdmin") || roles.Contains("Admin"))
                    {
                        redirectUrl = "/Admin";
                    }
                }
            }
            // ---------------------------------------------------------

            return Json(new
            {
                isSuccess = true,
                redirectUrl = redirectUrl
            });
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            // حالا از _db استفاده می‌کنیم که به جدول Gyms دسترسی دارد
            var gyms = await _db.Gyms
                .Where(g => g.IsActive)
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                })
                .ToListAsync();

            var model = new RegisterUserViewModel
            {
                Gyms = gyms
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(
            RequestRegisterUserDto request)
        {
            var result =
                await _registerUserService.Execute(request);

            return Json(result);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            // پاک کردن دستی کوکی‌ها در صورت نیاز (اگر نام کوکی سفارشی است)
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Login", "Auth");
            //return RedirectToAction("Login", "Auth", new { area = "Admin" });

        }





    }
}