using EndPoint.Site.Areas.Admin.Models;
using EndPoint.Site.Areas.Admin.Models.Foods;

using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.Foods.Commands.CreateFood;
using FitCore.Application.Services.Foods.Commands.EditFood;
using FitCore.Application.Services.Foods.Queries;
using FitCore.Common;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FoodController : Controller
    {
        private readonly IFoodFacad _foodFacad;
        private readonly IDataBaseContext _context;

        public FoodController(IFoodFacad foodFacad, IDataBaseContext context)
        {
            _foodFacad = foodFacad;
            _context = context;
        }

        public async Task<IActionResult> Index(string SearchKey, int? CategoryTypeId, bool? isGlobalFilter, int page = 1, int pageSize = 50)
        {
            var (gymId, isAdmin) = await GetCurrentUserGymContextAsync();

            var request = new RequestGetFoodsDto
            {
                Page = page,
                PageSize = pageSize,
                SearchKey = SearchKey,
                GymId = gymId,
                IsAdmin = isAdmin,
                CategoryTypeId = CategoryTypeId,
                IsGlobalFilter = isGlobalFilter
            };

            var result = await _foodFacad.GetFoodsService.Execute(request);

            var categoryTypes = await _context.FoodCategoryTypes
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            ViewBag.CategoryTypes = categoryTypes;
            ViewBag.SelectedCategoryTypeId = CategoryTypeId;

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = await BuildViewModelAsync();
            await FillLookupsAsync();

            var (gymId, isAdmin) = await GetCurrentUserGymContextAsync();
            ViewBag.IsSuperAdmin = isAdmin;
            ViewBag.CurrentGymId = gymId;

            var model = new FoodCreateEditViewModel
            {
                IsActive = true,
                IsGlobal = false
            };
            return View("CreateEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FoodCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await BuildViewModelAsync(model);
                return View("CreateEdit", model);
            }

            var (gymId, isAdmin) = await GetCurrentUserGymContextAsync();

            long? targetGymId;
            if (isAdmin)
            {
                targetGymId = model.IsGlobal ? null : gymId;
            }
            else
            {
                targetGymId = gymId;
            }

            var result = await _foodFacad.AddFoodService.Execute(new CreateFoodDto
            {
                GymId = targetGymId,
                Title = model.Title,
                EnglishTitle = model.EnglishTitle,
                CategoryTypeId = model.CategoryTypeId,
                CaloriesPerUnit = model.CaloriesPerUnit,
                ProteinPerUnit = model.ProteinPerUnit,
                CarbohydratePerUnit = model.CarbohydratePerUnit,
                FatPerUnit = model.FatPerUnit,
                DefaultUnitId = model.DefaultUnitId,
                IsActive = model.IsActive
            });

            // ==========================================
            // ذخیره ضرایب تبدیل در حالت ثبت جدید
            // ==========================================
            if (result.IsSuccess && model.Conversions != null)
            {
                var newlyAddedFood = await _context.Foods
                    .Where(f => f.Title == model.Title && f.GymId == targetGymId)
                    .OrderByDescending(f => f.Id)
                    .FirstOrDefaultAsync();

                if (newlyAddedFood != null)
                {
                    long targetFoodId = newlyAddedFood.Id;

                    foreach (var conv in model.Conversions.Where(c => c.UnitTypeId > 0 && c.ConversionFactor > 0))
                    {
                        if (conv.UnitTypeId == model.DefaultUnitId) continue;

                        _context.FoodUnitConversions.Add(new FitCore.Domain.Entities.NutritionProgram.Food.FoodUnitConversion
                        {
                            FoodId = targetFoodId,
                            UnitTypeId = conv.UnitTypeId,
                            ConversionFactor = conv.ConversionFactor,
                            GymId = targetGymId // ✅ ذخیره شناسه باشگاه (اگر سراسری باشد نال می‌ماند)
                        });
                    }
                    await _context.SaveChangesAsync();
                }
            }
            // ==========================================

            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message,
                redirectUrl = Url.Action(nameof(Index))
            });
        }

        private async Task<(long? GymId, bool IsAdmin)> GetCurrentUserGymContextAsync()
        {
            bool isAdmin = User.IsSuperAdmin();

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

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            long Id = SecurityUtils.DecryptId(id);
            var item = await _context.Foods.FirstOrDefaultAsync(x => x.Id == Id);

            await FillLookupsAsync();

            var food = await _foodFacad.GetFoodByIdService.Execute(Id);

            if (food == null) return NotFound();

            var (gymId, isAdmin) = await GetCurrentUserGymContextAsync();

            if (!isAdmin && item.GymId != gymId)
            {
                return Forbid();
            }

            await FillLookupsAsync();

            ViewBag.IsSuperAdmin = isAdmin;

            var vm = await BuildViewModelAsync(new FoodCreateEditViewModel
            {
                Id = food.Id,
                Title = food.Title,
                EnglishTitle = food.EnglishTitle,
                CategoryTypeId = food.CategoryTypeId,
                CaloriesPerUnit = food.CaloriesPerUnit,
                ProteinPerUnit = food.ProteinPerUnit,
                CarbohydratePerUnit = food.CarbohydratePerUnit,
                FatPerUnit = food.FatPerUnit,
                DefaultUnitId = food.DefaultUnitId,
                IsActive = food.IsActive,
                IsGlobal = item.GymId == null
            });

            // ✅ بارگذاری ضرایب تبدیل (فقط ضرایب سراسری + ضرایب مختص مالک این غذا)
            var existingConversions = await _context.FoodUnitConversions
                .Where(c => c.FoodId == Id && (c.GymId == null || c.GymId == item.GymId))
                .Select(c => new FoodUnitConversionItemDto
                {
                    UnitTypeId = c.UnitTypeId,
                    ConversionFactor = c.ConversionFactor,
                })
                .ToListAsync();

            vm.Conversions = existingConversions;

            return View("CreateEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FoodCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await BuildViewModelAsync(model);
                return View(model);
            }

            var result = await _foodFacad.EditFoodService.Execute(new UpdateFoodDto
            {
                Id = model.Id ?? 0,
                Title = model.Title,
                EnglishTitle = model.EnglishTitle,
                CategoryTypeId = model.CategoryTypeId,
                CaloriesPerUnit = model.CaloriesPerUnit,
                ProteinPerUnit = model.ProteinPerUnit,
                CarbohydratePerUnit = model.CarbohydratePerUnit,
                FatPerUnit = model.FatPerUnit,
                DefaultUnitId = model.DefaultUnitId,
                IsActive = model.IsActive
            });

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message ?? "خطا");
                model = await BuildViewModelAsync(model);
                return View(model);
            }

            // ==========================================
            // ذخیره ضرایب تبدیل در حالت ویرایش (بسیار حساس)
            // ==========================================
            long targetFoodId = model.Id.Value;

            // ✅ 0. پیدا کردن مالک غذا (GymId) برای تعیین محدوده مجاز ویرایش
            var foodOwnerId = await _context.Foods
                .AsNoTracking()
                .Where(f => f.Id == targetFoodId)
                .Select(f => f.GymId)
                .FirstOrDefaultAsync();

            // ✅ 1. حذف قطعی ضرایب قبلی (فقط ضرایب مربوط به همین اسکوپ)
            // این دستور جلوی پاک شدن ضرایب سراسری توسط مدیران باشگاه‌ها را می‌گیرد!
            if (foodOwnerId == null)
            {
                // غذا سراسری است -> فقط ضرایب سراسری حذف شوند
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM FoodUnitConversions WHERE FoodId = {0} AND GymId IS NULL", targetFoodId);
            }
            else
            {
                // غذا متعلق به باشگاه است -> فقط ضرایب آن باشگاه حذف شوند
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM FoodUnitConversions WHERE FoodId = {0} AND GymId = {1}", targetFoodId, foodOwnerId);
            }

            // ✅ 2. افزودن ضرایب جدید (با ثبت GymId مربوطه)
            if (model.Conversions != null && model.Conversions.Any())
            {
                var newConversions = model.Conversions
                    .Where(c => c.UnitTypeId > 0 && c.ConversionFactor > 0)
                    .Where(c => c.UnitTypeId != model.DefaultUnitId)
                    .Select(c => new FitCore.Domain.Entities.NutritionProgram.Food.FoodUnitConversion
                    {
                        FoodId = targetFoodId,
                        UnitTypeId = c.UnitTypeId,
                        ConversionFactor = c.ConversionFactor,
                        GymId = foodOwnerId // ✅ ثبت شناسه مالک غذا
                    })
                    .ToList();

                if (newConversions.Any())
                {
                    await _context.FoodUnitConversions.AddRangeAsync(newConversions);
                    await _context.SaveChangesAsync();
                }
            }
            // ==========================================

            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message,
                redirectUrl = Url.Action(nameof(Index))
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "شناسه ارسال نشده است."
                });
            }

            long foodId = SecurityUtils.DecryptId(id);

            var result = await _foodFacad.DeleteFoodService.Execute(foodId);

            return Json(new
            {
                isSuccess = result.IsSuccess,
                message = result.Message
            });
        }

        private async Task<FoodCreateEditViewModel> BuildViewModelAsync(FoodCreateEditViewModel? model = null)
        {
            model ??= new FoodCreateEditViewModel();

            model.CategoryTypes = await _context.FoodCategoryTypes
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            model.DefaultUnits = await _context.NutritionUnitTypes
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            return model;
        }

        private async Task FillLookupsAsync()
        {
            var foodCategoryTypes = await _context.FoodCategoryTypes
                .OrderBy(x => x.Name.Trim())
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name.Trim()
                })
                .ToListAsync();

            foodCategoryTypes.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "انتخاب کنید"
            });

            ViewBag.foodCategoryTypes = foodCategoryTypes;

            var foodUnitTypes = await _context.NutritionUnitTypes
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            foodUnitTypes.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "انتخاب کنید"
            });

            ViewBag.foodUnitTypes = foodUnitTypes;
        }
    }
}