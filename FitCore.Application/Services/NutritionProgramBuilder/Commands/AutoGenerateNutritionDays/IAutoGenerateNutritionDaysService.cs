using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.NutritionProgram.NutritionProgramDay;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.NutritionProgramBuilder.Commands.AutoGenerateNutritionDays
{
    public interface IAutoGenerateNutritionDaysService
    {
        ResultDto Execute(AutoGenerateNutritionDaysDto request);
    }
    public class AutoGenerateNutritionDaysService : IAutoGenerateNutritionDaysService
    {
        private readonly IDataBaseContext _context;

        public AutoGenerateNutritionDaysService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(AutoGenerateNutritionDaysDto request)
        {
            if (request == null || request.NutritionProgramId <= 0)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "شناسه برنامه غذایی معتبر نیست."
                };
            }

            var program = _context.NutritionPrograms
                .FirstOrDefault(x => x.Id == request.NutritionProgramId);

            if (program == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "برنامه غذایی موردنظر یافت نشد."
                };
            }

            if (string.IsNullOrWhiteSpace(program.StartDate) || string.IsNullOrWhiteSpace(program.EndDate))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "تاریخ شروع یا پایان برنامه غذایی مشخص نیست."
                };
            }

            var startDate = ParsePersianDate(program.StartDate);
            var endDate = ParsePersianDate(program.EndDate);

            if (startDate == null || endDate == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "فرمت تاریخ شروع یا پایان معتبر نیست. فرمت صحیح: yyyy/MM/dd"
                };
            }

            if (endDate.Value.Date < startDate.Value.Date)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "تاریخ پایان نمی‌تواند قبل از تاریخ شروع باشد."
                };
            }

            var hasAnyDay = _context.NutritionProgramDays
                .Any(x => x.NutritionProgramId == request.NutritionProgramId);

            if (hasAnyDay)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "برای این برنامه قبلاً روز ثبت شده است. برای جلوگیری از ایجاد رکورد تکراری، ابتدا روزهای قبلی را حذف کنید."
                };
            }

            var currentDate = startDate.Value.Date;
            var finalDate = endDate.Value.Date;

            int dayNumber = 1;
            int createdCount = 0;

            while (currentDate <= finalDate)
            {
                string persianDateText = ToPersianDateString(currentDate);
                string persianDayName = GetPersianDayName(currentDate.DayOfWeek);

                var day = new NutritionProgramDay
                {
                    NutritionProgramId = request.NutritionProgramId,
                    DayNumber = dayNumber,
                    Title = $"{persianDayName} - {persianDateText}"
                };

                _context.NutritionProgramDays.Add(day);

                createdCount++;
                dayNumber++;
                currentDate = currentDate.AddDays(1);
            }

            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = $"{createdCount} روز با موفقیت برای برنامه غذایی ایجاد شد."
            };
        }

        private DateTime? ParsePersianDate(string persianDate)
        {
            if (string.IsNullOrWhiteSpace(persianDate))
                return null;

            persianDate = NormalizeDigits(persianDate.Trim());

            var parts = persianDate.Split('/');

            if (parts.Length != 3)
                return null;

            if (!int.TryParse(parts[0], out int year))
                return null;

            if (!int.TryParse(parts[1], out int month))
                return null;

            if (!int.TryParse(parts[2], out int day))
                return null;

            try
            {
                var pc = new PersianCalendar();
                return pc.ToDateTime(year, month, day, 0, 0, 0, 0);
            }
            catch
            {
                return null;
            }
        }

        private string ToPersianDateString(DateTime date)
        {
            var pc = new PersianCalendar();

            int year = pc.GetYear(date);
            int month = pc.GetMonth(date);
            int day = pc.GetDayOfMonth(date);

            return $"{year:0000}/{month:00}/{day:00}";
        }

        private string GetPersianDayName(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return "شنبه";

                case DayOfWeek.Sunday:
                    return "یکشنبه";

                case DayOfWeek.Monday:
                    return "دوشنبه";

                case DayOfWeek.Tuesday:
                    return "سه‌شنبه";

                case DayOfWeek.Wednesday:
                    return "چهارشنبه";

                case DayOfWeek.Thursday:
                    return "پنجشنبه";

                case DayOfWeek.Friday:
                    return "جمعه";

                default:
                    return "";
            }
        }

        private string NormalizeDigits(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return input
                .Replace("۰", "0")
                .Replace("۱", "1")
                .Replace("۲", "2")
                .Replace("۳", "3")
                .Replace("۴", "4")
                .Replace("۵", "5")
                .Replace("۶", "6")
                .Replace("۷", "7")
                .Replace("۸", "8")
                .Replace("۹", "9")
                .Replace("٠", "0")
                .Replace("١", "1")
                .Replace("٢", "2")
                .Replace("٣", "3")
                .Replace("٤", "4")
                .Replace("٥", "5")
                .Replace("٦", "6")
                .Replace("٧", "7")
                .Replace("٨", "8")
                .Replace("٩", "9");
        }
    }


    public class AutoGenerateNutritionDaysDto
    {
        public long NutritionProgramId { get; set; }
    }
}
