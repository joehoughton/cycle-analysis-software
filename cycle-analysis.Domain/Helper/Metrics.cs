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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using cycle_analysis.Domain.Session.Dto;

    public static class Metrics
    {
        /// <summary>
        /// Calculates moving averages over a specified period
        /// Returns list of moving averages
        /// </summary>
        public static List<double> CalculateMovingAverages(this List<double> data, int period)
        {
            var movingAverages = Enumerable
                .Range(0, data.Count - period + 1)
                .Select(n => data.Skip(n).Take(period).Average())
                .ToList();

            return movingAverages;
        }

        /// <summary>
        /// Raises all items in list to the specified power
        /// </summary>
        public static List<double> ToPower(this List<double> array, int p)
        {
            for (var x = 0; x < array.Count; x++)
            {
                array[x] = Math.Pow(array[x], p);
            }

            return array;
        }

        /// <summary>
        /// Finds the nth root of a number
        /// </summary>
        public static double NthRoot(this double number, int n)
        {
            return Math.Pow(number, 1.0 / n);
        }

        /// <summary>
        /// Calculates Normalized Power for session data starting at 30 seconds
        /// </summary>
        public static double CalculateNormalizedPower(this SessionDto session)
        {
            var interval = session.Interval;
            var powers = new List<double>();

            for (var x = 0; x < session.SessionData.Count; x++)
            {
                if (((x + 1) * interval) >= 30) // start rolling average at 30 seconds
                {
                    powers.Add(session.SessionData[x].Power);
                }
            }

            if (!powers.Any() || powers.Count < 30) // 30 powers are required to calculate moving averages
            {
                return 0;
            }

            // calculate a rolling 30 second average of the preceding time points after 30 seconds
            var movingAverages = powers.CalculateMovingAverages(30 / interval);

            // raise all the moving averages to the fourth power
            var averagesToFourthPower = movingAverages.ToPower(4);

            // find the average of values raised to fourth power
            var averageOfFourthPower = averagesToFourthPower.Average();

            // take the fourth root of the average values raised to the fourth power
            var normalizedPower =  Math.Round(averageOfFourthPower.NthRoot(4), 2, MidpointRounding.AwayFromZero);

            return normalizedPower;
        }

        /// <summary>
        /// Intensity Factor is the ratio of the normalized power to Functional Threshold Power
        /// </summary>
        public static double CalculateIntensityFactor(this double normalizedPower, double functionalThresholdPower)
        {
            return Math.Round(normalizedPower / functionalThresholdPower, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Training Stress Score = (s x NP x IF) / (FTP x 3,600) x 100
        /// </summary>
        public static double CalculateTrainingStressScore(double seconds, double normalizedPower, double intensityFactor, double functionalThresholdPower)
        {
            return Math.Round(((seconds * normalizedPower * intensityFactor) / (functionalThresholdPower * 3600)) * 100, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Generate TSS Status
        /// </summary>
        public static string TrainingStressScoreStatus(double trainingStressScore)
        {
            var status = "";

            if (trainingStressScore < 150)
            {
                status = "Recovery generally complete by the following day";
            }
            else if (trainingStressScore >= 150 && trainingStressScore <= 300)
            {
                status = "Some residual fatigue may be present the next day, but gone by the second day";
            }
            else if (trainingStressScore >= 150 && trainingStressScore <= 300)
            {
                status = "Some residual fatigue may be present even after 2 days";
            }
            else
            {
                status = "Residual fatigue lasting several days is likely";
            }
            return status;
        }

    }
}
