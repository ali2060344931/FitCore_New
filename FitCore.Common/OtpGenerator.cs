using System;
using System.Security.Cryptography;

namespace FitCore.Common
{
    /// <summary>
    /// جهت ایجاد کد پیامکی برای ارسال به کاربران
    /// </summary>
    public static class OtpGenerator
    {
        public static string Generate()
        {
            var bytes = new byte[4];
            RandomNumberGenerator.Fill(bytes);

            int value = BitConverter.ToInt32(bytes, 0);
            value = Math.Abs(value % 900000) + 100000;

            return value.ToString();
        }
    }
}
