using EndPoint.Site.Models.ViewModels.AuthenticationViewModel;

using FitCore.Application.Services.Setings.Queries.GetSetings;
using FitCore.Application.Services.SiteSettings;
using FitCore.Application.Services.Users.Commands.RgegisterUser;
using FitCore.Application.Services.Users.Commands.UserLogin;
using FitCore.Common.Dto;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EndPoint.Site.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly IRegisterUserService _registerUserService;
        private readonly IUserLoginService _userLoginService;

        private readonly IGetSetings _getSetings;
        //private readonly ISiteSettingService _siteSetting;

        private readonly ISiteSettingService _siteSettingService;
        //public AuthenticationController(IRegisterUserService registerUserService, IUserLoginService userLoginService, IGetSetings getSetings, ISiteSettingService siteSetting)
        //{
        //    _registerUserService = registerUserService;
        //    _userLoginService = userLoginService;
        //    _getSetings = getSetings;
        //    _siteSetting = siteSetting;
        //}

        public AuthenticationController(
    IRegisterUserService registerUserService,
    IUserLoginService userLoginService,
    ISiteSettingService siteSettingService, IGetSetings getSetings)
        {
            _registerUserService = registerUserService;
            _userLoginService = userLoginService;
            _siteSettingService = siteSettingService;
            _getSetings = getSetings;
        }


        [HttpGet]
        public IActionResult Signup()
        {

            //var result = _getSetings.Execute();
            //var setting = result.Data.FirstOrDefault(p => p.Id == 1);

            //if (setting == null)
            //    setting = new SetingDto { TextFilde = "فروشگاه" };

            //return View(setting);

            var siteSetting = _siteSettingService.Get();
            return View(siteSetting);
        }

        [HttpPost]
        public IActionResult Signup(SignupViewModel request)
        {
            #region این بخش از کد برای ثبت نام می باشند

            if (string.IsNullOrWhiteSpace(request.FullName) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Tel) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.RePassword))
            {
                return Json(new ResultDto { IsSuccess = false, Message = "لطفا تمامی موارد رو ارسال نمایید" });
            }

            if (User.Identity.IsAuthenticated == true)
            {
                return Json(new ResultDto { IsSuccess = false, Message = "شما به حساب کاربری خود وارد شده اید! و در حال حاضر نمیتوانید ثبت نام مجدد نمایید" });
            }
            if (request.Password != request.RePassword)
            {
                return Json(new ResultDto { IsSuccess = false, Message = "رمز عبور و تکرار آن برابر نیست" });
            }
            if (request.Password.Length < 8)
            {
                return Json(new ResultDto { IsSuccess = false, Message = "رمز عبور باید حداقل 8 کاراکتر باشد" });
            }

            string emailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Z0-9.-]+\.[A-Z]{2,}$";

            var match = Regex.Match(request.Email, emailRegex, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return Json(new ResultDto { IsSuccess = true, Message = "ایمیل خودرا به درستی وارد نمایید" });
            }


            var signeupResult = _registerUserService.Execute(new RequestRegisterUserDto
            {
                Email = request.Email,
                FullName = request.FullName,
                Tel=request.Tel,
                Password = request.Password,
                RePasword = request.RePassword,
                roles = new List<RolesInRegisterUserDto>()
                {
                     new RolesInRegisterUserDto { Id = 3},
                }
            });

            #endregion


            #region این کدها جهت لاگین می باشد
            if (signeupResult.IsSuccess == true)
                {
                    var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,signeupResult.Data.UserId.ToString()),
                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.Name, request.FullName),
                new Claim(ClaimTypes.Role, "Customer"),
            };


                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties()
                    {
                        IsPersistent = true
                    };
                    HttpContext.SignInAsync(principal, properties);

                }

            #endregion

            return Json(signeupResult);
        }



        //public IActionResult Signin(string ReturnUrl = "/")
        //{
        //    ViewBag.url = ReturnUrl;

        //    var result = _getSetings.Execute();
        //    var setting = result.Data.FirstOrDefault(p => p.Id == 1);

        //    if (setting == null)
        //        setting = new SetingDto { TextFilde = "فروشگاه" };

        //    return View(setting);
        //}

        [HttpGet]
        public IActionResult Signin()
        {
            var siteSetting = _siteSettingService.Get();
            return View(siteSetting);
        }


        [HttpPost]
        public IActionResult Signin(string Email, string Password, string url = "/")
        {
            var signupResult = _userLoginService.Execute(Email, Password);
            if (signupResult.IsSuccess == true)
            {
                var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,signupResult.Data.UserId.ToString()),
                new Claim(ClaimTypes.Email, Email),
                new Claim(ClaimTypes.Name, signupResult.Data.Name),
                new Claim(ClaimTypes.Role, signupResult.Data.Roles ),
            };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.Now.AddDays(5),
                };
                HttpContext.SignInAsync(principal, properties);

            }
            return Json(signupResult);
        }




        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
