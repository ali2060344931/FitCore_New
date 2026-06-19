using FitCore.Application.Services.Tickets.DTOs;
using FitCore.Application.Services.Tickets.Interfaces;

using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

public class MemberTicketController : Controller
{
    private readonly IGetGymTicketsService _getGymTicketsService;
    private readonly ICreateTicketService _createTicketService;
    private readonly IGetTicketDetailService _getTicketDetailService;

    public MemberTicketController(IGetGymTicketsService getGymTicketsService, ICreateTicketService createTicketService, IGetTicketDetailService getTicketDetailService)
    {
        _getGymTicketsService = getGymTicketsService;
        _createTicketService= createTicketService;
        _getTicketDetailService= getTicketDetailService;
    }

    public IActionResult MyTickets()
    {
        // شناسایی آیدی کاربر جاری
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        // فقط تیکت‌های متعلق به خودش را می‌بیند (بدون نیاز به GymId)
        var result = _getGymTicketsService.Execute(userId);
        return View(result.Data);
    }

    [HttpPost]
    public IActionResult Create(CreateTicketDto request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = _createTicketService.Execute(request, userId);
        return Json(result);
    }

    public IActionResult Detail(long id)
    {
        // فقط اگر کاربر صاحب تیکت باشد یا متعلق به آن باشد، می‌تواند آن را ببیند
        var result = _getTicketDetailService.Execute(id, null);
        return View(result.Data);
    }
}