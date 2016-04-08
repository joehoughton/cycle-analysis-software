namespace cycle_analysis.Domain.SessionData.Dtos
{
    public class IntervalSummaryRequestDto
    {
        public int SessionId { get; set; }
        public decimal StartTime { get; set; }
        public decimal FinishTime { get; set; }
    }
}
