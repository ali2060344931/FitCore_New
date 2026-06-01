using FitCore.Application.Services.NutritionProgramBuilder.Queries;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    public class NutritionProgramBuilderController : Controller
    {
        private readonly
                IGetProgramBuilderService
                _getProgramBuilderService;

        public NutritionProgramBuilderController(
            IGetProgramBuilderService
                getProgramBuilderService)
        {
            _getProgramBuilderService =
                getProgramBuilderService;
        }

        public IActionResult Index(long id)
        {
            var result = _getProgramBuilderService.Execute(id);

            return View(result);
        }
        // GET: NutritionProgramBuilder/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NutritionProgramBuilder/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NutritionProgramBuilder/Create
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

        // GET: NutritionProgramBuilder/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NutritionProgramBuilder/Edit/5
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

        // GET: NutritionProgramBuilder/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NutritionProgramBuilder/Delete/5
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
