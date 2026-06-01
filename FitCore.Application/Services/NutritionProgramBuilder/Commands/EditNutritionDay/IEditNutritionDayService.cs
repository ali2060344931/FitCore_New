using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionProgramBuilder.Commands.EditNutritionDay
{
    public interface IEditNutritionDayService
    {
        ResultDto Execute(EditNutritionDayDto request);
    }

    public class EditNutritionDayService : IEditNutritionDayService
    {
        private readonly IDataBaseContext _context;

        public EditNutritionDayService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(EditNutritionDayDto request)
        {
            if (request == null || request.Id <= 0)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "اطلاعات ورودی نامعتبر است"
                };
            }

            var day = _context.NutritionProgramDays.Find(request.Id);

            if (day == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "روز موردنظر یافت نشد"
                };
            }

            // اگر خواستی شماره روز تکراری نباشد، این گارد را نگه دار
            if (request.DayNumber.HasValue)
            {
                var duplicateDayNumber = _context.NutritionProgramDays
                    .Any(x => x.Id != request.Id
                           && x.NutritionProgramId == day.NutritionProgramId
                           && x.DayNumber == request.DayNumber.Value);

                if (duplicateDayNumber)
                {
                    return new ResultDto
                    {
                        IsSuccess = false,
                        Message = "شماره این روز قبلاً در همین برنامه ثبت شده است"
                    };
                }
            }

            day.Title = request.Title?.Trim();

            if (request.DayNumber.HasValue)
            {
                day.DayNumber = request.DayNumber.Value;
            }

            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "روز با موفقیت ویرایش شد"
            };
        }
    }

    public class EditNutritionDayDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "عنوان روز الزامی است")]
        [MaxLength(200, ErrorMessage = "عنوان روز نمی‌تواند بیشتر از 200 کاراکتر باشد")]
        public string Title { get; set; }

        public int? DayNumber { get; set; }
    }
}
