/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Domain.Session.Dto
{
    using System;

    public class SessionCalendarDto
    {
        public SessionCalendarDto() { }
        public SessionCalendarDto(int id, string title, DateTime startDate, DateTime endDate, int athleteId)
        {
            Id = id;
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            AthleteId = athleteId;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AthleteId { get; set; }
    }
}