using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Interfaces.IMembers;
using FitCore.Application.Services.Gyms.Commands.EditGym;
using FitCore.Application.Services.Member.Queries;
using FitCore.Application.Services.Members.Commands;
using FitCore.Common;

using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.RulesetToEditorconfig;

using System;
using System.Linq;
using System.Security.Claims;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MembersController : Controller
    {
        private readonly IMemberFacad _memberFacad;
        private readonly IDataBaseContext _dataBaseContext;
        private readonly IGetMembersByIdService _getMembersByIdService;
        public MembersController(IGetMembersByIdService getMembersByIdService, IMemberFacad memberFacad, IDataBaseContext dataBaseContext)
        {
            _memberFacad = memberFacad;
            _dataBaseContext = dataBaseContext;
            _getMembersByIdService = getMembersByIdService;
        }
        // در کنترلر یا یک کلاس Utility

        [HttpGet]
        public IActionResult Index(int page = 1, string SearchKey = "")
        {
            var appUserId = long.Parse(User.FindFirstValue("AppUserId"));
            var request = new RequestGetMemberDto
            {
                AppUserId = appUserId,
                Page = page,
                PageSize = 10,
                SearchKey = SearchKey
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

            var appUserId = long.Parse(User.FindFirstValue("AppUserId"));
            request.AppUserId = appUserId;

            var result = _memberFacad.AddNewMemberService.Execute(request);
            return Json(result);
        }


        [HttpGet]
        public IActionResult Edit(string Id)
        {
            // تبدیل رشته به عدد واقعی
            long decryptedId = SecurityUtils.DecryptId(Id);

            // فراخوانی سرویس با عدد به دست آمده
            var q = _getMembersByIdService.Execute((int)decryptedId);

            if (q.Data == null) return NotFound();

            var qq = new GetMemberByIdDto
            {
                Id= decryptedId,
                FirstName = q.Data.FirstName,
                LastName = q.Data.LastName,
                Mobile = q.Data.Mobile,
                BirthDate = q.Data.BirthDate,
                Gender = q.Data.Gender,
            };
            return View(qq);
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