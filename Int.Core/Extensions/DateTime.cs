//
// DateTime.cs
//
// Author:
//      Songurov <f.songurov@software-dep.net>
//
// Copyright (c) 2016 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Int.Core.Extensions
{
    /// <summary>
    ///     Extensions.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        ///     The epoch.
        /// </summary>
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1);

        /// <summary>
        ///     Gets the now.
        /// </summary>
        /// <value>The now.</value>
        public static long Now => (long) (DateTime.UtcNow - Epoch).TotalSeconds;

        /// <summary>
        ///     Tos the epoch.
        /// </summary>
        /// <returns>The epoch.</returns>
        /// <param name="dt">Dt.</param>
        public static long ToEpoch(this DateTime dt)
        {
            return (long) (dt.ToUniversalTime() - Epoch).TotalSeconds;
        }

        /// <summary>
        ///     Starts the of day.
        /// </summary>
        /// <returns>The of day.</returns>
        /// <param name="dateTime">Date time.</param>
        public static DateTime StartOfDay(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        ///     Ends the of day.
        /// </summary>
        /// <returns>The of day.</returns>
        /// <param name="dateTime">Date time.</param>
        public static DateTime EndOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
        }

        /// <summary>
        ///     Firsts the day of month.
        /// </summary>
        /// <returns>The day of month.</returns>
        /// <param name="dateTime">Date time.</param>
        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        ///     Lasts the day of month.
        /// </summary>
        /// <returns>The day of month.</returns>
        /// <param name="dateTime">Date time.</param>
        public static DateTime LastDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }

        /// <summary>
        ///     Firsts the day of week.
        /// </summary>
        /// <returns>The day of week.</returns>
        /// <param name="dateTime">Date time.</param>
        public static DateTime FirstDayOfWeek(this DateTime dateTime)
        {
            var firstDayInWeek = dateTime.Date;

            while (firstDayInWeek.DayOfWeek != DayOfWeek.Monday)
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            return firstDayInWeek.StartOfDay();
        }

        /// <summary>
        ///     Lasts the day of week.
        /// </summary>
        /// <returns>The day of week.</returns>
        /// <param name="dateTime">Date time.</param>
        public static DateTime LastDayOfWeek(this DateTime dateTime)
        {
            var lastDayInWeek = dateTime.Date;

            while (lastDayInWeek.DayOfWeek != DayOfWeek.Sunday)
                lastDayInWeek = lastDayInWeek.AddDays(1);
            return lastDayInWeek.StartOfDay();
        }

        /// <summary>
        ///     Ends the of last day of month.
        /// </summary>
        /// <returns>The of last day of month.</returns>
        /// <param name="dateTime">Date time.</param>
        public static DateTime EndOfLastDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month), 23,
                59,
                59);
        }

        /// <summary>
        ///     Ises the in period.
        /// </summary>
        /// <returns><c>true</c>, if in period was ised, <c>false</c> otherwise.</returns>
        /// <param name="dateTime">Date time.</param>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        public static bool IsInPeriod(this DateTime dateTime, DateTime startDate, DateTime endDate)
        {
            if (dateTime >= startDate && dateTime <= endDate)
                return true;
            return false;
        }

        /// <summary>
        ///     Ises the out of period.
        /// </summary>
        /// <returns><c>true</c>, if out of period was ised, <c>false</c> otherwise.</returns>
        /// <param name="dateTime">Date time.</param>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        public static bool IsOutOfPeriod(this DateTime dateTime, DateTime startDate, DateTime endDate)
        {
            if (dateTime < startDate || dateTime > endDate)
                return true;
            return false;
        }

        /// <summary>
        ///     Adds the time.
        /// </summary>
        /// <returns>The time.</returns>
        /// <param name="date">Date.</param>
        /// <param name="hour">Hour.</param>
        /// <param name="minutes">Minutes.</param>
        public static DateTime AddTime(this DateTime date, int hour, int minutes)
        {
            return date + new TimeSpan(hour, minutes, 0);
        }

        /// <summary>
        ///     Adds the time.
        /// </summary>
        /// <returns>The time.</returns>
        /// <param name="date">Date.</param>
        /// <param name="hour">Hour.</param>
        /// <param name="minutes">Minutes.</param>
        /// <param name="seconds">Seconds.</param>
        public static DateTime AddTime(this DateTime date, int hour, int minutes, int seconds)
        {
            return date + new TimeSpan(hour, minutes, seconds);
        }

        /// <summary>
        ///     Tos the unix timestamp.
        /// </summary>
        /// <returns>The unix timestamp.</returns>
        /// <param name="date">Date.</param>
        public static double ToUnixTimestamp(this DateTime date)
        {
            var utcTime = TimeZoneInfo.ConvertTime(date, TimeZoneInfo.Utc);
            var localTime = TimeZoneInfo.ConvertTime(date, TimeZoneInfo.Local);
            var localTimes = TimeZoneInfo.ConvertTime(Epoch, TimeZoneInfo.Local);
            date = date.AddHours((utcTime - localTime).Hours);
            return (date - localTimes).TotalSeconds;
        }


        public static double ToUnixTimestampStatic(this DateTime date)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0);
            var unixTimeSpan = date - unixEpoch;

            return (long) unixTimeSpan.TotalSeconds;
        }

        /// <summary>
        ///     Converts the given date value to epoch time.
        /// </summary>
        public static long ToEpochTime(this DateTime dateTime)
        {
            var date = dateTime.ToUniversalTime();
            var ticks = date.Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks;
            var ts = ticks / TimeSpan.TicksPerSecond;
            return ts;
        }

        /// <summary>
        ///     Tos the oracle sql date.
        /// </summary>
        /// <returns>The oracle sql date.</returns>
        /// <param name="dateTime">Date time.</param>
        public static string ToOracleSqlDate(this DateTime dateTime)
        {
            return $"to_date('{dateTime:dd.MM.yyyy HH:mm:ss}','dd.mm.yyyy hh24.mi.ss')";
        }

        /// <summary>
        ///     Ises the weekend.
        /// </summary>
        /// <returns><c>true</c>, if weekend was ised, <c>false</c> otherwise.</returns>
        /// <param name="d">D.</param>
        public static bool IsWeekend(this DayOfWeek d)
        {
            return !d.IsWeekend();
        }

        /// <summary>
        ///     Ises the weekday.
        /// </summary>
        /// <returns><c>true</c>, if weekday was ised, <c>false</c> otherwise.</returns>
        /// <param name="d">D.</param>
        public static bool IsWeekday(this DateTime d)
        {
            return d.DayOfWeek.IsWeekday();
        }

        /// <summary>
        ///     Ises the weekend.
        /// </summary>
        /// <returns><c>true</c>, if weekend was ised, <c>false</c> otherwise.</returns>
        /// <param name="d">D.</param>
        public static bool IsWeekend(this DateTime d)
        {
            return d.DayOfWeek.IsWeekend();
        }

        /// <summary>
        ///     Ises the weekday.
        /// </summary>
        /// <returns><c>true</c>, if weekday was ised, <c>false</c> otherwise.</returns>
        /// <param name="d">D.</param>
        public static bool IsWeekday(this DayOfWeek d)
        {
            switch (d)
            {
                case DayOfWeek.Sunday:
                    return false;
                case DayOfWeek.Saturday:
                    return false;

                default:
                    return true;
            }
        }

        /// <summary>
        ///     Ises the between dt, startDate, endDate and compareTime.
        /// </summary>
        /// <returns>The <see cref="T:System.Boolean" />.</returns>
        /// <param name="dt">Dt.</param>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        /// <param name="compareTime">If set to <c>true</c> compare time.</param>
        public static bool IsBetween(this DateTime dt, DateTime startDate, DateTime endDate, bool compareTime = false)
        {
            return compareTime
                ? dt >= startDate && dt <= endDate
                : dt.Date >= startDate.Date && dt.Date <= endDate.Date;
        }

        /// <summary>
        ///     Ises the overlap.
        /// </summary>
        /// <returns><c>true</c>, if overlap was ised, <c>false</c> otherwise.</returns>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        /// <param name="intersectingStartDate">Intersecting start date.</param>
        /// <param name="intersectingEndDate">Intersecting end date.</param>
        public static bool IsOverlap(this DateTime startDate, DateTime endDate, DateTime intersectingStartDate,
            DateTime intersectingEndDate)
        {
            return intersectingEndDate >= startDate && intersectingStartDate <= endDate;
        }

        /// <summary>
        ///     Ises the leap day.
        /// </summary>
        /// <returns><c>true</c>, if leap day was ised, <c>false</c> otherwise.</returns>
        /// <param name="date">Date.</param>
        public static bool IsLeapDay(this DateTime date)
        {
            return date.Month == 2 && date.Day == 29;
        }

        /// <summary>
        ///     Ises the leap year.
        /// </summary>
        /// <returns><c>true</c>, if leap year was ised, <c>false</c> otherwise.</returns>
        /// <param name="value">Value.</param>
        public static bool IsLeapYear(this DateTime value)
        {
            return DateTime.DaysInMonth(value.Year, 2) == 29;
        }

        public static DateTime[] ToStartEndUnixTime(this string value)
        {
            var date = DateTime.ParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            var date1 = date.StartOfDay();
            var date2 = date.EndOfDay();
            return new[] {date1, date2};
        }

        public static bool IsValuesForEntireDay(this DateTime date, DateTime time)
        {
            return date.Year == time.Year &&
                   date.Month == time.Month &&
                   date.Day == time.Day &&
                   date.Hour == 0 && date.Minute == 0 &&
                   time.Hour == 23 && time.Minute == 59;
        }


        public static string TimeAgo(this DateTime yourDate, IList<Tuple<string[]>> dateTranslated = null)
        {
            const int second = 1;
            const int minute = 60 * second;
            const int hour = 60 * minute;
            const int day = 24 * hour;
            const int month = 30 * day;

            if (dateTranslated.IsNull())
                dateTranslated = new List<Tuple<string[]>>
                {
                    Tuple.Create(new[] {"second ago", "seconds ago"}),
                    Tuple.Create(new[] {"minute ago", "minutes ago"}),
                    Tuple.Create(new[] {"hour ago", "hours ago"}),
                    Tuple.Create(new[] {"day ago", "days ago"}),
                    Tuple.Create(new[] {"month ago", "months ago"}),
                    Tuple.Create(new[] {"year ago", "years ago"})
                };

            var ts = new TimeSpan(DateTime.Now.Ticks - yourDate.Ticks);
            var delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * minute)
                return ts.Seconds == 1
                    ? "1 " + dateTranslated?.ElementAtOrDefault(0)?.Item1[0]
                    : ts.Seconds + " " + dateTranslated?.ElementAtOrDefault(0)?.Item1[1];

            if (delta < 2 * minute)
                return "1 " + dateTranslated?.ElementAtOrDefault(1)?.Item1[0];

            if (delta < 45 * minute)
                return ts.Minutes + " " + dateTranslated?.ElementAtOrDefault(1)?.Item1[1];

            if (delta < 90 * minute)
                return "1 " + dateTranslated?.ElementAtOrDefault(2)?.Item1[0];

            if (delta < 24 * hour)
                return ts.Hours + " " + dateTranslated?.ElementAtOrDefault(2)?.Item1[1];

            if (delta < 48 * hour)
                return dateTranslated?.ElementAtOrDefault(3)?.Item1[0];

            if (delta < 30 * day)
                return ts.Days + " " + dateTranslated?.ElementAtOrDefault(3)?.Item1[1];

            if (delta < 12 * month)
            {
                var months = Convert.ToInt32(Math.Floor((double) ts.Days / 30));
                return months <= 1
                    ? "1 " + dateTranslated?.ElementAtOrDefault(4)?.Item1[0]
                    : months + " " + dateTranslated?.ElementAtOrDefault(4)?.Item1[1];
            }
            var years = Convert.ToInt32(Math.Floor((double) ts.Days / 365));
            return years <= 1
                ? "1 " + dateTranslated?.ElementAtOrDefault(5)?.Item1[0]
                : years + " " + dateTranslated?.ElementAtOrDefault(5)?.Item1[1];
        }
    }
}