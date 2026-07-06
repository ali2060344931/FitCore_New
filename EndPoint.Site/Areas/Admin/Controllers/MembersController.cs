using EndPoint.Site.Areas.Admin.Models.Member;
using EndPoint.Site.BaleBot.Services;

using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Interfaces.IMembers;
using FitCore.Application.Services.Gyms.Commands.EditGym;
using FitCore.Application.Services.Member.Queries;
using FitCore.Application.Services.Members.Commands;
using FitCore.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; // حتماً مطمئن شوید این Using وجود دارد
using Microsoft.CodeAnalysis.RulesetToEditorconfig;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]

    //[Authorize(Roles = "Admin, Trainer, Member")]
    public class MembersController : Controller
    {
        private readonly IMemberFacad _memberFacad;
        private readonly IDataBaseContext _dataBaseContext;
        private readonly IGetMembersByIdService _getMembersByIdService;
        private readonly IAddNewMemberService _addNewMemberService;
        private readonly IDataBaseContext _context;
        private readonly IBaleMenuService _baleMenuService;

        public MembersController(IAddNewMemberService addNewMemberService, IGetMembersByIdService getMembersByIdService, IMemberFacad memberFacad, IDataBaseContext dataBaseContext, IDataBaseContext context, IBaleMenuService baleMenuService)
        {
            _memberFacad = memberFacad;
            _dataBaseContext = dataBaseContext;
            _getMembersByIdService = getMembersByIdService;
            _addNewMemberService = addNewMemberService;
            _context = context;
            _baleMenuService = baleMenuService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string SearchKey = "")
        {
            // این متد بدون تغییر باقی می‌ماند. چون AppUserId مربی را می‌گیرد 
            // و سرویس مربوطه اعضای باشگاه مربی را برمی‌گرداند.
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
        public IActionResult Edit(string Id, bool isMemberID = false)
        {
            long decryptedId = SecurityUtils.DecryptId(Id);

            if (isMemberID)
            {
                decryptedId = _context.Members.Where(v => v.Id == decryptedId).FirstOrDefault().AppUserId;
            }

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
                IsActive = q.Data.IsActive
            };
            return View("CreateEdit", qq);
        }


        [HttpPost]
        public IActionResult Edit(RequestEditMemberDto request)
        {
            // ارسال پیام به ربات هم بدون تغییر باقی می‌ماند و برای مربی هم کار می‌کند
            string msg = "تغییراتی در پنل کاربری از طرف مدیر باشگاه انجام شد\nلطفاً بخش اطلاعات کاربر، موارد ویرایش شده را مشاهده نمائید";
            var result = _memberFacad.EditMemberService.Execute(request);

            if (result.IsSuccess)
            {
                long memberId = result.Data;

                var member = _context.Members
                               .Include(x => x.AppUser)
                               .FirstOrDefault(x => x.Id == memberId);

                _baleMenuService.EditMemberInfoSend((long)member.AppUser.BaleChatId, msg);
            }
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