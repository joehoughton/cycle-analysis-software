﻿/****************************** Cycle Analysis ******************************\
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
    using System.Collections.Generic;

    public class SessionDataGraphDto
    {
        public SessionDataGraphDto()
        {
            HeartRates = new List<HeartRates>();
            Speeds = new List<Speeds>();
            Speeds = new List<Speeds>();
            Altitudes = new List<Altitudes>();
            Powers = new List<Powers>();
            Cadences = new List<Cadences>();
        }

        public double YAxisScale;
        public int XAxisScale;
        public int Interval;
        public List<HeartRates> HeartRates;
        public List<Speeds> Speeds;
        public List<Altitudes> Altitudes;
        public List<Powers> Powers;
        public List<Cadences> Cadences;
        public List<DetectedInterval> DetectedIntervals;
    }

    public class HeartRates
    {
        public double HeartRate { get; set; }
    }

    public class Speeds
    {
        public double Speed { get; set; }
    }

    public class Altitudes
    {
        public double Altitude { get; set; }
    }

    public class Powers
    {
        public double Power { get; set; }
    }

    public class Cadences
    {
        public double Cadence { get; set; }
    }
}
