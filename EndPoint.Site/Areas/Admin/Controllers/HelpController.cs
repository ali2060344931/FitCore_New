
using EndPoint.Site.Areas.Admin.Models;

using FitCore.Application.Contexts;
using FitCore.Application.Services.Halpe;
using FitCore.Domain.Entities.Help;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HelpController : Controller
    {
        private readonly IDataBaseContext _context;
        private readonly IHelp_Service _helpService;

        public HelpController(IDataBaseContext context, IHelp_Service helpService)
        {
            _context = context;
            _helpService = helpService;
        }


        [HttpGet]
        public async Task<IActionResult> Get(string key)
        {
            var result = await _helpService.GetHelpAsync(key);
            return Json(new { title = result.title, content = result.content });
        }


        public async Task<IActionResult> Index()
        {
            var list = await _context.HelpContents
                .AsNoTracking()
                .ToListAsync();

            return View(list);
        }

        public async Task<IActionResult> CreateEdit(long? id)
        {
            if (id == null)
                return View(new HelpContentCreateEditViewModel());

            var item = await _context.HelpContents.FindAsync(id);

            if (item == null)
                return NotFound();

            return View(new HelpContentCreateEditViewModel
            {
                Id = item.Id,
                HelpKey = item.HelpKey,
                Title = item.Title,
                Content = item.Content,
                IsActive = item.IsActive
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateEdit(HelpContentCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Id == 0)
            {
                var entity = new HelpContent
                {
                    HelpKey = model.HelpKey,
                    Title = model.Title,
                    Content = model.Content,
                    IsActive = model.IsActive
                };

                _context.HelpContents.Add(entity);
            }
            else
            {
                var entity = await _context.HelpContents.FindAsync(model.Id);

                entity.HelpKey = model.HelpKey;
                entity.Title = model.Title;
                entity.Content = model.Content;
                entity.IsActive = model.IsActive;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _context.HelpContents.FindAsync(id);

            if (entity != null)
            {
                _context.HelpContents.Remove(entity);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
