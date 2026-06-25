using System;
using System.Globalization;

public static class DateConverterMlladiToShamsi
{
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
    /// <param name="shamsiDate1">تاریخ اول (مثلا تاریخ پایان)</param>
    /// <param name="shamsiDate2">تاریخ دوم (مثلا تاریخ امروز)</param>
    /// <returns>اگر مثبت باشد یعنی تاریخ اول جلوتر است، اگر منفی باشد یعنی گذشته است</returns>
    public static int GetDaysDifference(string shamsiDate1, string shamsiDate2)
    {
        try
        {
            PersianCalendar pc = new PersianCalendar();

            // تجزیه تاریخ اول
            var parts1 = shamsiDate1.Trim().Split('/');
            DateTime date1 = pc.ToDateTime(int.Parse(parts1[0]), int.Parse(parts1[1]), int.Parse(parts1[2]), 0, 0, 0, 0);

            // تجزیه تاریخ دوم
            var parts2 = shamsiDate2.Trim().Split('/');
            DateTime date2 = pc.ToDateTime(int.Parse(parts2[0]), int.Parse(parts2[1]), int.Parse(parts2[2]), 0, 0, 0, 0);

            // محاسبه اختلاف
            TimeSpan diff = date1 - date2;
            return diff.Days;
        }
        catch
        {
            // در صورت ناقص بودن فرمت تاریخ، خطا را مدیریت می‌کنیم
            return -9999;
        }
    }
}