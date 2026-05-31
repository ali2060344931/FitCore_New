using EndPoint.Site.Areas.Admin.Models;

using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.NutritionPrograms.Commands.AddNutritionProgram;
using FitCore.Application.Services.NutritionPrograms.Commands.DeleteNutritionProgram;
using FitCore.Application.Services.NutritionPrograms.Queries.GetNutritionProgram;
using FitCore.Common;

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
    public class NutritionProgramController : Controller
    {
        private readonly INutritionProgramFacad _nutritionProgramFacad;
        private readonly IDataBaseContext _context;
        //private readonly IDeleteNutritionProgramService _deleteNutritionProgramService ;


        public NutritionProgramController(
            INutritionProgramFacad nutritionProgramFacad,
            IDataBaseContext context)
        {
            _nutritionProgramFacad = nutritionProgramFacad;
            _context = context;
        }

        //====================================================
        // لیست برنامه های غذایی
        //====================================================
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1,int PageSize=20, string SearchKey = "")
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdValue))
                return Unauthorized();

            var appUserId = long.Parse(userIdValue);

            var request = new RequestGetNutritionProgramsDto
            {
                AppUserId = appUserId,
                Page = page,
                PageSize = PageSize,
                SearchKey = SearchKey
            };

            var result = await _nutritionProgramFacad.GetNutritionProgramsService.Execute(request);
            return View(result);
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

            var model = new NutritionProgramCreateEditViewModel
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
        public async Task<IActionResult> Create(NutritionProgramCreateEditViewModel model)
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

            if (gymId == null)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "باشگاه کاربر یافت نشد."
                });
            }

            var request = new RequestAddNutritionProgramDto
            {
                GymId = gymId.Value,
                MemberId = model.MemberId,
                CreatedByUserId = createdByUserId,
                ProgramTypeId = model.ProgramTypeId,
                GoalTypeId = model.GoalTypeId,
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                IsActive = model.IsActive
            };

            var result = await _nutritionProgramFacad.AddNutritionProgramService.Execute(request);
            return Json(result);
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


            var item = await _context.NutritionPrograms
                .Include(x => x.Member)
                .ThenInclude(x => x.AppUser)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (item == null)
                return NotFound();

            await FillLookupsAsync();

            ViewBag.MemberName = item.Member?.AppUser?.FullName ?? "-";

            var model = new NutritionProgramCreateEditViewModel
            {
                Id = item.Id,
                MemberId = item.MemberId,
                ProgramTypeId = item.ProgramTypeId,
                GoalTypeId = item.GoalTypeId,
                Title = item.Title,
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
        public async Task<IActionResult> Edit(NutritionProgramCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "اطلاعات ورودی معتبر نیست."
                });
            }

            var item = await _context.NutritionPrograms
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (item == null)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "رکورد مورد نظر یافت نشد."
                });
            }

            item.ProgramTypeId = model.ProgramTypeId;
            item.GoalTypeId = model.GoalTypeId;
            item.Title = model.Title;
            item.Description = model.Description;
            item.StartDate = model.StartDate;
            item.EndDate = model.EndDate;
            item.IsActive = model.IsActive;

            await _context.SaveChangesAsync();

            return Json(new
            {
                isSuccess = true,
                message = "برنامه غذایی با موفقیت ویرایش شد."
            });
        }

        private async Task FillLookupsAsync()
        {
            var programTypes = await _context.NutritionProgramTypes
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

            ViewBag.ProgramTypes = programTypes;

            var goalTypes = await _context.GetGoalTypes
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

            ViewBag.GetGoalTypes = goalTypes;
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            long Id = SecurityUtils.DecryptId(id);

            return Json(_nutritionProgramFacad.DeleteNutritionProgramService.Execute(Id));
        }

    }
}
