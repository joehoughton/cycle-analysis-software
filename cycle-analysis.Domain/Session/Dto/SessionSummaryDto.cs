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

    public class SessionSummaryDto
    {
        public SessionSummaryDto() { }
        public SessionSummaryDto(int id, string title, double totalDistance, double averageSpeed, double maximumSpeed,
                                double averageHeartRate, double minimumHeartRate, double maximumHeartRate, double averagePower,
                                double maximumPower, double averageAltitude, double maximumAltitude, double averageCadence,
                                double maximumCadence, double normalizedPower, double intensityFactor, int sessionId)
        {
            Id = id;
            Title = title;
            TotalDistance = totalDistance;
            AverageSpeed = averageSpeed;
            MaximumSpeed = maximumSpeed;
            AverageHeartRate = averageHeartRate;
            MinimumHeartRate = minimumHeartRate;
            MaximumHeartRate = maximumHeartRate;
            AveragePower = averagePower;
            MaximumPower = maximumPower;
            AverageAltitude = averageAltitude;
            MaximumAltitude = maximumAltitude;
            AverageCadence = averageCadence;
            MaximumCadence = maximumCadence;
            NormalizedPower = normalizedPower;
            IntensityFactor = intensityFactor;
            SessionId = sessionId;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public double TotalDistance { get; set; }
        public double AverageSpeed { get; set; }
        public double MaximumSpeed { get; set; }
        public double AverageHeartRate { get; set; }
        public double MinimumHeartRate { get; set; }
        public double MaximumHeartRate { get; set; }
        public double AveragePower { get; set; }
        public double MaximumPower { get; set; }
        public double AverageAltitude { get; set; }
        public double MaximumAltitude { get; set; }
        public double AverageCadence { get; set; }
        public double MaximumCadence { get; set; }
        public double NormalizedPower { get; set; }
        public double IntensityFactor { get; set; }
        public DateTime Date { get; set; }
        public int SessionId { get; set; }
    }
}
