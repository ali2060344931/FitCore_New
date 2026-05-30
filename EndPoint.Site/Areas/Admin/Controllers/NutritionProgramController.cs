using Azure.Core;

using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.Facads;
using FitCore.Application.Services.Member.Queries;
using FitCore.Application.Services.NutritionPrograms.Commands.AddNutritionProgram;
using FitCore.Application.Services.NutritionPrograms.Queries.GetNutritionProgram;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public NutritionProgramController(INutritionProgramFacad nutritionProgramFacad, IDataBaseContext context)
        {
            _nutritionProgramFacad = nutritionProgramFacad;
            _context = context;
        }



        //====================================================
        // لیست برنامه های غذایی
        //====================================================

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string SearchKey = "")
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdValue))
            {
                return Unauthorized();
            }

            var appUserId = long.Parse(userIdValue);

            var request = new RequestGetNutritionProgramsDto
            {
                AppUserId = appUserId,
                Page = page,
                PageSize = 10,
                SearchKey = SearchKey
            };

            var result = await _nutritionProgramFacad.GetNutritionProgramsService.Execute(request);

            return View(result);
        }


        // GET: NutritionProgramController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        /*
        [HttpPost]
        public async Task<IActionResult> Create(RequestAddNewMemberDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            request.AppUserId = long.Parse(userId);
            var result = await _memberFacad.AddNewMemberService.Execute(request);
            return Json(result);
        }
         */




        [HttpGet]
        public async Task<IActionResult> Create()
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


            return View(new RequestAddNutritionProgramDto()
            {
                IsActive = true
            });
        }




        // POST: NutritionProgramController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RequestAddNutritionProgramDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            request.CreatedByUserId = long.Parse(userId);

            var result = await _nutritionProgramFacad.AddNutritionProgramService.Execute(request);
            return Json(result);
        }



        // GET: NutritionProgramController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NutritionProgramController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NutritionProgramController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NutritionProgramController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
