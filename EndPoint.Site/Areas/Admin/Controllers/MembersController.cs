using FitCore.Application.Interfaces.FacadPatterns;
using FitCore.Application.Services.Members.Commands;

using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MembersController : Controller
    {
        private readonly IMemberFacad _memberFacad;

        public MembersController(
            IMemberFacad memberFacad)
        {
            _memberFacad = memberFacad;
        }

        public IActionResult Index()
        {
            long gymId = 2;

            var result = _memberFacad.GetMembersService.Execute(gymId);

            return View(result.Data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RequestAddNewMemberDto request)
        {
            //var result = _memberFacad.AddNewMemberService.Execute(request);
            //return Json(new RequestAddNewMemberDto());
            var result = _memberFacad.AddNewMemberService.Execute(request);
            return Json(result);
        }
    }
}