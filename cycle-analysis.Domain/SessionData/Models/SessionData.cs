namespace cycle_analysis.Domain.SessionData.Models
{
    using cycle_analysis.Domain.Session.Models;

    public class SessionData
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public double HeartRate { get; set; }
        public double Speed { get; set; }
        public double Cadence { get; set; }
        public double Altitude { get; set; }
        public double Power { get; set; }
        public int SessionId { get; set; }
        public virtual Session Session { get; set; }
    }
}
