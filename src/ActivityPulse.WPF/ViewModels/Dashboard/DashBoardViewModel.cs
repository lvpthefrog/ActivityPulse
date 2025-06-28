using ActivityPulse.Application;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace ActivityPulse.WPF
{
    public partial class DashboardViewModel : BaseViewModel, IDisposable
    {
        private readonly AggregatorService _aggregatorService;
        private readonly ILogger<DashboardViewModel> _logger;
        private DispatcherTimer _timer;

        [ObservableProperty]
        private string _dailyQuote = string.Empty;

        [ObservableProperty]
        private string _applicationsCount = string.Empty;

        [ObservableProperty]
        private string _activeTime = string.Empty;

        [ObservableProperty]
        private string _productivityPercentage = string.Empty;

        [ObservableProperty]
        private ObservableCollection<AppUsageDto> _topApplications = new();

        public DashboardViewModel(
            AggregatorService aggregatorService,
            ILogger<DashboardViewModel> logger,
            INavigationService navigationService
            ) : base(navigationService)
        {
            _aggregatorService = aggregatorService;
            _logger = logger;
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(30)
            };
            _timer.Tick += async (s, e) => await RefreshDataAsync();
        }

        public async Task LoadAsync()
        {
            await RefreshDataAsync();
            if (!_timer.IsEnabled)
            {
                _timer.Start();
            }
        }

        public void Leave()
        {
            _timer.Stop();
        }

        private async Task RefreshDataAsync()
        {
            try
            {
                await _aggregatorService.StartAggregation();
                var (apps, userState) = await _aggregatorService.GetTodayDataAsync();

                DailyQuote = Helpers.GetRandomQuote();
                ApplicationsCount = apps.Count.ToString();
                ActiveTime = GetActiveTimeDisplay(userState);
                ProductivityPercentage = Math.Round(userState.ActiveTime / (double)(userState.TotalTime == 0 ? 1 : userState.TotalTime) * 100).ToString() + "%";
                TopApplications = new ObservableCollection<AppUsageDto>(apps);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing data in DashboardViewModel");
            }
        }

        private string GetActiveTimeDisplay(UserStateDto userState)
        {
            var activeTime = Helpers.FormatDisplayTime(userState.ActiveTime);
            var totalTime = Helpers.FormatDisplayTime(userState.TotalTime);
            return $"{activeTime} / {totalTime}";
        }

        public void Dispose()
        {
            _timer.Stop();
        }
    }
}
