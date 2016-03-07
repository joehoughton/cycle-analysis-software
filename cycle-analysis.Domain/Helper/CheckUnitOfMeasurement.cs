namespace cycle_analysis.Domain.Helper
{
    public static class CheckUnitOfMeasurement
    {
        /// <summary>
        /// Returns value true if the measurement bit is set to 0 (metric) or false if the measurement bit is set to 1 (imperial).
        /// </summary>
        public static bool IsMetric(this string sMode)
        {
            var measurementBit =  sMode.ToCharArray()[7]; // get the 8th measurement bit
            var isMetric = measurementBit.ToString() == "0";

            return isMetric;
        }
    }
}
