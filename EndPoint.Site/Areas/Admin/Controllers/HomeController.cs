using FitCore.Application.Contexts;
using FitCore.Common;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Security.Claims;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class HomeController : Controller
    {

        private readonly IDataBaseContext _context;

        public HomeController(IDataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string gymName = "فاقد باشگاه";
            if (!User.IsSuperAdmin())
            {
                long appUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var gid = _context.Users.Where(c => c.Id == appUserId).First().GymId;

                gymName = _context.Gyms.Where(c => c.Id == gid).First().Name;
            }

                ViewBag.GymName = gymName;

            return View();
        }
    }
}
