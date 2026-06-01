using EndPoint.Site.Areas.Admin.Models;

using FitCore.Application.Contexts;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.AddNutritionMealDto;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.AddNutritionMealItemDto;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.AddNutritionProgramDay;
using FitCore.Application.Services.NutritionProgramBuilder.Queries;
using FitCore.Domain.Entities.NutritionProgram.NutritionMeal;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

using static EndPoint.Site.Areas.Admin.Models.NutritionProgramBuilderPageViewModel;

namespace EndPoint.Site.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize]

    public class NutritionProgramBuilderController : Controller
    {
        private readonly IGetProgramBuilderService _getProgramBuilderService;
        private readonly IAddNutritionProgramDayService _addNutritionProgramDayService;
        private readonly IAddNutritionMealService _addNutritionMealService;
        private readonly IAddNutritionMealItemService _addNutritionMealItemService;
        private readonly IGetBuilderLookupService _getBuilderLookupService;
        private readonly IDataBaseContext _Context;
        public NutritionProgramBuilderController(
            IGetProgramBuilderService getProgramBuilderService,
            IAddNutritionProgramDayService addNutritionProgramDayService,
            IAddNutritionMealService addNutritionMealService,
            IAddNutritionMealItemService addNutritionMealItemService,
            IGetBuilderLookupService getBuilderLookupService,
            IDataBaseContext Context
            )
        {
            _getProgramBuilderService = getProgramBuilderService;
            _addNutritionProgramDayService = addNutritionProgramDayService;
            _addNutritionMealService = addNutritionMealService;
            _addNutritionMealItemService = addNutritionMealItemService;
            _getBuilderLookupService = getBuilderLookupService;
            _Context = Context;
        }

        //public IActionResult Index(long id)
        //{
        //    var result = _getProgramBuilderService.Execute(id);

        //    return View(result);
        //}





        public IActionResult Index(long id)
        {
            var builderResult = _getProgramBuilderService.Execute(id);

            if (!builderResult.IsSuccess || builderResult.Data == null)
            {
                TempData["ErrorMessage"] = builderResult.Message;
                return RedirectToAction("Index", "NutritionProgram", new { area = "Admin" });
            }

            var lookupResult = _getBuilderLookupService.Execute();

            var vm = new NutritionProgramBuilderPageViewModel
            {
                Program = builderResult.Data,
                Lookups = lookupResult.Data ?? new BuilderLookupDto()
            };

            return View(vm);
        }



        [HttpPost]
        public JsonResult AddDay(RequestAddNutritionProgramDayDto request)
        {
            var result = _addNutritionProgramDayService.Execute(request);

            return Json(result);
        }




        //[HttpPost]

        //public JsonResult AddDay(AddNutritionProgramDayDto request)
        //{
        //    var result = _addNutritionProgramDayService.Execute(request);
        //    return Json(new
        //    {
        //        isSuccess = result.IsSuccess,
        //        message = result.Message,
        //        data = result.Data
        //    });
        //}




        
        [HttpPost]
        public JsonResult AddMeal(AddNutritionMealDto request)
        {
            var result = _addNutritionMealService.Execute(request);
            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message,
                data = result.Data
            });
        }

        

        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMeal(AddMealVm model)
        {

            //if (model.NutritionProgramDayId <= 0)
            //    return Json(new { isSuccess = false, message = "شناسه روز برنامه غذایی ارسال نشده است." });

            var ProgramDayId= _Context.NutritionProgramDays.Where(c=>c.Id==model.MealTypeId).FirstOrDefault().NutritionProgramId;

            if (model.MealTypeId <= 0)
                return Json(new { isSuccess = false, message = "نوع وعده را انتخاب کنید." });

            //// اگر می‌خوای مطمئن شی Day وجود دارد:
            //var dayExists = await _Context.NutritionProgramDays
            //    .AnyAsync(x => x.Id == model.NutritionProgramDayId);

            //if (!dayExists)
            //    return Json(new { isSuccess = false, message = "روز انتخاب شده معتبر نیست." });


            var meal = new NutritionMeal
            {
                NutritionProgramDayId = ProgramDayId,
                MealTypeId = model.MealTypeId,
                Title = model.Title,
                Description = model.Description,
                MealTime = model.MealTime,
                SortOrder = model.SortOrder
            };

            _Context.NutritionMeals.Add(meal);
            await _Context.SaveChangesAsync();

            return Json(new { isSuccess = true });
        }
        */





        [HttpPost]
        public JsonResult AddMealItem(AddNutritionMealItemDto request)
        {
            var result = _addNutritionMealItemService.Execute(request);
            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message,
                data = result.Data
            });
        }
    }
}
