using ActivityPulse.Application;
using System.Runtime.Versioning;

namespace ActivityPulse.Infrastructure
{
    public class SystemProvider : ISystemProvider
    {
        public SystemProvider() { }

        public ActiveWindowInfo GetActiveWindowInfo()
        {
            return Win32Helper.GetActiveWindowInfo();
        }

        public TimeSpan GetIdleTime()
        {
            return Win32Helper.GetIdleTime();
        }

        [SupportedOSPlatform("windows")]
        public string? ExtractIcon(string processPath)
        {
            return IconExtractor.ExtractIcon(processPath);
        }
    }
}
