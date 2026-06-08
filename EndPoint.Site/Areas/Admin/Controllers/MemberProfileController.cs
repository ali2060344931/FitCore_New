using FitCore.Application.Contexts;
using FitCore.Application.Services.Members.Commands;
using FitCore.Application.Services.Members.Queries;
using FitCore.Application.Services.Members.Queries.ReportMembers;
using FitCore.Domain.Entities.Members;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Linq;
using System.Security.Claims;

namespace FitCore.EndPoint.Site.Areas.MemberPanel.Controllers
{
    //[Area("MemberPanel")]
    [Area("Admin")]
    public class MemberProfileController : Controller
    {
        private readonly IAddOrUpdateMemberService _addOrUpdateMemberService;
        private readonly IGetMemberByAppUserIdService _getMemberByAppUserIdService;
        private readonly IAddMemberBodyMeasurementService _addMemberBodyMeasurementService;
        private readonly IEditMemberBodyMeasurementService _editMemberBodyMeasurementService;
        private readonly IDataBaseContext _context;
        public MemberProfileController(
            IAddOrUpdateMemberService addOrUpdateMemberService,
            IGetMemberByAppUserIdService getMemberByAppUserIdService,
            IAddMemberBodyMeasurementService addMemberBodyMeasurementService,
            IEditMemberBodyMeasurementService editMemberBodyMeasurementService,
            IDataBaseContext context)
        {
            _addOrUpdateMemberService = addOrUpdateMemberService;
            _getMemberByAppUserIdService = getMemberByAppUserIdService;
            _addMemberBodyMeasurementService = addMemberBodyMeasurementService;
            _editMemberBodyMeasurementService = editMemberBodyMeasurementService;
            _context = context;
        }
        [HttpGet]
        public IActionResult CompleteInfo()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdValue))
            {
                return Unauthorized();
            }

            var appUserId = long.Parse(userIdValue);
            var memberId = _context.Members.Where(c => c.AppUserId == appUserId).FirstOrDefault().Id;

            var member = _context.Members
                .Where(x => x.Id == memberId)
                .Select(x => new GetMemberCompleteInfoDto
                {
                    Id = x.Id,
                    FullName = x.AppUser.FullName,
                    Mobile = x.AppUser.PhoneNumber,
                    Gender = x.Gender,
                    ActivityLevelId = x.ActivityLevelId,
                    ExperienceLevelId = x.ExperienceLevelId,
                    MembershipStartDate = x.MembershipStartDate,
                    MembershipEndDate = x.MembershipEndDate,
                    BirthDate = x.BirthDate,
                    Description = x.Description,
                    Injuries = x.Injuries,
                    IsActive = x.IsActive,
                    FoodAllergies = x.FoodAllergies,
                    MedicalConditions = x.MedicalConditions,
                    Height=x.Height,

                }).FirstOrDefault();

            ViewBag.ActivityLevels = _context.activityLevels
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

            ViewBag.ExperienceLevels = _context.experiences
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

            return View(member);
        }

        [HttpPost]
        public IActionResult CompleteInfo(RequestCompleteMemberInfoDto request)
        {
            request.AppUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = _addOrUpdateMemberService.Execute(request);
            return Json(result);
        }


        [HttpGet]
        public IActionResult AddBodyMeasurement()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdValue))
            {
                return Unauthorized();
            }

            var appUserId = long.Parse(userIdValue);
            var memberId = _context.Members.Where(c => c.AppUserId == appUserId).FirstOrDefault().Id;


            var model = new RequestAddMemberBodyMeasurementDto
            {
                MemberId = memberId
            };
            ViewBag.FullName=_context.Users.Where(c=>c.Id == appUserId).FirstOrDefault().FullName;
            ViewBag.Mobile=_context.Users.Where(c=>c.Id == appUserId).FirstOrDefault().PhoneNumber;
            
            return View(model);
        }

        [HttpPost]
        public IActionResult AddBodyMeasurement(RequestAddMemberBodyMeasurementDto request)
        {

            var result = _addMemberBodyMeasurementService.Execute(request);
            return Json(result);
        }

        [HttpPost]
        public IActionResult EditBodyMeasurement(RequestEditMemberBodyMeasurementDto request)
        {
            var result = _editMemberBodyMeasurementService.Execute(request);
            return Json(result);
        }
    }
}
