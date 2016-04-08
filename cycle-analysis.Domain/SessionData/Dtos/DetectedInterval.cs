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
    public class DetectedInterval
    {
        public DetectedInterval(decimal startTime, decimal finishTime, double averagePower, bool isRest)
        {
            StartTime = startTime;
            FinishTime = finishTime;
            AveragePower = averagePower;
            IsRest = isRest;
        }

        public decimal StartTime { get; set; }
        public decimal FinishTime { get; set; }
        public double AveragePower { get; set; }
        public bool IsRest { get; set; }
    }
}
