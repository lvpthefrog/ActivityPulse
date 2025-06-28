namespace ActivityPulse.Application
{
    public class AppUsageDto
    {
        public string ProcessName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? IconPath { get; set; }
        public double TotalDuration { get; set; }

        public string TotalDurationDisplay => Helpers.FormatDisplayTime(TotalDuration);

        public double ProductivityPercentage { get; set; }

        public string ProductivityPercentageDisplay => $"{ProductivityPercentage.ToString()}%";
    }
}
