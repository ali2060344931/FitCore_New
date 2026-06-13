using EndPoint.Site.Areas.Admin.Models;

using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.Exercises.Commands.AddExercise;
using FitCore.Application.Services.Exercises.Commands.EditExercise;
using FitCore.Application.Services.Exercises.Queries;
using FitCore.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ExerciseController : Controller
    {
        private readonly IExerciseFacad _exerciseFacad;
        private readonly IDataBaseContext _context;

        public ExerciseController(
            IExerciseFacad exerciseFacad,
            IDataBaseContext context)
        {
            _exerciseFacad = exerciseFacad;
            _context = context;
        }

        //====================================================
        // لیست حرکات
        //====================================================
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int PageSize = 20, string SearchKey = "")
        {
            var request = new GetExercisesRequestDto
            {
                Page = page,
                PageSize = PageSize,
                SearchKey = SearchKey
            };

            var result = await _exerciseFacad.GetExercisesService.Execute(request);

            return View(result.Data);
        }

        //====================================================
        // Create - GET
        //====================================================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await FillLookupsAsync();

            var model = new ExerciseCreateEditViewModel
            {
                IsActive = true
            };

            return View("CreateEdit", model);
        }

        //====================================================
        // Create - POST
        //====================================================
        [HttpPost]
        public async Task<IActionResult> Create(ExerciseCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "اطلاعات ورودی معتبر نیست."
                });
            }

            var request = new RequestAddExerciseDto
            {
                Name = model.Name,
                EnglishName = model.EnglishName,
                Description = model.Description,
                PrimaryMuscleGroupId = model.PrimaryMuscleGroupId,
                EquipmentTypeId = model.EquipmentTypeId,
                DifficultyLevelId = model.DifficultyLevelId,
                VideoUrl = model.VideoUrl,
                ImagePath = model.ImagePath,
                IsActive = model.IsActive
            };

            var result = await _exerciseFacad.AddExerciseService.Execute(request);

            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message,
                redirectUrl = Url.Action("Index", "Exercise", new { area = "Admin" })
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

            var item = await _context.Exercises
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (item == null)
                return NotFound();

            await FillLookupsAsync();

            var model = new ExerciseCreateEditViewModel
            {
                Id = item.Id,
                Name = item.Name,
                EnglishName = item.EnglishName,
                Description = item.Description,
                PrimaryMuscleGroupId = item.PrimaryMuscleGroupId,
                EquipmentTypeId = item.EquipmentTypeId,
                DifficultyLevelId = item.DifficultyLevelId,
                VideoUrl = item.VideoUrl,
                ImagePath = item.ImagePath,
                IsActive = item.IsActive
            };

            return View("CreateEdit", model);
        }

        //====================================================
        // Edit - POST
        //====================================================
        [HttpPost]
        public async Task<IActionResult> Edit(ExerciseCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "اطلاعات ورودی معتبر نیست."
                });
            }

            var request = new RequestEditExerciseDto
            {
                Id = model.Id,
                Name = model.Name,
                EnglishName = model.EnglishName,
                Description = model.Description,
                PrimaryMuscleGroupId = model.PrimaryMuscleGroupId,
                EquipmentTypeId = model.EquipmentTypeId,
                DifficultyLevelId = model.DifficultyLevelId,
                VideoUrl = model.VideoUrl,
                ImagePath = model.ImagePath,
                IsActive = model.IsActive
            };

            var result = await _exerciseFacad.EditExerciseService.Execute(request);

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
            var result = await _exerciseFacad.DeleteExerciseService.Execute(Id);
            return Json(result);
        }

        //====================================================
        // Lookups
        //====================================================
        private async Task FillLookupsAsync()
        {
            var muscleGroups = await _context.MuscleGroups
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            muscleGroups.Insert(0, new SelectListItem { Value = "", Text = "انتخاب کنید" });
            ViewBag.MuscleGroups = muscleGroups;

            var equipmentTypes = await _context.EquipmentTypes
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            equipmentTypes.Insert(0, new SelectListItem { Value = "", Text = "انتخاب کنید" });
            ViewBag.EquipmentTypes = equipmentTypes;

            var difficultyLevels = await _context.ExerciseDifficultyLevels
                .OrderBy(x => x.Id)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            difficultyLevels.Insert(0, new SelectListItem { Value = "", Text = "انتخاب کنید" });
            ViewBag.DifficultyLevels = difficultyLevels;
        }
    }
}