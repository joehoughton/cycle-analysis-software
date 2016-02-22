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

    public static class DistanceConverter
    {
        private const double MilesToKilometres = 0.621371192;

        /// <summary>
        /// Converts Kilometres into Miles.
        /// </summary>
        public static double ConvertToMiles(this double kilometres)
        {
            return Math.Round(kilometres * MilesToKilometres, 2, MidpointRounding.AwayFromZero);
        }
    }
}
