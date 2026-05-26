using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.Gyms.Commands.EditGym;
using FitCore.Application.Services.Member.Queries;
using FitCore.Application.Services.Members.Commands;

using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.RulesetToEditorconfig;

using System.Linq;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MembersController : Controller
    {
        private readonly IMemberFacad _memberFacad;
        private readonly IDataBaseContext _dataBaseContext;

        public MembersController(IMemberFacad memberFacad, IDataBaseContext dataBaseContext)
        {
            _memberFacad = memberFacad;
            _dataBaseContext= dataBaseContext;
        }

        [HttpGet]
        public IActionResult Index(int page = 1, string SearchKey = "")
        {
            long gymId = 23;

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



        //[HttpGet]
        //public IActionResult Edit(string code)
        //{
        //    var gym = _getGymByIdService.GetById(code);
        //    if (gym == null) return NotFound();

        //    var model = new UpdateGymDto
        //    {
        //        Id = gym.Id,
        //        Name = gym.Name,
        //        Code = gym.Code,
        //        MobileNumber = gym.MobileNumber,
        //        Description = gym.Description,

        //    };

        //    return View(model);
        //}

        [HttpGet]
        public IActionResult Edit(string code)
        {
            //int id=Converter.int
            //var gym = _dataBaseContext.Members.Where(c=>c.Id    == code);

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