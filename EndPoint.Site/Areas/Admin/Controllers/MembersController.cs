using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.Member.Queries;
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

        public IActionResult Index(int page = 1, string SearchKey="")
        {
            long gymId = 2;

            var request = new RequestGetMemberDto
            {
                GymId = gymId,
                Page = page,
                PageSize = 10,
                SearchKey = SearchKey
                //SearchKey
            };

            var result = _memberFacad.GetMembersService.Execute(request);

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
            var result = _memberFacad.AddNewMemberService.Execute(request);
            return Json(result);
        }


        [HttpGet]
        public IActionResult Edit(long id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(RequestEditMemberDto request)
        {
            var result = _memberFacad.EditMemberService.Execute(request);

            return Json(result);
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var result = _memberFacad.RemoveMemberService.Execute(id);

            return Json(result);
        }
    }
}