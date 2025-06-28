namespace ActivityPulse.Domain
{
    public class SessionAggregate : AuditableEntity
    {
        public DateTime Date { get; set; }
        public string ProcessName { get; set; } = string.Empty;
        public string ProcessPath { get; set; } = string.Empty;
        public double TotalDuration { get; set; }
        public int SessionCount { get; set; }
    }
}
