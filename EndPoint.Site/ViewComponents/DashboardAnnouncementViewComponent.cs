using FitCore.Application.Services.Announcements.Dashboard;
using FitCore.Application.Services.Announcements.Dashboard.GetDashboardAnnouncements;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.ViewComponents
{
    public class DashboardAnnouncementViewComponent : ViewComponent
    {
        private readonly IGetDashboardAnnouncementsService _service;

        public DashboardAnnouncementViewComponent(
            IGetDashboardAnnouncementsService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity.IsAuthenticated)
                return Content("");

            var userIdClaim =
                UserClaimsPrincipal.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
                return Content("");

            long userId = long.Parse(userIdClaim);

            long? gymId = null;

            var gymClaim =
                UserClaimsPrincipal.FindFirst("GymId");

            if (gymClaim != null)
                gymId = long.Parse(gymClaim.Value);

            // فعلاً برای تست
            var roleIds = new List<long>();

            var result =
                await _service.Execute(
                    new RequestGetDashboardAnnouncementsDto
                    {
                        UserId = userId,
                        GymId = gymId,
                        RoleIds = roleIds
                    });

            if (!result.IsSuccess || result.Data == null)
                return Content("");

            return View(result.Data);
        }
    }
}