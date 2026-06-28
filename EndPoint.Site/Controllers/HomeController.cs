using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EndPoint.Site.Models;

namespace EndPoint.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            //if (User.Identity != null && User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("GettingStarted", "Home", new { area = "Admin" });
            //}

            //return View(); // Landing Page
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    //return RedirectToAction("GettingStarted", "Home", new { area = "Admin" }); // View A
                    return RedirectToAction("index", "Home", new { area = "Admin" }); // View A
                }

                if (User.IsInRole("Member"))
                {
                    return RedirectToAction("Index", "MemberDashboard", new { area = "Admin" }); // View B
                }
            }

            return View(); // Landing Page

        }   

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
