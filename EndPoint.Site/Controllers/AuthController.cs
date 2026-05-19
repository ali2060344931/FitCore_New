// ===========================
// AuthController
// ===========================

using FitCore.Application.Interfaces.Contexts;
using FitCore.Application.Services.Auth;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;


namespace EndPoint.Site.Controllers
{
    public class AuthController : Controller
    {
        private readonly SendOtpService _sendOtpService;

        private readonly VerifyOtpService _verifyOtpService;

        private readonly RegisterUserService _registerUserService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RegisterUserViewModel _context;


        private readonly IDataBaseContext _db; // تغییر نام به _db برای خوانایی

        public AuthController(
            SendOtpService sendOtpService,
            VerifyOtpService verifyOtpService,
            RegisterUserService registerUserService,
            IDataBaseContext db, // تزریق دیتابیس بجای ViewModel
            SignInManager<AppUser> signInManager)
        {
            _sendOtpService = sendOtpService;
            _verifyOtpService = verifyOtpService;
            _registerUserService = registerUserService;
            _signInManager = signInManager;
            _db = db; // انتساب به دیتابیس
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendOtp(string mobile)
        {
            var result = await _sendOtpService.Execute(mobile);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(
            string mobile,
            string code)
        {
            var result = await _verifyOtpService
                .Execute(mobile, code);

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

            return RedirectToAction("Login", "Auth");
        }




    }
}