using EndPoint.Site.Areas.Admin.Models;

using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.Members.Queries.ReportMembers;
using FitCore.Application.Services.NutritionProgramReports.Queries;
using FitCore.Application.Services.NutritionPrograms.Commands.AddNutritionProgram;
using FitCore.Application.Services.NutritionPrograms.Commands.DeleteNutritionProgram;
using FitCore.Application.Services.NutritionPrograms.Queries.GetNutritionProgram;
using FitCore.Common;
using FitCore.Common.Roles;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;

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
        //private readonly IReportMembersService _reportMembersService ;
        private readonly IGetNutritionProgramPdfService _pdfService;

        public NutritionProgramController(
            INutritionProgramFacad nutritionProgramFacad,
            IDataBaseContext context, IGetNutritionProgramPdfService pdfService
            /*IReportMembersService reportMembersService*/)
        {
            //_reportMembersService= reportMembersService;
            _nutritionProgramFacad = nutritionProgramFacad;
            _context = context;
            _pdfService = pdfService;
        }

        //====================================================
        // لیست برنامه های غذایی
        //====================================================

        /*
        public async Task<IActionResult> Index(
    long? userId,
    int page = 1,
    int pageSize = 20,
    string searchKey = "")
        {
            var currentUserIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(currentUserIdValue))
                return Unauthorized();

            var currentUserId = long.Parse(currentUserIdValue);

            long targetUserId;

            if (User.IsInRole(UserRoles.Admin))
            {
                targetUserId = userId ?? currentUserId;
            }
            else
            {
                targetUserId = currentUserId;
            }

            var request = new RequestGetNutritionProgramsDto
            {
                AppUserId = targetUserId,
                Page = page,
                PageSize = pageSize,
                SearchKey = searchKey
            };

            var result = await _nutritionProgramFacad
                .GetNutritionProgramsService
                .Execute(request);

            return View(result);
        }
        */
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int PageSize = 20, string SearchKey = "")
        {

            var isAdmin =User.IsAdmin() ;
            
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrWhiteSpace(userIdValue))
                return Unauthorized();

            var appUserId = long.Parse(userIdValue);

            var request = new RequestGetNutritionProgramsDto
            {
                AppUserId = appUserId,
                Page = page,
                PageSize = PageSize,
                SearchKey = SearchKey,
                IsAdmin = isAdmin
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
            ViewBag.MemberMobile = member.AppUser.PhoneNumber;

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
                //Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                IsActive = model.IsActive
            };

            var result = await _nutritionProgramFacad.AddNutritionProgramService.Execute(request);



            var programId = result.Data.Id;

            return Json(new
            {
                //isSuccess = true,
                //message = result.Message,
                //redirectUrl = Url.Action("Index", "NutritionProgramBuilder", new { id = programId })
                isSuccess = true,
                message = "برنامه غذایی با موفقیت ثبت شد",
                id = result.Data.Id, // یا ProgramId
                //redirectUrl = "/Admin/NutritionProgram"
                //redirectUrl = "/Admin/NutritionProgramBuilder"
                redirectUrl = Url.Action("Index", "NutritionProgram", new { area = "Admin" })
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
            //item.Title = model.Title;
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
                .OrderBy(x => x.Id)
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
            var result = _nutritionProgramFacad.DeleteNutritionProgramService.Execute(Id);
            return Json(result);
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

            return File(pdfBytes, "application/pdf", "NutritionProgram.pdf");
        }

    }
}
