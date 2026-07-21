using System;
using System.Globalization;

public static class PersianDateCalse
{
    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی
    /// </summary>
    /// <param name="miladiDate"></param>
    /// <returns></returns>
    public static string ToShamsi(DateTime miladiDate)
    {
        PersianCalendar pc = new PersianCalendar();
        int year = pc.GetYear(miladiDate);
        int month = pc.GetMonth(miladiDate);
        int day = pc.GetDayOfMonth(miladiDate);

        return $"{year}/{month.ToString("D2")}/{day.ToString("D2")}";
    }

    /// <summary>
    /// اختلاف دو تاریخ شمسی را به روز برمی‌گرداند
    /// </summary>
    public static int GetDaysDifference(string shamsiDate1, string shamsiDate2)
    {
        try
        {
            PersianCalendar pc = new PersianCalendar();

            var parts1 = shamsiDate1.Trim().Split('/');
            DateTime date1 = pc.ToDateTime(int.Parse(parts1[0]), int.Parse(parts1[1]), int.Parse(parts1[2]), 0, 0, 0, 0);

            var parts2 = shamsiDate2.Trim().Split('/');
            DateTime date2 = pc.ToDateTime(int.Parse(parts2[0]), int.Parse(parts2[1]), int.Parse(parts2[2]), 0, 0, 0, 0);

            TimeSpan diff = date1 - date2;
            return diff.Days;
        }
        catch
        {
            return -9999;
        }
    }

    /// <summary>
    /// محاسبه سن شمسی با قابلیت تعیین نحوه نمایش خروجی
    /// </summary>
    /// <param name="shamsiBirthDate">تاریخ تولد شمسی</param>
    /// <param name="mode">نحوه نمایش خروجی</param>
    /// <returns>خروجی بر اساس مود انتخاب شده</returns>
    public static string GetAge(string shamsiBirthDate, AgeDisplayMode mode = AgeDisplayMode.YearMonthDay)
    {
        try
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime today = DateTime.Now;

            // استخراج سال، ماه و روز امروز شمسی
            int tYear = pc.GetYear(today);
            int tMonth = pc.GetMonth(today);
            int tDay = pc.GetDayOfMonth(today);

            // تجزیه تاریخ تولد
            var parts = shamsiBirthDate.Trim().Split('/');
            if (parts.Length != 3) return "فرمت نامعتبر";

            int bYear = int.Parse(parts[0]);
            int bMonth = int.Parse(parts[1]);
            int bDay = int.Parse(parts[2]);

            // محاسبه روز (با در نظر گرفتن ماه‌های 29 و 30 روزه شمسی)
            int ageDays = tDay - bDay;
            if (ageDays < 0)
            {
                tMonth--;
                int prevMonth = tMonth == 0 ? 12 : tMonth;
                int prevYear = tMonth == 0 ? tYear - 1 : tYear;
                ageDays += pc.GetDaysInMonth(prevYear, prevMonth);
            }

            // محاسبه ماه
            int ageMonths = tMonth - bMonth;
            if (ageMonths < 0)
            {
                ageMonths += 12;
                tYear--;
            }

            // محاسبه سال
            int ageYears = tYear - bYear;

            // بررسی تاریخ آینده
            if (ageYears < 0)
                return "تاریخ نامعتبر (آینده)";

            // تبدیل تاریخ شمسی به میلادی برای محاسبه دقیق کل روزها (Mode 4)
            DateTime birthDateMiladi = pc.ToDateTime(bYear, bMonth, bDay, 0, 0, 0, 0);
            int totalDays = (int)(today - birthDateMiladi).TotalDays;


            // اعمال منطق بر اساس Mode درخواستی
            switch (mode)
            {
                case AgeDisplayMode.Year:
                    return ageYears.ToString();

                case AgeDisplayMode.YearMonthDay:
                    return $"{ageYears} سال، {ageMonths} ماه و {ageDays} روز";

                case AgeDisplayMode.Compact:
                    return $"{ageYears}/{ageMonths.ToString("D2")}/{ageDays.ToString("D2")}";

                case AgeDisplayMode.TotalDays:
                    return totalDays.ToString("N0"); // جدا کننده هزارگان اضافه میکند (مثلا 11,850)

                case AgeDisplayMode.Friendly:
                    if (ageYears == 0 && ageMonths == 0) return $"{ageDays} روزه";
                    if (ageYears == 0) return $"{ageMonths} ماهه";
                    return $"{ageYears} ساله";

                case AgeDisplayMode.ShortFriendly:
                    if (ageYears == 0) return $"{ageMonths} ماه و {ageDays} روز";
                    if (ageMonths == 0 && ageDays == 0) return $"{ageYears} سال";
                    return $"{ageYears} سال و {ageMonths} ماه";

                default:
                    return "مدت نامعتبر است";
            }
        }
        catch
        {
            return "خطا در پردازش";
        }
    }

    // تعریف حالت‌های مختلف خروجی (Enum)
    public enum AgeDisplayMode
    {
        Year = 1,               // فقط عدد سال (مثلا: 32)
        YearMonthDay = 2,       // سال، ماه، روز متنی (مثلا: 32 سال، 2 ماه و 5 روز)
        Compact = 3,            // فرمت فشرده (مثلا: 32/02/05)
        TotalDays = 4,          // کل روزهای عمر (مثلا: 11850 روز)
        Friendly = 5,           // متن دوستانه و کوتاه (مثلا: 32 ساله)
        ShortFriendly = 6       // سال و ماه به صورت متن (مثلا: 32 سال و 2 ماه)
    }



    /// <summary>
    /// تعداد روزهای یک ماه شمسی
    /// </summary>
    public static int GetDaysInMonth(int year, int month)
    {
        PersianCalendar pc = new PersianCalendar();
        return pc.GetDaysInMonth(year, month);
    }

    /// <summary>
    /// آیا سال شمسی کبیسه است؟
    /// </summary>
    public static bool IsLeapYear(int year)
    {
        PersianCalendar pc = new PersianCalendar();
        return pc.IsLeapYear(year);
    }
}