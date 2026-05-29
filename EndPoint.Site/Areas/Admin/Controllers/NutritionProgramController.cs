using FitCore.Application.FacadPatterns;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class NutritionProgramController : Controller
    {
        private readonly INutritionProgramFacad _nutritionProgramFacad;

        public NutritionProgramController(
            INutritionProgramFacad nutritionProgramFacad)
        {
            _nutritionProgramFacad = nutritionProgramFacad;
        }

        //====================================================
        // لیست برنامه های غذایی
        //====================================================

        [HttpGet]
        public async Task<IActionResult> Index(
            string searchKey = "",
            int page = 1)
        {
            var result =
                await _nutritionProgramFacad.GetNutritionProgramsService.Execute(searchKey, page);

            return View(result);
        }


        // GET: NutritionProgramController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NutritionProgramController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NutritionProgramController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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
