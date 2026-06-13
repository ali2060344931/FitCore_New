using EndPoint.Site.Areas.Admin.Models;

using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.TrainingProgramBuilder.Commands.AddTrainingDay;
using FitCore.Application.Services.TrainingProgramBuilder.Commands.AddTrainingExercise;
using FitCore.Application.Services.TrainingProgramBuilder.Commands.EditTrainingDay;
using FitCore.Application.Services.TrainingProgramBuilder.Commands.EditTrainingExercise;
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
    public class TrainingProgramBuilderController : Controller
    {
        private readonly ITrainingProgramFacad _trainingProgramFacad;
        private readonly ITrainingProgramBuilderFacad _builderFacad;
        private readonly IDataBaseContext _context;

        public TrainingProgramBuilderController(
            ITrainingProgramFacad trainingProgramFacad,
            ITrainingProgramBuilderFacad builderFacad,
            IDataBaseContext context)
        {
            _trainingProgramFacad = trainingProgramFacad;
            _builderFacad = builderFacad;
            _context = context;
        }

        //====================================================
        // صفحه برنامه‌سازی
        //====================================================
        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            long Id = SecurityUtils.DecryptId(id);

            var result = await _trainingProgramFacad.GetTrainingProgramByIdService.Execute(Id);

            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index", "TrainingProgram", new { area = "Admin" });
            }

            var vm = new TrainingProgramBuilderPageViewModel
            {
                Program = result.Data,
                DayTypes = await GetDayTypesSelectListAsync(),
                Exercises = await GetExercisesSelectListAsync()
            };

            ViewBag.EncryptedId = id;

            return View(vm);
        }

        //====================================================
        // افزودن روز تمرینی
        //====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDay(RequestAddTrainingDayDto request)
        {
            var result = await _builderFacad.AddTrainingDayService.Execute(request);
            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message,
                data = result.Data
            });
        }

        //====================================================
        // ویرایش روز تمرینی
        //====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDay(RequestEditTrainingDayDto request)
        {
            var result = await _builderFacad.EditTrainingDayService.Execute(request);
            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message
            });
        }

        //====================================================
        // حذف روز تمرینی
        //====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDay(long trainingDayId)
        {
            var result = await _builderFacad.RemoveTrainingDayService.Execute(trainingDayId);
            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message
            });
        }

        //====================================================
        // افزودن حرکت به روز
        //====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExercise(RequestAddTrainingExerciseDto request)
        {
            var result = await _builderFacad.AddTrainingExerciseService.Execute(request);
            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message,
                data = result.Data
            });
        }

        //====================================================
        // ویرایش حرکت
        //====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditExercise(RequestEditTrainingExerciseDto request)
        {
            var result = await _builderFacad.EditTrainingExerciseService.Execute(request);
            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message
            });
        }

        //====================================================
        // حذف حرکت از روز
        //====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteExercise(long trainingExerciseItemId)
        {
            var result = await _builderFacad.RemoveTrainingExerciseService.Execute(trainingExerciseItemId);
            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message
            });
        }

        //====================================================
        // Lookups
        //====================================================
        private async Task<System.Collections.Generic.List<SelectListItem>> GetDayTypesSelectListAsync()
        {
            var dayTypes = await _context.TrainingDayTypes
                .OrderBy(x => x.Id)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            return dayTypes;
        }

        private async Task<System.Collections.Generic.List<SelectListItem>> GetExercisesSelectListAsync()
        {
            var exercises = await _context.Exercises
                .Where(x => x.IsActive && !x.IsRemoved)
                .Include(x => x.PrimaryMuscleGroup)
                .OrderBy(x => x.PrimaryMuscleGroup.Name)
                .ThenBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.PrimaryMuscleGroup.Name + " - " + x.Name
                })
                .ToListAsync();

            return exercises;
        }
    }
}