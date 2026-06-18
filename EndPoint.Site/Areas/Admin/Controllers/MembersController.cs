using EndPoint.Site.Areas.Admin.Models.Member;

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
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MembersController : Controller
    {
        private readonly IMemberFacad _memberFacad;
        private readonly IDataBaseContext _dataBaseContext;
        private readonly IGetMembersByIdService _getMembersByIdService;
        private readonly IAddNewMemberService _addNewMemberService;
        private readonly IDataBaseContext _context;
        public MembersController(IAddNewMemberService addNewMemberService, IGetMembersByIdService getMembersByIdService, IMemberFacad memberFacad, IDataBaseContext dataBaseContext, IDataBaseContext context)
        {
            _memberFacad = memberFacad;
            _dataBaseContext = dataBaseContext;
            _getMembersByIdService = getMembersByIdService;
            _addNewMemberService = addNewMemberService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string SearchKey = "")
        {

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdValue))
            {
                return Unauthorized();
            }

            var appUserId = long.Parse(userIdValue);

            var request = new RequestGetMemberDto
            {
                AppUserId = appUserId,
                Page = page,
                PageSize = 10,
                SearchKey = SearchKey
            };

            var result = await _memberFacad.GetMembersService.Execute(request);

            return View(result.Data);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View("CreateEdit", new MemberCreateEditViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestAddNewMemberDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            request.AppUserId = long.Parse(userId);
            var result = await _memberFacad.AddNewMemberService.Execute(request);
            return Json(result);
        }



        [HttpGet]
        public IActionResult Edit(string Id,bool isMemberID=false)
        {
            // تبدیل رشته به عدد واقعی
            long decryptedId = SecurityUtils.DecryptId(Id);

            if(isMemberID)
            {
                decryptedId= _context.Members.Where(v=>v.Id == decryptedId).FirstOrDefault().AppUserId;
            }


            // فراخوانی سرویس با عدد به دست آمده
            var q = _getMembersByIdService.Execute((int)decryptedId);

            if (q.Data == null) return NotFound();

            var qq = new MemberCreateEditViewModel
            {
                Id = decryptedId,
                FullName = q.Data.FullName,
                Mobile = q.Data.Mobile,
                BirthDate = q.Data.BirthDate,
                Gender = q.Data.Gender,
                MembershipStartDate = q.Data.MembershipStartDate,
                MembershipEndDate = q.Data.MembershipEndDate,
            };
            return View("CreateEdit", qq);
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