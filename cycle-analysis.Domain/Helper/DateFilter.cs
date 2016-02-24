namespace cycle_analysis.Domain.Helper
{
    using System;
    using System.Collections.Generic;

    using cycle_analysis.Domain.SessionData.Dtos;
    using cycle_analysis.Domain.SessionData.Models;

    public static class DateFilter
    {
        /// <summary>
        /// Takes a starting date and adds the minimum and maximum amount of seconds to it.
        /// Returns all dates that are within range of the minimum and maximum dates.
        /// DateTime.Compare returns -1 if date parameter 1 is less than than date paramater 2
        /// DateTime.Compare returns 0 if date parameter 1 is equal to date parameter 2
        /// DateTime.Compare returns 1 if date parameter 1 is greater than date parameter 2
        /// </summary>
        public static List<DateTime> FilterDates(this List<DateTime> dates, DateTime startDate, double minimumStartTime, double maximumStartTime)
        {
            var minimumDate = startDate.AddSeconds(minimumStartTime);
            var maximumDate = startDate.AddSeconds(maximumStartTime);

            var filteredDates = new List<DateTime>();

            foreach (var date in dates)
            {
                if (DateTime.Compare(date, minimumDate) >= 0 && DateTime.Compare(date, maximumDate) < 1)
                {
                    filteredDates.Add(date);
                }
            }

            return filteredDates;
        }
        // ToDo: Make method generic
        public static List<SessionDataDto> FilterListDates(this List<SessionDataDto> sessionDataDto, DateTime startDate, double minimumStartTime, double maximumStartTime)
        {
            var minimumDate = startDate.AddSeconds(minimumStartTime);
            var maximumDate = startDate.AddSeconds(maximumStartTime);

            var filteredObjects = new List<SessionDataDto>();

            foreach (var sessionData in sessionDataDto)
            {
                if (DateTime.Compare(sessionData.Date, minimumDate) >= 0 && DateTime.Compare(sessionData.Date, maximumDate) < 1)
                {
                    filteredObjects.Add(sessionData);
                }
            }

            return filteredObjects;
        }
    }
}
