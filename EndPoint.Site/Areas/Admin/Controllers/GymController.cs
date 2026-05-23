using FitCore.Application.Interfaces.IGym;
using FitCore.Application.Services.Gyms.Commands;
using FitCore.Application.Services.Gyms.Commands.AddGym;
using FitCore.Application.Services.Gyms.Commands.EditGym;
using FitCore.Application.ViewModels.Gyms;

using Microsoft.AspNetCore.Mvc;

using System.Linq;

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

        public GymController(IGetGymByIdService getGymByIdService, IGetGymsService gymService, IEditGymService editGymService, IAddGymService addGymService, IDeleteGymService deleteGymService)
        {
            _getGymByIdService = getGymByIdService;
            _getGymsService = gymService;
            _editGymService = editGymService;
            _addGymService = addGymService;
            _deleteGymService = deleteGymService;
        }

        public IActionResult Index()
        {
            var gyms = _getGymsService.GetAll();
            return View(gyms);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        /*
        [HttpPost]
        public IActionResult Create(CreateGymDto vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = new CreateGymDto
            {
                Name = vm.Name,
                Code = vm.Code,
                Description = vm.Description,
                MobileNumber = vm.MobileNumber,
            };
            var result = _addGymService.Execute(dto);

            return Json(result);
        }
        */
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
                SubDomain = gym.SubDomain,
                BrandName = gym.BrandName,
                Description = gym.Description,
                Logo = gym.Logo,
                PhoneNumber = gym.PhoneNumber,
                MobileNumber = gym.MobileNumber,
                Email = gym.Email,
                Website = gym.Website,
                Province = gym.Province,
                Cities = gym.Cities,
                Address = gym.Address,
                PostalCode = gym.PostalCode,
                Latitude = gym.Latitude,
                Longitude = gym.Longitude,
                IsActive = gym.IsActive,
                SubscriptionExpireDate = gym.SubscriptionExpireDate,
                MaxMembers = gym.MaxMembers,
                AllowOnlineRegistration = gym.AllowOnlineRegistration,
                OtpExpireSeconds = gym.OtpExpireSeconds,
                MaxOtpRequestPerMinute = gym.MaxOtpRequestPerMinute
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(UpdateGymDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            _editGymService.Update(dto);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(long id)
        {
            _deleteGymService.Execute(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
