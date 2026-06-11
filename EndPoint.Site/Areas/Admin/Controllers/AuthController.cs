// ===========================
// AuthController
// ===========================

using FitCore.Application.Contexts;
using FitCore.Application.Services.Auth;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

using System.Linq;
using System.Threading.Tasks;


namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize] // حتما چک شود که کاربر لاگین باشد
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AuthController : Controller
    {
        private readonly SendOtpService _sendOtpService;

        private readonly VerifyOtpService _verifyOtpService;

        private readonly RegisterUserService _registerUserService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RegisterUserViewModel _context;
        private readonly UserManager<AppUser> _userManager; // اضافه شد

        private readonly IDataBaseContext _db; // تغییر نام به _db برای خوانایی

        public AuthController(
            SendOtpService sendOtpService,
            VerifyOtpService verifyOtpService,
            RegisterUserService registerUserService,
            IDataBaseContext db, // تزریق دیتابیس بجای ViewModel
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager)
        {
            _sendOtpService = sendOtpService;
            _verifyOtpService = verifyOtpService;
            _registerUserService = registerUserService;
            _signInManager = signInManager;
            _db = db; // انتساب به دیتابیس
            _userManager = userManager;
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendOtpLogin(string mobile)
        {
            var result = await _sendOtpService.Execute(mobile);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> SendOtpReg(string FullName, string mobile, int gymId)
        {
            var result = await _sendOtpService.Execute(FullName, mobile, gymId);
            return Json(result);
        }




        [HttpPost]
        public async Task<IActionResult> VerifyOtp(string mobile, string code)
        {
            var result = await _verifyOtpService.Execute(mobile, code);
            return Json(result);
        }


        [HttpPost]
        public async Task<IActionResult> CompleteLogin(CompleteLoginRequestDto request)
        {
            var result = await _verifyOtpService.CompleteLogin(request.LoginToken, request.GymId);
            return Json(result);
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


        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();

        //    return RedirectToAction("Login", "Auth");
        //}



        [HttpPost]
        public async Task<IActionResult> LoginWithPassword(string mobile, string password)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Mobile == mobile);

            if (user == null)
                return Json(new { isSuccess = false, message = "کاربری با این شماره یافت نشد." });

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                password,
                lockoutOnFailure: false);

            if (!result.Succeeded)
                return Json(new { isSuccess = false, message = "رمز عبور اشتباه است." });

            await _signInManager.SignInAsync(user, isPersistent: true);

            return Json(new { isSuccess = true });
        }



    }
}