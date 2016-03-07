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

    public static class AltitudeConverter
    {
        private const double MetresToFeet = 3.28084;
        
        /// <summary>
        /// Converts Metres into Feet.
        /// </summary>
        public static double ConvertToFeet(this double metres)
        {
            return Math.Round(metres * MetresToFeet, 2, MidpointRounding.AwayFromZero);
        }

        private const double FeetToMetres = 3.28084;

        /// <summary>
        /// Converts Feet into Metres.
        /// </summary>
        public static double ConvertToMetres(this double feet)
        {
            return Math.Round(feet / FeetToMetres, 2, MidpointRounding.AwayFromZero);
        }
    }
}
