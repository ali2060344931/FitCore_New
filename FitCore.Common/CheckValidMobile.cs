using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Common
{

    /// <summary>
    /// کنترل صحیح بودن شماره موبایل
    /// </summary>
    public static class CheckValidMobile
    {

        public static  bool IsValidMobile(string mobile)
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
