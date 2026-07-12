using EndPoint.Site.Areas.Admin.Models.Announcement;

using FitCore.Application.Services.Announcements.Commands.AddAnnouncement;
using FitCore.Application.Services.Announcements.Commands.EditAnnouncement;
using FitCore.Application.Services.Announcements.Dashboard.DismissAnnouncement;
using FitCore.Application.Services.Announcements.Dashboard.RegisterAnnouncementClick;
using FitCore.Application.Services.Announcements.Dashboard.RegisterAnnouncementView;
using FitCore.Application.Services.Announcements.Facade;
using FitCore.Application.Services.Announcements.Queries.GetAnnouncements;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{


    [Area("Admin")]
    //[Authorize(Roles = "Member")]
    [Authorize(Roles = "SuperAdmin,Admin,Trainer,Member")]
    public class AnnouncementController : Controller
    {

        private long CurrentUserId =>
    long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


        private readonly IAnnouncementFacade _announcementFacade;

        public AnnouncementController(
            IAnnouncementFacade announcementFacade)
        {
            _announcementFacade = announcementFacade;
        }



        public async Task<IActionResult> Index(
    string searchKey = "",
    bool? isActive = null,
    int page = 1)
        {
            var result =
                await _announcementFacade
                .GetAnnouncementsService
                .Execute(new RequestGetAnnouncementsDto()
                {
                    SearchKey = searchKey,
                    IsActive = isActive,
                    Page = page,
                    PageSize = 20
                });

            return View(result.Data);
        }



        public async Task<IActionResult> Create()
        {
            AnnouncementCreateEditViewModel vm =
                new AnnouncementCreateEditViewModel();

            await FillLookups(vm);

            return View("CreateEdit", vm);
        }

        private async Task FillLookups(
    AnnouncementCreateEditViewModel model)
        {
            var roles =
                await _announcementFacade
                .GetRolesService
                .Execute();

            model.Roles =
                roles.Data
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                })
                .ToList();

            var gyms =
                await _announcementFacade
                .GetGymsService
                .Execute();

            model.Gyms =
                gyms.Data
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                })
                .ToList();
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnnouncementCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await FillLookups(model);
                return View("CreateEdit", model);
            }

            var dto = new RequestAddAnnouncementDto
            {
                Title = model.Title,
                Message = model.Message,
                ImageUrl = model.ImageUrl,
                ButtonText = model.ButtonText,
                ButtonUrl = model.ButtonUrl,
                Type = model.Type,
                Priority = model.Priority,
                IsActive = model.IsActive,
                IsPinned = model.IsPinned,
                ShowOnce = model.ShowOnce,
                IsForAllRoles = model.IsForAllRoles,
                IsForAllGyms = model.IsForAllGyms,
                CanDismiss = model.CanDismiss,
                RepeatAfterDays = model.RepeatAfterDays,
                DisplayOrder = model.DisplayOrder,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                RoleIds = model.SelectedRoleIds,
                GymIds = model.SelectedGymIds
            };

            var result = await _announcementFacade
                .AddAnnouncementService
                .Execute(dto);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Message);

            await FillLookups(model);

            return View("CreateEdit", model);
        }




        public async Task<IActionResult> Edit(long id)
        {
            var result = await _announcementFacade
                .GetAnnouncementByIdService
                .Execute(id);

            if (!result.IsSuccess || result.Data == null)
                return NotFound();

            var vm = new AnnouncementCreateEditViewModel
            {
                Id = result.Data.Id,
                Title = result.Data.Title,
                Message = result.Data.Message,
                ImageUrl = result.Data.ImageUrl,
                ButtonText = result.Data.ButtonText,
                ButtonUrl = result.Data.ButtonUrl,
                Type = result.Data.Type,
                Priority = result.Data.Priority,
                IsActive = result.Data.IsActive,
                IsPinned = result.Data.IsPinned,
                ShowOnce = result.Data.ShowOnce,
                IsForAllRoles = result.Data.IsForAllRoles,
                IsForAllGyms = result.Data.IsForAllGyms,
                CanDismiss = result.Data.CanDismiss,
                RepeatAfterDays = result.Data.RepeatAfterDays,
                DisplayOrder = result.Data.DisplayOrder,
                StartDate = result.Data.StartDate,
                EndDate = result.Data.EndDate,
                SelectedRoleIds = result.Data.RoleIds,
                SelectedGymIds = result.Data.GymIds
            };

            await FillLookups(vm);

            return View("CreateEdit", vm);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AnnouncementCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await FillLookups(model);
                return View("CreateEdit", model);
            }

            var dto = new RequestEditAnnouncementDto
            {
                Id = model.Id,
                Title = model.Title,
                Message = model.Message,
                ImageUrl = model.ImageUrl,
                ButtonText = model.ButtonText,
                ButtonUrl = model.ButtonUrl,
                Type = model.Type,
                Priority = model.Priority,
                IsActive = model.IsActive,
                IsPinned = model.IsPinned,
                ShowOnce = model.ShowOnce,
                IsForAllRoles = model.IsForAllRoles,
                IsForAllGyms = model.IsForAllGyms,
                CanDismiss = model.CanDismiss,
                RepeatAfterDays = model.RepeatAfterDays,
                DisplayOrder = model.DisplayOrder,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                RoleIds = model.SelectedRoleIds,
                GymIds = model.SelectedGymIds
            };

            var result = await _announcementFacade
                .EditAnnouncementService
                .Execute(dto);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Message);

            await FillLookups(model);

            return View("CreateEdit", model);
        }





        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _announcementFacade
                .DeleteAnnouncementService
                .Execute(id);

            return Json(result);
        }


        public IActionResult Details(long id)
        {
            return RedirectToAction(nameof(Edit), new { id });
        }


        private async Task<string> SaveImage(
    IFormFile file)
        {
            if (file == null)
                return "";

            // مرحله بعد تکمیل می‌شود

            return "";
        }


        public async Task<IActionResult> Click(long id)
        {
            await _announcementFacade
                .RegisterAnnouncementClickService
                .Execute(new RequestRegisterAnnouncementClickDto
                {
                    AnnouncementId = id,
                    UserId = CurrentUserId
                });

            var announcement =
                await _announcementFacade
                .GetAnnouncementByIdService
                .Execute(id);

            if (announcement.Data == null)
                return Redirect("/Admin");

            if (string.IsNullOrWhiteSpace(announcement.Data.ButtonUrl))
                return Redirect("/Admin");

            return Redirect(announcement.Data.ButtonUrl);
        }



        [HttpPost]
        public async Task<IActionResult> Dismiss(long id)
        {
            await _announcementFacade
                .DismissAnnouncementService
                .Execute(new RequestDismissAnnouncementDto
                {
                    AnnouncementId = id,
                    UserId = CurrentUserId
                });

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> RegisterView(long id)
        {
            await _announcementFacade
                .RegisterAnnouncementViewService
                .Execute(new RequestRegisterAnnouncementViewDto
                {
                    AnnouncementId = id,
                    UserId = CurrentUserId
                });

            return Ok();
        }






    }





}