using EndPoint.Site.Areas.Admin.Models;

using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.NutritionProgramReports.Queries;
using FitCore.Application.Services.TrainingProgramReports.Queries;
using FitCore.Application.Services.TrainingPrograms.Commands.AddTrainingProgram;
using FitCore.Application.Services.TrainingPrograms.Commands.EditTrainingProgram;
using FitCore.Application.Services.TrainingPrograms.Queries.GetTrainingPrograms;
using FitCore.Common;
using FitCore.Common.Roles;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TrainingProgramController : Controller
    {
        private readonly ITrainingProgramFacad _trainingProgramFacad;
        private readonly IDataBaseContext _context;
        private readonly IGetTrainingProgramPdfService _pdfService;

        public TrainingProgramController(
            ITrainingProgramFacad trainingProgramFacad,
            IDataBaseContext context,
            IGetTrainingProgramPdfService pdfService)
        {
            _trainingProgramFacad = trainingProgramFacad;
            _context = context;
            _pdfService = pdfService;
        }

        //====================================================
        // لیست برنامه های تمرینی
        //====================================================
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int PageSize = 20, string SearchKey = "")
        {
            // ✅ استفاده از IsInRole به جای IsAdmin
            // چون IsAdmin برای مدیر باشگاه هم true برمی‌گرداند، باید نقش دقیق را چک کنیم
            // (نام "SuperAdmin" را مطابق با نام نقش مدیر کل در دیتابیس خودتان قرار دهید)
            bool isAdmin = User.IsAdmin();
            bool isTrainer = User.IsTrainer();

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdValue))
                return Unauthorized();

            var appUserId = long.Parse(userIdValue);

            long? gymId = null;

            // ✅ فقط اگر کاربر مدیر کل نبود، شناسه باشگاه او را استخراج کن
            if (isAdmin || isTrainer)
            {
                gymId = await _context.Users
                    .Where(x => x.Id == appUserId)
                    .Select(x => x.GymId)
                    .FirstOrDefaultAsync();
            }

            var request = new GetTrainingProgramsRequestDto
            {
                AppUserId = appUserId,
                Page = page,
                PageSize = PageSize,
                SearchKey = SearchKey,
                IsAdmin = isAdmin,
                IsTriner = isTrainer,
                GymId = gymId
            };

            var result = await _trainingProgramFacad.GetTrainingProgramsService.Execute(request);

            return View(result.Data);
        }
        //====================================================
        // Create - GET
        //====================================================
        [HttpGet]
        public async Task<IActionResult> Create(string id = "")
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            long userId = SecurityUtils.DecryptId(id);

            var member = await _context.Members
                .Include(m => m.AppUser)
                .FirstOrDefaultAsync(m => m.AppUserId == userId);

            if (member == null)
                return NotFound();

            await FillLookupsAsync();

            ViewBag.MemberName = member.AppUser.FullName;
            ViewBag.MemberMobile = member.AppUser.PhoneNumber;

            var model = new TrainingProgramCreateEditViewModel
            {
                MemberId = member.Id,
                IsActive = true
            };

            return View("CreateEdit", model);
        }

        //====================================================
        // Create - POST
        //====================================================
        [HttpPost]
        public async Task<IActionResult> Create(TrainingProgramCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "اطلاعات ورودی معتبر نیست."
                });
            }

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdValue))
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "کاربر نامعتبر است."
                });
            }

            var createdByUserId = long.Parse(userIdValue);

            var gymId = await _context.Users
                .Where(x => x.Id == createdByUserId)
                .Select(x => x.GymId)
                .FirstOrDefaultAsync();

            var request = new RequestAddTrainingProgramDto
            {
                GymId = gymId,
                MemberId = model.MemberId,
                CreatedByUserId = createdByUserId,
                Title = model.Title,
                TrainingProgramTypeId = model.TrainingProgramTypeId,
                TrainingGoalTypeId = model.TrainingGoalTypeId,
                SessionsPerWeek = model.SessionsPerWeek,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                IsActive = model.IsActive
            };

            var result = await _trainingProgramFacad.AddTrainingProgramService.Execute(request);

            if (!result.IsSuccess)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = result.Message
                });
            }

            return Json(new
            {
                isSuccess = true,
                message = "برنامه تمرینی با موفقیت ثبت شد",
                id = result.Data.Id,
                redirectUrl = Url.Action("Index", "TrainingProgramBuilder", new
                {
                    area = "Admin",
                    id = SecurityUtils.EncryptId(result.Data.Id)
                })
            });
        }

        //====================================================
        // Edit - GET
        //====================================================
        [HttpGet]
        public async Task<IActionResult> Edit(string id = "")
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            long Id = SecurityUtils.DecryptId(id);

            var item = await _context.TrainingPrograms
                .Include(x => x.Member)
                .ThenInclude(x => x.AppUser)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (item == null)
                return NotFound();

            await FillLookupsAsync();

            ViewBag.MemberName = item.Member?.AppUser?.FullName ?? "-";

            var model = new TrainingProgramCreateEditViewModel
            {
                Id = item.Id,
                MemberId = item.MemberId,
                Title = item.Title,
                TrainingProgramTypeId = item.TrainingProgramTypeId,
                TrainingGoalTypeId = item.TrainingGoalTypeId,
                SessionsPerWeek = item.SessionsPerWeek,
                Description = item.Description,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                IsActive = item.IsActive
            };

            return View("CreateEdit", model);
        }

        //====================================================
        // Edit - POST
        //====================================================
        [HttpPost]
        public async Task<IActionResult> Edit(TrainingProgramCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "اطلاعات ورودی معتبر نیست."
                });
            }

            var request = new RequestEditTrainingProgramDto
            {
                Id = model.Id,
                Title = model.Title,
                TrainingProgramTypeId = model.TrainingProgramTypeId,
                TrainingGoalTypeId = model.TrainingGoalTypeId,
                SessionsPerWeek = model.SessionsPerWeek,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                IsActive = model.IsActive
            };

            var result = await _trainingProgramFacad.EditTrainingProgramService.Execute(request);

            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message
            });
        }

        //====================================================
        // Delete
        //====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            long Id = SecurityUtils.DecryptId(id);
            var result = await _trainingProgramFacad.DeleteTrainingProgramService.Execute(Id);
            return Json(result);
        }

        //====================================================
        // Lookups
        //====================================================
        private async Task FillLookupsAsync()
        {
            var programTypes = await _context.TrainingProgramTypes
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            programTypes.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "انتخاب کنید"
            });

            ViewBag.TrainingProgramTypes = programTypes;

            var goalTypes = await _context.TrainingGoalTypes
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            goalTypes.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "انتخاب کنید"
            });

            ViewBag.TrainingGoalTypes = goalTypes;
        }







        [HttpGet]
        public IActionResult PrintProgram(string id, [FromServices] IGetNutritionProgramPdfService service)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            long Id = SecurityUtils.DecryptId(id);


            //var result = service.Execute(Id);

            var pdfBytes = _pdfService.Execute(Id);
            //if (!result.IsSuccess)
            //    return NotFound();

            return File(pdfBytes, "application/pdf", "TrainingProgram.pdf");
        }

    }
}
