using FitCore.Application.Contexts;
using FitCore.Application.Services;
using FitCore.Application.Services.Members.Commands;
using FitCore.Application.Services.Members.Queries;
using FitCore.Application.Services.Members.Queries.ReportMembers;
using FitCore.Common;
using FitCore.Common.Dto;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
        private readonly IGetMemberBodyMeasurementsService _getMemberBodyMeasurementsService;
        private readonly IRemoveBodyMeasurementService _removeBodyMeasurementService;
        private readonly IDataBaseContext _context;
        private readonly IFileCompressionService _fileService;


        public MemberProfileController(
            IAddOrUpdateMemberService addOrUpdateMemberService,
            IGetMemberByAppUserIdService getMemberByAppUserIdService,
            IAddMemberBodyMeasurementService addMemberBodyMeasurementService,
            IEditMemberBodyMeasurementService editMemberBodyMeasurementService,
            IDataBaseContext context,
            IGetMemberBodyMeasurementsService getMemberBodyMeasurementsService,
            IRemoveBodyMeasurementService removeBodyMeasurementService, IFileCompressionService fileService)
        {
            _addOrUpdateMemberService = addOrUpdateMemberService;
            _getMemberByAppUserIdService = getMemberByAppUserIdService;
            _addMemberBodyMeasurementService = addMemberBodyMeasurementService;
            _editMemberBodyMeasurementService = editMemberBodyMeasurementService;
            _context = context;
            _getMemberBodyMeasurementsService = getMemberBodyMeasurementsService;
            _removeBodyMeasurementService = removeBodyMeasurementService;
            _fileService = fileService;
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
                    Height = x.Height,


                    ProfileImageUrl = x.ProfileImageUrl,
                    VideoUrl = x.VideoUrl,
                    BodyImageUrl1 = x.BodyImageUrl1,
                    BodyImageUrl2 = x.BodyImageUrl2,
                    BodyImageUrl3 = x.BodyImageUrl3

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


        // اضافه شدن async و Task
        [HttpPost]
        public async Task<IActionResult> CompleteInfo(RequestCompleteMemberInfoDto request)
        {
            request.AppUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // اضافه شدن await
            var result = await _addOrUpdateMemberService.Execute(request);

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> BodyMeasurement(string Id, string SearchKey, int page = 1, int pageSize = 10)
        {
            var id = SecurityUtils.DecryptId(Id);

            var memberId = _context.Members.Where(c => c.AppUserId == id).FirstOrDefault().Id;

            var request = new RequestGetMemberBodyMeasurementsDto
            {
                MemberId = memberId,
                Page = page,
                PageSize = pageSize,
                SearchKey = SearchKey
            };

            var result = await _getMemberBodyMeasurementsService.Execute(request);
            var q = _context.Users.Where(c => c.Id == id).FirstOrDefault();
            ViewBag.FullName_Mobile = q.FullName + " - " + q.PhoneNumber;
            ViewBag.MemberId = memberId;

            return View(result);
        }



        [HttpPost]
        public ResultDto DeleteMedia(string mediaType)
        {
            // دریافت AppUserId از کاربر لاگین شده
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdValue))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "لطفاً مجدداً وارد شوید"
                };
            }

            var appUserId = long.Parse(userIdValue);

            var member = _context.Members
                .FirstOrDefault(x => x.AppUserId == appUserId);

            if (member == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "عضو یافت نشد"
                };
            }

            string fileUrl = null;

            switch (mediaType)
            {
                case "ProfileImage":
                    fileUrl = member.ProfileImageUrl;
                    member.ProfileImageUrl = null;
                    break;

                case "Video":
                    fileUrl = member.VideoUrl;
                    member.VideoUrl = null;
                    break;

                case "BodyImage1":
                    fileUrl = member.BodyImageUrl1;
                    member.BodyImageUrl1 = null;
                    break;

                case "BodyImage2":
                    fileUrl = member.BodyImageUrl2;
                    member.BodyImageUrl2 = null;
                    break;

                case "BodyImage3":
                    fileUrl = member.BodyImageUrl3;
                    member.BodyImageUrl3 = null;
                    break;

                default:
                    return new ResultDto
                    {
                        IsSuccess = false,
                        Message = "نوع فایل نامعتبر است"
                    };
            }

            // فقط در صورتی که فایلی وجود داشت حذف شود
            if (!string.IsNullOrEmpty(fileUrl))
            {
                _fileService.DeleteFile(fileUrl);
            }

            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "فایل با موفقیت حذف شد."
            };
        }

        [HttpGet]
        public IActionResult AddBodyMeasurement(string? memberId)
        {
            long id;

            if (!string.IsNullOrEmpty(memberId))
            {
                id = SecurityUtils.DecryptId(memberId);
            }
            else
            {
                var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userIdValue))
                {
                    return Unauthorized();
                }

                var appUserId = long.Parse(userIdValue);
                id = _context.Members.FirstOrDefault(c => c.AppUserId == appUserId).Id;
            }

            var member = _context.Members.FirstOrDefault(c => c.Id == id);
            var user = _context.Users.FirstOrDefault(c => c.Id == member.AppUserId);

            var model = new RequestAddMemberBodyMeasurementDto
            {
                MemberId = id
            };

            ViewBag.FullName = user.FullName;
            ViewBag.Mobile = user.PhoneNumber;

            return View("CreateEditBodyMeasurement", model);
        }


        [HttpPost]
        public IActionResult AddBodyMeasurement(RequestAddMemberBodyMeasurementDto request)
        {

            var result = _addMemberBodyMeasurementService.Execute(request);
            return Json(result);
        }

        [HttpPost]
        public IActionResult deleteMeasurement(string Id)
        {
            var id = SecurityUtils.DecryptId(Id);

            var result = _removeBodyMeasurementService.Execute(id);

            return Json(result);
        }

        [HttpGet]
        public IActionResult EditBodyMeasurement(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            long recordId;

            try
            {
                recordId = SecurityUtils.DecryptId(id);
            }
            catch
            {
                return BadRequest("شناسه نامعتبر است.");
            }

            var record = _context.memberBodyMeasurements
                .Include(x => x.Member)
                .ThenInclude(x => x.AppUser)
                .FirstOrDefault(x => x.Id == recordId);

            if (record == null)
                return NotFound();

            // پر کردن اطلاعات عضو برای نمایش readonly
            ViewBag.FullName = record.Member?.AppUser?.FullName ?? "-";
            ViewBag.Mobile = record.Member?.AppUser?.PhoneNumber ?? "-";

            // مپ به مدل CreateEdit
            var model = new RequestAddMemberBodyMeasurementDto
            {
                Id = record.Id,
                MemberId = record.MemberId,
                RecordDate = record.RecordDate,
                Weight = record.Weight,
                BodyFatPercentage = record.BodyFatPercentage,
                Waist = record.Waist,
                Hip = record.Hip,
                Chest = record.Chest
            };

            return View("CreateEditBodyMeasurement", model);
        }

        [HttpPost]
        public IActionResult EditBodyMeasurement(RequestEditMemberBodyMeasurementDto request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "اطلاعات ورودی معتبر نیست."
                });
            }

            var result = _editMemberBodyMeasurementService.Execute(request);

            if (!result.IsSuccess)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = result.Message
                });
            }

            return Json(new
            {
                isSuccess = true,
                message = result.Message
            });
        }

    }
}
