using FitCore.Application.Interfaces.IGym;
using FitCore.Application.Services.Gyms.Commands;
using FitCore.Application.Services.Gyms.Commands.AddGym;
using FitCore.Application.Services.Gyms.Commands.EditGym;
using FitCore.Application.Services.Gyms.Commands.EditGym.FitCore.Application.ViewModels.Gyms;
using FitCore.Application.ViewModels.Gyms;
using FitCore.Common.Dto;

using Humanizer;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GymController : Controller
    {
        private readonly IGetGymsService _getGymsService;
        private readonly IEditGymService _editGymService;
        private readonly IAddGymService _addGymService;
        private readonly IDeleteGymService _deleteGymService;
        private readonly IGetGymByIdService _getGymByIdService;
        private readonly ICompleteGymInfoService _completeGymInfoService;

        public readonly IGetProvincesService _getProvincesService;

        public readonly IGetCitiesService _getCitiesService;
        public GymController(IGetCitiesService getCitiesService, IGetProvincesService getProvincesService, ICompleteGymInfoService completeGymInfoService, IGetGymByIdService getGymByIdService, IGetGymsService gymService, IEditGymService editGymService, IAddGymService addGymService, IDeleteGymService deleteGymService)
        {
            _getGymByIdService = getGymByIdService;
            _getGymsService = gymService;
            _editGymService = editGymService;
            _addGymService = addGymService;
            _deleteGymService = deleteGymService;
            _completeGymInfoService = completeGymInfoService;
            _getProvincesService = getProvincesService;
            _getCitiesService = getCitiesService;
        }


        public IActionResult Index(string SearchKey = "")
        {
            var gyms = _getGymsService.GetAll();

            if (!string.IsNullOrWhiteSpace(SearchKey))
            {
                gyms = gyms.Where(x =>

                    x.Name.Contains(SearchKey) ||

                    x.Code.Contains(SearchKey) ||

                    x.MobileNumber.Contains(SearchKey)

                ).ToList();
            }

            return View(gyms);
        }
        //public IActionResult Index()
        //{
        //    var gyms = _getGymsService.GetAll();
        //    return View(gyms);
        //}


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(CreateGymDto vm)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new
                {
                    isSuccess = false,
                    message = string.Join(" - ", errors)
                });
            }

            var result = _addGymService.Execute(vm);

            return Json(result);
        }


        [HttpGet]
        public IActionResult Edit(long id)
        {
            var gym = _getGymByIdService.GetById(id);
            if (gym == null) return NotFound();

            var model = new UpdateGymDto
            {
                Id = gym.Id,
                Name = gym.Name,
                Code = gym.Code,
                MobileNumber = gym.MobileNumber,
                Description = gym.Description,

            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(UpdateGymDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new
                {
                    isSuccess = false,
                    message = string.Join(" - ", errors)
                });
            }
            var result = _editGymService.Execute(dto);
            return Json(result);
        }


        [HttpPost]
        public IActionResult Delete(long id)
        {
            return Json(_deleteGymService.Execute(id));
        }

        [HttpGet]
        public IActionResult CompleteInfo(long id)
        {
            var gym = _getGymByIdService.GetById(id);

            if (gym == null)
            {
                return NotFound();
            }

            var model = new CompleteGymInfoDto()
            {
                Id = gym.Id,

                BrandName = gym.BrandName,

                SubDomain = gym.SubDomain,

                PhoneNumber = gym.PhoneNumber,

                Email = gym.Email,

                Website = gym.Website,

                CitiesId = gym.CitiesId,

                Address = gym.Address,

                PostalCode = gym.PostalCode,

                Latitude = gym.Latitude,

                Longitude = gym.Longitude,

                IsActive = gym.IsActive,

                SubscriptionExpireDate =
                    gym.SubscriptionExpireDate,

                MaxMembers = gym.MaxMembers,

                AllowOnlineRegistration =
                    gym.AllowOnlineRegistration,

                OtpExpireSeconds =
                    gym.OtpExpireSeconds,

                MaxOtpRequestPerMinute =
                    gym.MaxOtpRequestPerMinute
            };
            ViewBag.Provinces = _getProvincesService.Execute()
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                })
                .ToList();

//            var city = _getCitiesService.Execute(1);


//            ViewBag.Cities = new SelectList(
//    _getCitiesService.Execute(city.ProvincesId),
//    "Id",
//    "Name",
//    model.CitiesId
//);

            return View(model);
        }


        [HttpPost]
        public IActionResult CompleteInfo(CompleteGymInfoDto dto)
        {
            var result = _completeGymInfoService.Execute(dto);

            return Json(result);
        }

        [HttpGet]
        public IActionResult GetCities(int provinceId)
        {
            var cities = _getCitiesService.Execute(provinceId)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToList();

            return Json(cities);
        }
    }
}
