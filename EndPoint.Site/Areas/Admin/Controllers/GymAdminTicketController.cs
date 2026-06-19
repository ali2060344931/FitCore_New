using FitCore.Application.Services.Tickets.DTOs;
using FitCore.Application.Services.Tickets.Interfaces;

using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

public class GymAdminTicketController : Controller
{
    private readonly IGetGymTicketsService _getGymTicketsService;
    private readonly IGetTicketDetailService _getTicketDetailService;
    private readonly IReplyTicketService _replyTicketService;

    public GymAdminTicketController(IGetGymTicketsService getGymTicketsService, IGetTicketDetailService getTicketDetailService, IReplyTicketService replyTicketService)
    {
        _getGymTicketsService = getGymTicketsService;
        _getTicketDetailService = getTicketDetailService;
        _replyTicketService = replyTicketService;
    }

    public IActionResult Tickets()
    {
        // پیدا کردن GymId کاربر جاری (اگر نداشت باشد یعنی مدیر کل است، خطا می‌دهد)
        var gymId = long.Parse(User.FindFirstValue("GymId"));

        var result = _getGymTicketsService.Execute(gymId);
        return View(result.Data);
    }

    public IActionResult Detail(long id)
    {
        // با پاس دادن GymId، سرویس چک می‌کند که آیا این تیکت متعلق به باشگاه اوست یا خیر
        var gymId = long.Parse(User.FindFirstValue("GymId"));
        var result = _getTicketDetailService.Execute(id, gymId);

        if (!result.IsSuccess)
            return Unauthorized(); // یا نمایش پیام خطا

        return View(result.Data);
    }

    [HttpPost]
    public IActionResult Reply(ReplyTicketDto request)
    {
        var adminId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = _replyTicketService.Execute(request, adminId);
        return Json(result);
    }
}