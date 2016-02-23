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
    }

    public class HeartRates
    {
        public double HeartRate { get; set; }
        public double Date { get; set; }
    }

    public class Speeds
    {
        public double Speed { get; set; }
        public double Date { get; set; }
    }

    public class Altitudes
    {
        public double Altitude { get; set; }
        public double Date { get; set; }
    }

    public class Powers
    {
        public double Power { get; set; }
        public double Date { get; set; }
    }

    public class Cadences
    {
        public double Cadence { get; set; }
        public double Date { get; set; }
    }
}
