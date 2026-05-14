using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bugeto_Store.Application.Services.Users.Commands.EditUser;
using Bugeto_Store.Application.Services.Users.Commands.RemoveUser;
using Bugeto_Store.Application.Services.Users.Commands.RgegisterUser;
using Bugeto_Store.Application.Services.Users.Commands.UserSatusChange;
using Bugeto_Store.Application.Services.Users.Queries.GetRoles;
using Bugeto_Store.Application.Services.Users.Queries.GetUsers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IGetUsersService _getUsersService;
        private readonly IGetRolesService _getRolesService;
        private readonly IRegisterUserService _registerUserService;
        private readonly IRemoveUserService _removeUserService;
        private readonly IUserSatusChangeService _userSatusChangeService;
        private readonly IEditUserService _editUserService;
        public UsersController(IGetUsersService getUsersService
            , IGetRolesService getRolesService
            , IRegisterUserService registerUserService
            , IRemoveUserService removeUserService
            , IUserSatusChangeService userSatusChangeService
            , IEditUserService editUserService)
        {
            _getUsersService = getUsersService;
            _getRolesService = getRolesService;
            _registerUserService = registerUserService;
            _removeUserService = removeUserService;
            _userSatusChangeService = userSatusChangeService;
            _editUserService = editUserService;
        }

        public IActionResult Index(string serchkey, int page = 1)
        {

            //ViewBag.SearchKey = serchkey;

            //return View(result);

            ViewBag.SearchKey = serchkey;
            return View(_getUsersService.Execute(new RequestGetUserDto
            {
                Page = page,
                SearchKey = serchkey,
            }));
        }





        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_getRolesService.Execute().Data, "Id", "Name");
            return View();
        }



        //[HttpPost]
        //public IActionResult Create(RequestRegisterUserDto request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var result = _registerUserService.Execute(request);
        //    return Json(result);
        //}



        
        [HttpPost]
        public IActionResult Create(string Email, string FullName, long RoleId, string Password, string RePassword,string Tel)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}


            var result = _registerUserService.Execute(new RequestRegisterUserDto
            {
                Email = Email,
                FullName = FullName,
                Tel=Tel,
                roles = new List<RolesInRegisterUserDto>()
                   {
                        new RolesInRegisterUserDto
                        {
                             Id= RoleId
                        }
                   },
                Password = Password,
                RePasword = RePassword,
            });
            return Json(result);
        }
       



        [HttpPost]
        public IActionResult Delete(long UserId)
        {
            return Json(_removeUserService.Execute(UserId));
        }

        [HttpPost]
        public IActionResult UserSatusChange(long UserId)
        {
            return Json(_userSatusChangeService.Execute(UserId));
        }

        [HttpPost]
        public IActionResult Edit(long UserId, string Fullname,string Email,string Tel)
        {
            return Json(_editUserService.Execute(new RequestEdituserDto
            {
                Fullname = Fullname,
                Email= Email,
                Tel=Tel,
                UserId = UserId,
            }));
        }


        [HttpGet]
        public IActionResult UserRoles(string userId)
        {
            var result = _getRolesService.Execute();

            return PartialView("_UserRoles", result);
        }

    }
}
