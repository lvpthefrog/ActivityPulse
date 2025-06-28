namespace ActivityPulse.Domain
{
    public class Session : AuditableEntity
    {
        public long AppMetaDataId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string ProcessName { get; set; } = string.Empty;
        public string ProcessPath { get; set; } = string.Empty;
        public string? WindowTitle { get; set; }
    }
}
