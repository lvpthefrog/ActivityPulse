namespace ActivityPulse.Application
{
    public interface ISystemProvider
    {
        ActiveWindowInfo GetActiveWindowInfo();
        TimeSpan GetIdleTime();
        string? ExtractIcon(string processPath);
    }
}
