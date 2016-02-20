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
    using System.Collections.Generic;
    using cycle_analysis.Domain.SessionData.Dtos;

    public class SessionDto
    {
        public SessionDto() { }
        public SessionDto(int id, double softwareVersion, int monitorVersion, int sMode, DateTime startTime, DateTime length, DateTime date, int interval, int upper1, int lower1, int athleteId, List<SessionDataDto> sessionData)
        {
            Id = id;
            SoftwareVersion = softwareVersion;
            MonitorVersion = monitorVersion;
            SMode = sMode;
            StartTime = startTime;
            Length = length;
            Date = date;
            Interval = interval;
            Upper1 = upper1;
            Lower1 = lower1;
            AthleteId = athleteId;
            SessionData = sessionData;
        }

        public int Id { get; set; }
        public double SoftwareVersion { get; set; }
        public int MonitorVersion { get; set; }
        public int SMode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Length { get; set; }
        public DateTime Date { get; set; }
        public int Interval { get; set; }
        public int Upper1 { get; set; }
        public int Lower1 { get; set; }
        public int AthleteId { get; set; }
        public List<SessionDataDto> SessionData { get; set; }
    }
}
