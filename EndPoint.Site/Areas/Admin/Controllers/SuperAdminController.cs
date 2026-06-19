using FitCore.Application.Services.Tickets.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")] // فقط مدیر سایت می‌تواند به این صفحه دسترسی کند
    public class SuperAdminController : Controller
    {
        private readonly IGetAllTicketsService _getAllTicketsService;
        public SuperAdminController(IGetAllTicketsService getAllTicketsService)
        {
            _getAllTicketsService = getAllTicketsService;
        }

        // این اکشن لیست تمام تیکت‌ها را به ویو می‌فرستد
        public IActionResult Tickets()
        {
            var result = _getAllTicketsService.Execute();
            return View(result.Data);
        }
    }
}