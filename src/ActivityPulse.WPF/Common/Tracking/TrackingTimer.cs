using ActivityPulse.Application;
using Microsoft.Extensions.Logging;
using System.Windows.Threading;

namespace ActivityPulse.WPF
{
    public class TrackingTimer
    {
        private readonly ITrackingService _trackingService;
        private readonly ILogger<TrackingTimer> _logger;
        private readonly DispatcherTimer _trackingTimer;
        public TrackingTimer(ITrackingService trackingService, ILogger<TrackingTimer> logger)
        {
            _trackingService = trackingService;
            _logger = logger;
            _trackingTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(15)
            };
            _trackingTimer.Tick += async (s, e) => await TrackingAsync();
        }
        public void Start()
        {
            _trackingTimer.Start();
            TrackingAsync().ConfigureAwait(false);
        }
        private async Task TrackingAsync()
        {
            try
            {
                await _trackingService.TrackAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during tracking");
            }
        }
        public void Stop()
        {
            _trackingTimer.Stop();
        }

        public void Dispose()
        {
            _trackingTimer?.Stop();
        }
    }
}
