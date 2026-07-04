using EndPoint.Site.Areas.Admin.Models;
using EndPoint.Site.BaleBot.Services;

using FitCore.Application.Contexts;
using FitCore.Application.Services.Auth;
using FitCore.Application.Services.Foods.Queries;
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
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.NutritionProgram.NutritionMeal;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Globalization;
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
        private readonly IRemoveNutritionAllDayService _removeNutritionAllDayService;
        private readonly IFoodService _foodService;
        private readonly IBaleBotService _baleBotService;
        private readonly IBaleMenuService _menuService;


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
            IAutoGenerateNutritionDaysService autoGenerateNutritionDaysService,
            IFoodService foodService,
            IRemoveNutritionAllDayService removeNutritionAllDayService,
            IBaleBotService baleBotService,
            IBaleMenuService menuService
            )
        {
            _getProgramBuilderService = getProgramBuilderService;
            _addNutritionProgramDayService = addNutritionProgramDayService;
            _addNutritionMealService = addNutritionMealService;
            _addNutritionMealItemService = addNutritionMealItemService;
            _getBuilderLookupService = getBuilderLookupService;
            _Context = Context;
            _removeNutritionMealItemService = removeNutritionMealItemService;
            _editNutritionMealItemService = editNutritionMealItemService;
            _removeNutritionMealService = removeNutritionMealService;
            _editNutritionMealService = editNutritionMealService;
            _editNutritionDayService = editNutritionDayService;
            _removeNutritionDayService = removeNutritionDayService;
            _autoGenerateNutritionDaysService = autoGenerateNutritionDaysService;
            _removeNutritionAllDayService = removeNutritionAllDayService;
            _foodService = foodService;
            _baleBotService = baleBotService;
            _menuService = menuService;

        }
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


            // ==================================================================
            // خواندن اطلاعات عضو و محاسبه سن
            // ==================================================================
            long? memberId = _Context.NutritionPrograms
                .Where(p => p.Id == Id)
                .Select(p => p.MemberId)
                .FirstOrDefault();

            int? memberAge = null;

            if (memberId.HasValue && memberId.Value > 0)
            {
                vm.MemberDetails = _Context.Members
                    .Include(m => m.memberBodyMeasurements)
                    .FirstOrDefault(m => m.Id == memberId.Value);

                // محاسبه سن دقیق با در نظر گرفتن سال کبیسه
                if (vm.MemberDetails != null && !string.IsNullOrEmpty(vm.MemberDetails.BirthDate))
                {
                    try
                    {
                        //string[] parts = vm.MemberDetails.BirthDate.Split(new[] { '/', '-' });
                        //if (parts.Length >= 3)
                        //{
                        //    int y = int.Parse(parts[0]);
                        //    int m = int.Parse(parts[1]);
                        //    int d = int.Parse(parts[2]);

                        //    PersianCalendar pc = new PersianCalendar();
                        //    var birthDate = pc.ToDateTime(y, m, d,0,0,0,0);

                        //    int years = DateTime.Today.Year - birthDate.Year;
                        //    if (birthDate.Date > DateTime.Today) years--;

                        //    int days = (DateTime.Today - birthDate.AddYears(years)).Days;

                        //    if (years >= 0)
                        //    {
                        //        vm.MemberAge = years;
                        //    }
                        //}

                        vm.MemberAge = Convert.ToInt32(PersianDateCalse.GetAge(vm.MemberDetails.BirthDate, PersianDateCalse.AgeDisplayMode.Year));
                    }
                    catch { /* در صورت خطای تاریخ، سن محاسبه نمی‌شود */ }
                }
            }
            // ==================================================================

            return View(vm);
        }
        [HttpPost]
        public JsonResult AddDay(RequestAddNutritionProgramDayDto request)
        {
            var result = _addNutritionProgramDayService.Execute(request);

            return Json(result);
        }

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

        [HttpGet]
        public IActionResult GetFoodDefaultUnit(long foodId)
        {
            // اینجا از سرویس خودت استفاده کن که اطلاعات غذا را می‌گیرد
            var unitId = _foodService.GetDefaultUnitId(foodId);
            return Json(new { unitId = unitId });
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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAllDays(RemoveAllNutritionDaysDto request)
        {
            var result = _removeNutritionAllDayService.Execute(request);

            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message
            });
        }


        //--------------------------
        /// <summary>
        /// ارسال پیام برنامه غدایی جدید به ربات بله کاربران
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendNotification(long programId)
        {
            var program = await _Context.NutritionPrograms
                .Include(p => p.Member)


                    .ThenInclude(m => m.AppUser)
                    .Include(p => p.GoalType)
                    .Include(p => p.ProgramType)
                .FirstOrDefaultAsync(p => p.Id == programId);

            var programGymName = _Context.Gyms.Where(c => c.Id == program.GymId).First().Name;

            if (program?.Member?.AppUser != null && program.Member.AppUser.BaleChatId.HasValue)
            {
                string message = $"📋 *برنامه جدید برای شما*\n" +
                    $"🏢 باشگاه: {programGymName ?? "نامشخص نیست"}\n" +
                                 $"🍔 نوع برنامه: برنامه غذایی\n" +
                                 $"⏰ عنوان برنامه:" + program.ProgramType.Name + '\n' +
                                 $"🚴‍♂️ موضوع برنامه: " + program.GoalType.Name + '\n' +
                                 $"📅 تاریخ شروع: " + program.StartDate + '\n' +
                                 $"📅 تاریخ پایان: " + program.EndDate + '\n' +


                                 $"🗓️ تاریخ ثبت: " + PersianDateCalse.ToShamsi(program.InsertTime) + '\n' +
                                 $"🔔 لطفاً وارد ربات بله شوید و از بخش «دریافت لیست برنامه‌های من» برنامه خود را دانلود کنید.";

                await _baleBotService.SendMessageAsync(program.Member.AppUser.BaleChatId.Value, message);

                await _menuService.ShowMainMenu(program.Member.AppUser.BaleChatId.Value, "-");
                return Json(new { isSuccess = true, message = "پیام با موفقیت در ربات ارسال شد." });
            }

            return Json(new { isSuccess = false, message = "کاربر در ربات بله ثبت نام نکرده است یا چت آیدی او یافت نشد." });
        }


        //--------------------------


        [HttpGet]
        public IActionResult GetFoodAllowedUnits(long foodId)
        {
            var units = _foodService.GetAllowedUnitsForFood(foodId);
            return Json(units);
        }


    }
}