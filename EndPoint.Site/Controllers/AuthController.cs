// ===========================
// AuthController
// ===========================

using FitCore.Application.Services.Auth;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace EndPoint.Site.Controllers
{
    public class AuthController : Controller
    {
        private readonly SendOtpService _sendOtpService;

        private readonly VerifyOtpService _verifyOtpService;

        private readonly RegisterUserService _registerUserService;
        private readonly SignInManager<AppUser> _signInManager;


        public AuthController(
            SendOtpService sendOtpService,
            VerifyOtpService verifyOtpService,
            RegisterUserService registerUserService, SignInManager<AppUser> signInManager)
        {
            _sendOtpService = sendOtpService;

            _verifyOtpService = verifyOtpService;

            _registerUserService = registerUserService;
            _signInManager = signInManager;

        }



        //public AuthController(
        //    SendOtpService sendOtpService,
        //    VerifyOtpService verifyOtpService)
        //{
        //    _sendOtpService = sendOtpService;
        //    _verifyOtpService = verifyOtpService;
        //}

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
        public IActionResult Register()
        {
            return View();
        }

        // ===========================
        // RegisterUser Action
        // ===========================

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