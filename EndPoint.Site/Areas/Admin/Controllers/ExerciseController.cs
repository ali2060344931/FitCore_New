using EndPoint.Site.Areas.Admin.Models;

using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.Exercises.Commands.AddExercise;
using FitCore.Application.Services.Exercises.Commands.EditExercise;
using FitCore.Application.Services.Exercises.Queries;
using FitCore.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ExerciseController : Controller
    {
        private readonly IExerciseFacad _exerciseFacad;
        private readonly IDataBaseContext _context;
        private readonly IWebHostEnvironment _env;

        public ExerciseController(
            IExerciseFacad exerciseFacad,
            IDataBaseContext context,
            IWebHostEnvironment env)
        {
            _exerciseFacad = exerciseFacad;
            _context = context;
            _env = env;
        }

        //====================================================
        // مسیر و نام پوشه ذخیره تصاویر حرکات
        //====================================================
        private const string ExerciseImagesFolder = "uploads/exercises";

        // پسوندهای مجاز برای آپلود تصویر
        private static readonly string[] AllowedExtensions =
            { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

        // حداکثر حجم مجاز فایل (2 مگابایت)
        private const long MaxFileSizeBytes = 2 * 1024 * 1024;

        //====================================================
        // تشخیص باشگاه و سطح دسترسی کاربر جاری
        //====================================================
        private async Task<(long? GymId, bool IsAdmin)> GetCurrentUserGymContextAsync()
        {
            bool isAdmin = User.IsSuperAdmin();

            if (isAdmin)
            {
                // مدیر کل: به همه حرکات دسترسی دارد
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

        //====================================================
        // لیست حرکات
        //====================================================
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 20, string SearchKey = "")
        {
            var (gymId, isAdmin) = await GetCurrentUserGymContextAsync();

            var request = new GetExercisesRequestDto
            {
                Page = page,
                PageSize = pageSize,
                SearchKey = SearchKey,
                GymId = gymId,
                IsAdmin = isAdmin
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

            var (gymId, isAdmin) = await GetCurrentUserGymContextAsync();

            // چک‌باکس سراسری فقط برای مدیر کل (SuperAdmin/Admin) نمایش داده می‌شود
            ViewBag.IsSuperAdmin = isAdmin;
            ViewBag.CurrentGymId = gymId;

            var model = new ExerciseCreateEditViewModel
            {
                IsActive = true,
                IsGlobal = false
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

            var (gymId, isAdmin) = await GetCurrentUserGymContextAsync();

            // تعیین GymId:
            // - مدیر باشگاه (Admin): همیشه GymId خودش
            // - مدیر کل (SuperAdmin): اگر IsGlobal = true باشد → null (سراسری)
            //                         اگر IsGlobal = false باشد → GymId خودش (اگر داشته باشد)
            long? targetGymId;

            if (isAdmin)
            {
                targetGymId = model.IsGlobal ? null : gymId;
            }
            else
            {
                targetGymId = gymId;
            }

            var request = new RequestAddExerciseDto
            {
                GymId = targetGymId,
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

            var (gymId, isAdmin) = await GetCurrentUserGymContextAsync();

            if (!isAdmin && item.GymId != gymId)
            {
                return Forbid();
            }

            await FillLookupsAsync();

            ViewBag.IsSuperAdmin = isAdmin;

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
                IsActive = item.IsActive,
                IsGlobal = item.GymId == null
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

            //====================================
            // در صورت تغییر تصویر، تصویر قبلی حذف شود
            //====================================

            var existing = await _context.Exercises
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (existing == null)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "حرکت تمرینی یافت نشد"
                });
            }

            //====================================
            // بررسی دسترسی: مدیر باشگاه فقط حرکات باشگاه خودش را ویرایش می‌کند
            //====================================

            var (gymId, isAdmin) = await GetCurrentUserGymContextAsync();

            if (!isAdmin && existing.GymId != gymId)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "شما اجازه ویرایش این حرکت را ندارید"
                });
            }

            // تعیین GymId جدید بر اساس چک‌باکس سراسری (فقط مدیر کل)
            long? newGymId = existing.GymId;
            bool updateGymId = false;

            if (isAdmin)
            {
                newGymId = model.IsGlobal ? null : gymId;
                updateGymId = (newGymId != existing.GymId);
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
                IsActive = model.IsActive,
                UpdateGymId = updateGymId,
                GymId = newGymId
            };

            string previousImagePath = existing.ImagePath;

            var result = await _exerciseFacad.EditExerciseService.Execute(request);

            if (result.IsSuccess &&
                !string.IsNullOrWhiteSpace(previousImagePath) &&
                !string.Equals(previousImagePath, model.ImagePath, System.StringComparison.OrdinalIgnoreCase))
            {
                DeleteImageFile(previousImagePath);
            }

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

            var existing = await _context.Exercises
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (existing == null)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "حرکت تمرینی یافت نشد"
                });
            }

            //====================================
            // بررسی دسترسی: مدیر باشگاه فقط حرکات باشگاه خودش را حذف می‌کند
            //====================================

            var (gymId, isAdmin) = await GetCurrentUserGymContextAsync();

            if (!isAdmin && existing.GymId != gymId)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "شما اجازه حذف این حرکت را ندارید"
                });
            }

            var result = await _exerciseFacad.DeleteExerciseService.Execute(Id);

            if (result.IsSuccess)
            {
                DeleteImageFile(existing.ImagePath);
            }

            return Json(result);
        }

        //====================================================
        // آپلود تصویر حرکت (AJAX) - بازمی‌گرداند مسیر نسبی فایل
        //====================================================
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "هیچ فایلی انتخاب نشده است"
                });
            }

            //====================================
            // بررسی حجم فایل
            //====================================

            if (file.Length > MaxFileSizeBytes)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "حجم فایل نباید بیشتر از 2 مگابایت باشد"
                });
            }

            //====================================
            // بررسی پسوند فایل
            //====================================

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "فرمت فایل مجاز نیست. فرمت‌های مجاز: jpg, jpeg, png, webp, gif"
                });
            }

            try
            {
                //====================================
                // ساخت پوشه در صورت عدم وجود
                //====================================

                var uploadsRoot = Path.Combine(_env.WebRootPath, ExerciseImagesFolder);

                if (!Directory.Exists(uploadsRoot))
                {
                    Directory.CreateDirectory(uploadsRoot);
                }

                //====================================
                // تولید نام یکتا برای فایل
                //====================================

                var fileName = $"{Guid.NewGuid():N}{extension}";
                var filePath = Path.Combine(uploadsRoot, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                //====================================
                // مسیر نسبی برای ذخیره در دیتابیس / نمایش
                //====================================

                var relativePath = $"/{ExerciseImagesFolder}/{fileName}";

                return Json(new
                {
                    isSuccess = true,
                    message = "تصویر با موفقیت آپلود شد",
                    imagePath = relativePath
                });
            }
            catch
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "خطا در ذخیره فایل تصویر"
                });
            }
        }

        //====================================================
        // حذف فایل تصویر از روی دیسک (در صورت وجود)
        //====================================================
        private void DeleteImageFile(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return;

            // فقط تصاویر آپلودشده در پوشه حرکات حذف می‌شوند
            if (!imagePath.Replace("\\", "/").Contains($"/{ExerciseImagesFolder}/"))
                return;

            var relativePath = imagePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
            var fullPath = Path.Combine(_env.WebRootPath, relativePath);

            try
            {
                if (System.IO. File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            catch
            {
                // عدم توانایی در حذف فایل فیزیکی نباید عملیات اصلی را با شکست مواجه کند
            }
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

        [HttpGet]
        public async Task<IActionResult> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Json(new { id = (long?)null });

            var (gymId, isAdmin) = await GetCurrentUserGymContextAsync();

            // جستجوی دقیق اول
            var exercise = await _context.Exercises
                .Where(x => !x.IsRemoved && x.IsActive &&
                       (x.GymId == null || x.GymId == gymId) &&
                       x.Name == name)
                .Select(x => new { x.Id, x.Name })
                .FirstOrDefaultAsync();

            // اگر پیدا نشد، جستجوی تقریبی
            if (exercise == null)
            {
                exercise = await _context.Exercises
                    .Where(x => !x.IsRemoved && x.IsActive &&
                           (x.GymId == null || x.GymId == gymId) &&
                           (x.Name.Contains(name) || name.Contains(x.Name)))
                    .Select(x => new { x.Id, x.Name })
                    .FirstOrDefaultAsync();
            }

            if (exercise == null)
                return Json(new { id = (long?)null, name = "" });

            return Json(new { id = exercise.Id, name = exercise.Name });
        }

    }
}
