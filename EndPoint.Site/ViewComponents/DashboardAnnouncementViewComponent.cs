using FitCore.Application.Contexts;
using FitCore.Application.Services.Announcements.Dashboard;
using FitCore.Application.Services.Announcements.Dashboard.GetDashboardAnnouncements;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.ViewComponents
{
    public class DashboardAnnouncementViewComponent : ViewComponent
    {
        private readonly IGetDashboardAnnouncementsService _service;
        private readonly UserManager<AppUser> _userManager;
        private readonly IDataBaseContext _context;
        public DashboardAnnouncementViewComponent(
            IGetDashboardAnnouncementsService service, UserManager<AppUser> userManager, IDataBaseContext context)
        {
            _service = service;
            _userManager = userManager;
            _context = context;

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

            var roleIds =
                _context.UserRoles
                    .Where(x => x.UserId == userId)
                    .Select(x => x.RoleId)
                    .ToList();


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