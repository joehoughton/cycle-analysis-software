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

    public class SessionDto
    {
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
    }
}
