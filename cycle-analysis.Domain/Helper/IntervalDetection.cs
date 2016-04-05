/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Domain.Helper
{
    using System.Collections.Generic;
    using System.Linq;
    using cycle_analysis.Domain.Session.Dto;
    using cycle_analysis.Domain.SessionData.Dtos;

    public static class IntervalDetection
    {
        public static List<DetectedInterval> DetectIntervals(this SessionDto session)
        {
            var sessionData = session.SessionData;
            var interval = session.Interval;
            var potentialIntervalStart = new SessionDataDto();
            var detectedIntervalEnd = new SessionDataDto();
            double potentialIntervalAverage = 0;
            var detectedIntervals = new List<DetectedInterval>();

            for (var x = 0; x < sessionData.Count; x++)
            {
                bool potentialIntervalDetected = false;
                bool intervalDetected = false;

                for (var p = x; p < sessionData.Count; p++)
                {
                    // get average of proceeding 14 seconds of powers - time taken for rider to reach maximum power
                    var currentPowers = new List<double>();
                    var proceedingPowers = new List<double>();

                    for (var i = 0; i < 14; i++)
                    {
                        if (p + (i + 1) < sessionData.Count)
                        {
                            if (sessionData[p + i].Power == 0) // rider must be applying power for next 14 seconds
                            {
                                break;
                            }
                            currentPowers.Add(sessionData[p + i].Power); // get power for the next 14 seconds
                            proceedingPowers.Add(sessionData[(p + 1) + i].Power); // get power for the next 14 seconds starting at current power +1
                        }
                    }

                    if (currentPowers.Count == 0) // no powers added to the last - last detected power was 0
                    {
                        break;
                    }

                    var currentPowersAverage = currentPowers.Average();
                    var proceedingPowersAverage = proceedingPowers.Average();

                    // check for potential interval
                    if (currentPowersAverage < proceedingPowersAverage)
                    {
                        if (!potentialIntervalDetected)
                        {
                            potentialIntervalStart = sessionData[p];
                            potentialIntervalDetected = true;
                        }
                    }
                    else // possible that cyclist built up speed and reached interval speed to maintain
                    {
                        var potentialIntervalPowerToMaintain = sessionData[p].Power;

                        var percentage = ((potentialIntervalPowerToMaintain * 40) / 100);
                        var minimumPowerRange = potentialIntervalPowerToMaintain - percentage;
                        var maximumPowerRange = potentialIntervalPowerToMaintain + percentage;
                        var minimumIntervalDuration = 10; // interval power must be maintained for atleast 10 seconds

                        var timer = 0;
                        var counter = 1;
                        for (var q = p; q < sessionData.Count; q++)
                        {
                            if (sessionData[q].Power > minimumPowerRange && sessionData[q].Power < maximumPowerRange)
                            {
                                timer += interval * counter;
                            }
                            else
                            {
                                if (timer > minimumIntervalDuration)
                                {
                                    intervalDetected = true;
                                    detectedIntervalEnd = sessionData[q];
                                    detectedIntervals.Add(new DetectedInterval(potentialIntervalStart.Row * interval, detectedIntervalEnd.Row * interval));
                                }
                                break;
                            }
                        }
                    }

                    if (intervalDetected)
                    {
                        x = detectedIntervalEnd.Row * interval; // start detecting new interval at the end of the detected interval
                        potentialIntervalStart = sessionData[detectedIntervalEnd.Row * interval];
                        break;
                    }
                }
            }
            return detectedIntervals;
        }
    }
}
