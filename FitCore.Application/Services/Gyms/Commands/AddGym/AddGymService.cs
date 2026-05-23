using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.IGym;
using FitCore.Application.ViewModels.Gyms;
using FitCore.Common.Dto;
namespace FitCore.Application.Services.Gyms.Commands
{
    public class AddGymService : IAddGymService
    {
        private readonly IDataBaseContext _context;

        public AddGymService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(CreateGymDto dto)
        {
            // بررسی خالی بودن فیلدها
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "نام باشگاه الزامی است"
                };
            }

            if (string.IsNullOrWhiteSpace(dto.Code))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کد باشگاه الزامی است"
                };
            }

            if (string.IsNullOrWhiteSpace(dto.MobileNumber))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "شماره موبایل الزامی است"
                };
            }

            // اعتبارسنجی فرمت موبایل
            if (!IsValidMobile(dto.MobileNumber))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "فرمت شماره موبایل صحیح نیست"
                };
            }

            var gym = new Domain.Entities.Gyms.Gyms
            {
                Name = dto.Name.Trim(),
                Code = dto.Code.Trim(),
                Description = dto.Description,
                MobileNumber = dto.MobileNumber.Trim(),
            };

            _context.Gyms.Add(gym);
            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "باشگاه با موفقیت ثبت شد"
            };
        }

        private bool IsValidMobile(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return false;

            mobile = mobile.Trim();

            // شماره موبایل ایران: 09xxxxxxxxx
            return System.Text.RegularExpressions.Regex.IsMatch(
                mobile,
                @"^09\d{9}$"
            );
        }
    }

}
