/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Domain.SessionData.Dtos
{
    using System;

    public class SessionDataDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Row { get; set; }
        public double HeartRate { get; set; }
        public double Speed { get; set; }
        public double Cadence { get; set; }
        public double Altitude { get; set; }
        public double Power { get; set; }
        public int SessionId { get; set; }
    }
}
