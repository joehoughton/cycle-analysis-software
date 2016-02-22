namespace cycle_analysis.Domain.SessionData.Dtos
{
    using System;

    public class SessionDataDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public int Row { get; set; }
        public double HeartRate { get; set; }
        public double Speed { get; set; }
        public double Cadence { get; set; }
        public double Altitude { get; set; }
        public double Power { get; set; }
        public int SessionId { get; set; }
    }
}
