namespace ActivityPulse.Domain
{
    public class AppMetaData
    {
        public string ProcessName { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? IconPath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
