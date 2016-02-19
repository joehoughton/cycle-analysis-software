/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Domain.Helper
{
    using System;
    using System.Globalization;

    public static class DateFormat
    {
        public static DateTime FormatDate(this string date)
        {
            string day = date.Substring(6, 2);
            string month = date.Substring(4, 2);
            string year = date.Substring(0, 4);

            DateTime result = DateTime.ParseExact(day + month + year, "ddMMyyyy", CultureInfo.InvariantCulture);

            return result;
        }

        public static DateTime FormatTime(this string startTime)
        {
            var convertedTime = Convert.ToDateTime(startTime); // 1754 earliest date possible to save
            var convertedStartTime = new DateTime(1754, 01, 01, convertedTime.Hour, convertedTime.Minute, convertedTime.Second, convertedTime.Millisecond);

            return convertedStartTime;
        }
    }
}
