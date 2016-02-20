namespace cycle_analysis.Domain.SessionData.Dtos
{
    using System;

    public class SessionDataDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public int Row { get; set; }
        public int HeartRate { get; set; }
        public int Speed { get; set; }
        public int Cadence { get; set; }
        public int Altitude { get; set; }
        public int Power { get; set; }
        public int SessionId { get; set; }
    }
}
