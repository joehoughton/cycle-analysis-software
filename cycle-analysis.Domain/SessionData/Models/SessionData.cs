namespace cycle_analysis.Domain.SessionData.Models
{
    using cycle_analysis.Domain.Session.Models;

    public class SessionData
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int HeartRate { get; set; }
        public int Speed { get; set; }
        public int Cadence { get; set; }
        public int Altitude { get; set; }
        public int Power { get; set; }
        public int SessionId { get; set; }
        public virtual Session Session { get; set; }
    }
}
