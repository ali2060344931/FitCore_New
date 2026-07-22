using EndPoint.Site.Areas.Admin.Models;

using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.Auth;
using FitCore.Application.Services.TrainingProgramBuilder.Commands.AddTrainingDay;
using FitCore.Application.Services.TrainingProgramBuilder.Commands.AddTrainingExercise;
using FitCore.Application.Services.TrainingProgramBuilder.Commands.EditTrainingDay;
using FitCore.Application.Services.TrainingProgramBuilder.Commands.EditTrainingExercise;
using FitCore.Application.Services.TrainingProgramBuilder.Commands.RemoveAllTrainingDays;
using FitCore.Common;
using FitCore.Domain.Entities.TrainingProgram;

using GymBot.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TrainingProgramBuilderController : Controller
    {
        private readonly ITrainingProgramFacad _trainingProgramFacad;
        private readonly ITrainingProgramBuilderFacad _builderFacad;
        private readonly IRemoveAllTrainingDaysService _RemoveAllTrainingDaysService;
        private readonly IDataBaseContext _context;
        private readonly IBaleBotService _baleBotService;


        public TrainingProgramBuilderController(
            ITrainingProgramFacad trainingProgramFacad,
            ITrainingProgramBuilderFacad builderFacad,
            IDataBaseContext context,
            IRemoveAllTrainingDaysService removeAllTrainingDaysService,
            IBaleBotService baleBotService)
        {
            _trainingProgramFacad = trainingProgramFacad;
            _builderFacad = builderFacad;
            _context = context;
            _RemoveAllTrainingDaysService = removeAllTrainingDaysService;
            _baleBotService = baleBotService;
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

            // ← گروه‌های عضلانی برای فیلتر
            ViewBag.MuscleGroups = await _context.MuscleGroups
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();


            // ==================================================================
            // خواندن اطلاعات عضو و محاسبه سن
            // ==================================================================
            long? memberId = _context.TrainingPrograms
                .Where(p => p.Id == Id)
                .Select(p => p.MemberId)
                .FirstOrDefault();

            int? memberAge = null;

            if (memberId.HasValue && memberId.Value > 0)
            {
                vm.MemberDetails = _context.Members
                    .Include(m => m.memberBodyMeasurements)
                    .FirstOrDefault(m => m.Id == memberId.Value);

                if (vm.MemberDetails != null && !string.IsNullOrEmpty(vm.MemberDetails.BirthDate))
                {
                    try
                    {
                        vm.MemberAge = Convert.ToInt32(PersianDateCalse.GetAge(vm.MemberDetails.BirthDate, PersianDateCalse.AgeDisplayMode.Year));
                    }
                    catch { /* در صورت خطای تاریخ، سن محاسبه نمی‌شود */ }
                }
            }
            // ==================================================================

            // پیدا کردن عضو و پاس دادن آدرس عکس از طریق ViewBag
            var member = _context.Members.Where(c => c.Id == memberId).First();

            ViewBag.MemberProfileImage = member?.ProfileImageUrl;

            ViewBag.FrontImage = member?.BodyImageUrl1;
            ViewBag.SideImage = member?.BodyImageUrl2;
            ViewBag.BackImage = member?.BodyImageUrl3;







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
        // تغییر ترتیب حرکات (Drag & Drop)
        //====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReorderExercises([FromBody] List<FitCore.Application.Services.TrainingProgramBuilder.Commands.ReorderTrainingExercises.ReorderItemDto> items)
        {
            var result = await _builderFacad.ReorderTrainingExercisesService.Execute(items);
            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message
            });
        }

        //====================================================
        // Lookups
        //====================================================
        private async Task<List<SelectListItem>> GetDayTypesSelectListAsync()
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

        private async Task<List<ExerciseLookupDto>> GetExercisesSelectListAsync()
        {
            var (gymId, isAdmin) = await GetCurrentUserGymContextAsync();

            var exercisesQuery = _context.Exercises
                .Where(x => x.IsActive && !x.IsRemoved)
                .Include(x => x.PrimaryMuscleGroup)
                .Include(x => x.EquipmentType)
                .AsQueryable();

            if (!isAdmin)
            {
                exercisesQuery = exercisesQuery
                    .Where(x => x.GymId == null || x.GymId == gymId);
            }

            var exercises = await exercisesQuery
                .OrderBy(x => x.PrimaryMuscleGroup.Name)
                .ThenBy(x => x.Name)
                .Select(x => new ExerciseLookupDto
                {
                    Value = x.Id.ToString(),
                    Text = x.PrimaryMuscleGroup.Name + " - " + x.EquipmentType.Name + " - " + x.Name +
                           (x.GymId == null ? " (عمومی)" : ""),
                    MuscleGroupId = x.PrimaryMuscleGroupId,
                    IsGlobal = x.GymId == null   // ← اضافه شد
                })
                .ToListAsync();

            return exercises;
        }




        //private async Task<List<ExerciseLookupDto>> GetExercisesSelectListAsync()
        //{
        //    var (gymId, isAdmin) = await GetCurrentUserGymContextAsync();

        //    var exercisesQuery = _context.Exercises
        //        .Where(x => x.IsActive && !x.IsRemoved)
        //        .Include(x => x.PrimaryMuscleGroup)
        //        .Include(x => x.EquipmentType)
        //        .AsQueryable();

        //    //====================================
        //    // فیلتر باشگاه:
        //    // مدیر کل: همه حرکات
        //    // مدیر باشگاه: فقط حرکات باشگاه خودش + حرکات سراسری
        //    //====================================
        //    if (!isAdmin)
        //    {
        //        exercisesQuery = exercisesQuery
        //            .Where(x => x.GymId == null || x.GymId == gymId);
        //    }

        //    var exercises = await exercisesQuery
        //        .OrderBy(x => x.PrimaryMuscleGroup.Name)
        //        .ThenBy(x => x.Name)
        //        .Select(x => new ExerciseLookupDto
        //        {
        //            Value = x.Id.ToString(),
        //            Text = x.PrimaryMuscleGroup.Name + " - " + x.EquipmentType.Name + " - " + x.Name +
        //                   (x.GymId == null ? " (عمومی)" : ""),
        //            MuscleGroupId = x.PrimaryMuscleGroupId
        //        })
        //        .ToListAsync();

        //    return exercises;
        //}


        //private async Task<List<SelectListItem>> GetExercisesSelectListAsync()
        //{
        //    var (gymId, isAdmin) = await GetCurrentUserGymContextAsync();

        //    var exercisesQuery = _context.Exercises
        //        .Where(x => x.IsActive && !x.IsRemoved)
        //        .Include(x => x.PrimaryMuscleGroup)
        //        .Include(x => x.EquipmentType)

        //        .AsQueryable();

        //    //====================================
        //    // فیلتر باشگاه:
        //    // مدیر کل: همه حرکات
        //    // مدیر باشگاه: فقط حرکات باشگاه خودش + حرکات سراسری
        //    //====================================

        //    if (!isAdmin)
        //    {
        //        exercisesQuery = exercisesQuery
        //            .Where(x => x.GymId == null || x.GymId == gymId);
        //    }

        //    var exercises = await exercisesQuery
        //        .OrderBy(x => x.PrimaryMuscleGroup.Name)
        //        .ThenBy(x => x.Name)
        //        .Select(x => new SelectListItem
        //        {
        //            Value = x.Id.ToString(),
        //            Text = x.PrimaryMuscleGroup.Name + " - " + x.EquipmentType.Name + " - " + x.Name +
        //                   (x.GymId == null ? " (عمومی)" : "")
        //        })
        //        .ToListAsync();

        //    return exercises;
        //}




        //====================================================
        // تشخیص باشگاه و سطح دسترسی کاربر جاری
        //====================================================
        private async Task<(long? GymId, bool IsAdmin)> GetCurrentUserGymContextAsync()
        {
            bool isAdmin = User.IsAdmin();

            if (isAdmin)
            {
                return (null, true);
            }

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdValue))
                return (null, false);

            var appUserId = long.Parse(userIdValue);

            var gymId = await _context.Users
                .Where(x => x.Id == appUserId)
                .Select(x => x.GymId)
                .FirstOrDefaultAsync();

            return (gymId, false);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAllDays(RemoveAllTrainingDaysDto request)
        {
            var result = _RemoveAllTrainingDaysService.Execute(request);

            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendNotification(long programId)
        {
            var program = await _context.TrainingPrograms
                .Include(p => p.Member)
                    .ThenInclude(m => m.AppUser)
                .Include(p => p.TrainingProgramType)
                .Include(p => p.TrainingGoalType)
                .FirstOrDefaultAsync(p => p.Id == programId);

            if (program == null)
            {
                return Json(new { isSuccess = false, message = "برنامه تمرینی یافت نشد." });
            }

            if (program.Member?.AppUser == null || !program.Member.AppUser.BaleChatId.HasValue)
            {
                return Json(new { isSuccess = false, message = "کاربر در ربات بله ثبت نام نکرده است یا چت آیدی او یافت نشد." });
            }

            var programGymName = _context.Gyms
                .Where(c => c.Id == program.GymId)
                .Select(c => c.Name)
                .FirstOrDefault();

            string message =
                $"📋 *برنامه جدید برای شما*\n" +
                $"🏢 باشگاه: {programGymName ?? "نامشخص نیست"}\n" +
                $"🏋️ نوع برنامه: برنامه تمرینی\n" +
                $"📑 عنوان برنامه: {program.TrainingProgramType?.Name}\n" +
                $"🏃‍♂️ هدف برنامه: {program.TrainingGoalType?.Name}\n" +
                $"🏃‍♂️ تعداد: {program.SessionsPerWeek} در هفته\n" +
                $"📅 تاریخ شروع: {program.StartDate}\n" +
                $"📅 تاریخ پایان: {program.EndDate}\n" +
                $"🗓️ تاریخ ثبت: {PersianDateCalse.ToShamsi(program.InsertTime)}\n" +
                $"🔔 لطفاً برای دانلود برنامه از دکمه زیر استفاده کنید.";

            var keyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>
        {
            new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton
                {
                    Text = "📥 دریافت برنامه تمرینی",
                    CallbackData = $"DL_TRN_{program.Id}"
                }
            }
        }
            };

            await _baleBotService.SendMessageAsync(
                program.Member.AppUser.BaleChatId.Value,
                message,
                keyboard);

            return Json(new { isSuccess = true, message = "پیام همراه با دکمه دریافت برنامه تمرینی با موفقیت در ربات ارسال شد." });
        }
    }
}
