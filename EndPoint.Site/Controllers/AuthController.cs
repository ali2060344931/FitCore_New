using FitCore.Application.Services.Users.Commands.LoginUser;
using FitCore.Application.Services.Users.Commands.LogoutUser;
using FitCore.Application.Services.Users.Commands.RegisterUser;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using static FitCore.Application.Services.Users.Commands.RegisterUser.RegisterUserService;

namespace EndPoint.Site.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILoginUserService _loginUserService;
        private readonly IRegisterUserService _registerUserService;

        public AuthController(ILoginUserService loginUserService, IRegisterUserService registerUserService)
        {
            _loginUserService = loginUserService;
            _registerUserService = registerUserService;
        }




        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginUserRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _loginUserService.Execute(request);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", result.Message);

            return View(request);
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
                return View(request);
            }
            var q = new RegisterUserRequest();
            q.Email = request.Email;
            q.Password = request.Password;
            q.FullName = request.FullName;
            q.GymId = 1;
            var result = await _registerUserService.Execute(request);

            if (result.IsSuccess)
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", result.Message);

            return View(request);
        }
    }
}
