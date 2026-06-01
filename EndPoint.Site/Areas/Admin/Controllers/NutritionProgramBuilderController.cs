using EndPoint.Site.Areas.Admin.Models;

using FitCore.Application.Contexts;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.AddNutritionMealDto;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.AddNutritionMealItemDto;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.AddNutritionProgramDay;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.AutoGenerateNutritionDays;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.EditNutritionDay;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.EditNutritionMeal;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.EditNutritionMealItem;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.RemoveNutritionDay;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.RemoveNutritionMeal;
using FitCore.Application.Services.NutritionProgramBuilder.Commands.RemoveNutritionMealItem;
using FitCore.Application.Services.NutritionProgramBuilder.Queries;
using FitCore.Common;
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

         private readonly IDataBaseContext _Context;
       private readonly IGetProgramBuilderService _getProgramBuilderService;
        private readonly IAddNutritionProgramDayService _addNutritionProgramDayService;
        private readonly IAddNutritionMealService _addNutritionMealService;
        private readonly IAddNutritionMealItemService _addNutritionMealItemService;
        private readonly IGetBuilderLookupService _getBuilderLookupService;
        private readonly IEditNutritionMealItemService _editNutritionMealItemService;
        private readonly IRemoveNutritionMealItemService _removeNutritionMealItemService;
        private readonly IRemoveNutritionMealService _removeNutritionMealService;
        private readonly IEditNutritionMealService _editNutritionMealService;
        private readonly IRemoveNutritionDayService _removeNutritionDayService;
        private readonly IEditNutritionDayService _editNutritionDayService;
        private readonly IAutoGenerateNutritionDaysService _autoGenerateNutritionDaysService;


        public NutritionProgramBuilderController(
            IGetProgramBuilderService getProgramBuilderService,
            IAddNutritionProgramDayService addNutritionProgramDayService,
            IAddNutritionMealService addNutritionMealService,
            IAddNutritionMealItemService addNutritionMealItemService,
            IGetBuilderLookupService getBuilderLookupService,
            IDataBaseContext Context, 
            IRemoveNutritionMealItemService removeNutritionMealItemService,
            IEditNutritionMealItemService editNutritionMealItemService,
            IRemoveNutritionMealService removeNutritionMealService,
            IEditNutritionMealService editNutritionMealService,
            IRemoveNutritionDayService removeNutritionDayService,
            IEditNutritionDayService editNutritionDayService,
            IAutoGenerateNutritionDaysService autoGenerateNutritionDaysService
            )
        {
            _getProgramBuilderService = getProgramBuilderService;
            _addNutritionProgramDayService = addNutritionProgramDayService;
            _addNutritionMealService = addNutritionMealService;
            _addNutritionMealItemService = addNutritionMealItemService;
            _getBuilderLookupService = getBuilderLookupService;
            _Context = Context;
            _removeNutritionMealItemService = removeNutritionMealItemService;
            _editNutritionMealItemService= editNutritionMealItemService;
            _removeNutritionMealService= removeNutritionMealService;
            _editNutritionMealService= editNutritionMealService;
            _editNutritionDayService = editNutritionDayService;
            _removeNutritionDayService= removeNutritionDayService;
            _autoGenerateNutritionDaysService= autoGenerateNutritionDaysService;

        }

        //public IActionResult Index(long id)
        //{
        //    var result = _getProgramBuilderService.Execute(id);

        //    return View(result);
        //}





        public IActionResult Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            long Id = SecurityUtils.DecryptId(id);




            var builderResult = _getProgramBuilderService.Execute(Id);

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






        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMealItem(RemoveNutritionMealItemDto request)
        {
            var result = _removeNutritionMealItemService.Execute(request);

            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMealItem(EditNutritionMealItemDto request)
        {
            var result = _editNutritionMealItemService.Execute(request);
            return Json(new { isSuccess = result.IsSuccess, message = result.Message });
        }


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMeal(RemoveNutritionMealDto request)
        {
            var result = _removeNutritionMealService.Execute(request);
            return Json(new { isSuccess = result.IsSuccess, message = result.Message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMeal(EditNutritionMealDto request)
        {
            var result = _editNutritionMealService.Execute(request);
            return Json(new { isSuccess = result.IsSuccess, message = result.Message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDay(RemoveNutritionDayDto request)
        {
            var result = _removeNutritionDayService.Execute(request);
            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDay(EditNutritionDayDto request)
        {
            var result = _editNutritionDayService.Execute(request);

            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message
            });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AutoGenerateDays(AutoGenerateNutritionDaysDto request)
        {
            var result = _autoGenerateNutritionDaysService.Execute(request);

            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message
            });
        }

    }
}
