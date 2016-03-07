﻿/****************************** Cycle Analysis ******************************\
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
    public class SessionSummaryRequestDto
    {
        public SessionSummaryRequestDto() { }
        public SessionSummaryRequestDto(int sessionId, int unit)
        {
            SessionId = sessionId;
            Unit = unit;
        }

        public int SessionId { get; set; }
        public int Unit { get; set; }
    }
}
