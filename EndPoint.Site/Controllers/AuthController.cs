using FitCore.Application.Services.SiteSettings;
using FitCore.Application.Services.Users.Commands.LoginUser;
using FitCore.Application.Services.Users.Commands.LogoutUser;
using FitCore.Application.Services.Users.Commands.RegisterUser;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Linq;
using System.Threading.Tasks;

using static FitCore.Application.Services.Users.Commands.RegisterUser.RegisterUserService;

namespace EndPoint.Site.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILoginUserService _loginUserService;
        private readonly IRegisterUserService _registerUserService;
        private readonly ISiteSettingService _siteSettingService;

        public AuthController(ISiteSettingService siteSettingService, ILoginUserService loginUserService, IRegisterUserService registerUserService)
        {
            _loginUserService = loginUserService;
            _registerUserService = registerUserService;
            _siteSettingService = siteSettingService;
        }




        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginPageViewModel
            {
                Login = new LoginUserRequest(),
                SiteSetting = _siteSettingService.Get()
            };

            return View(model);
        }

        [HttpPost]

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            var result = await _loginUserService.Execute(request);

            if (result.IsSuccess)
            {
                return Json(new { isSuccess = true, message = result.Message });
            }

            return Json(new { isSuccess = false, message = result.Message });
        }


        /*
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            var model = new LoginPageViewModel
            {
                Login = request,
                SiteSetting = _siteSettingService.Get()
            };

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _loginUserService.Execute(request);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", result.Message);

            return View(model);
        }

        */

        public class LoginPageViewModel
        {
            public LoginUserRequest Login { get; set; }
            public SiteSettingDto SiteSetting { get; set; }
        }


        public async Task<IActionResult> Logout(
            [FromServices] SignInManager<AppUser> signInManager)
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterUserRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();

                return Json(new { isSuccess = false, message = errors });
            }

            request.GymId = 2;

            var result = await _registerUserService.Execute(request);

            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message
            });
        }




        /*
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var q = new RegisterUserRequest
            {
                Email = request.Email,
                Password = request.Password,
                RePassword = request.RePassword,
                FullName = request.FullName,
                GymId = 2
            };

            var result = await _registerUserService.Execute(q);

            if (result.IsSuccess)
            {
                return Json(new { isSuccess = true, message = result.Message });
            }

            return Json(new { isSuccess = false, message = result.Message });
        }
        */

    }
}
