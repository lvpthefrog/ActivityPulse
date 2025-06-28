using ActivityPulse.Application;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ActivityPulse.WPF
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly TrackingTimer _trackingTimer;

        [ObservableProperty]
        private string _currentPageTitle = "Summary";

        [ObservableProperty]
        private string _currentDate = DateTime.Now.ToString("dddd, MMMM dd, yyyy");

        [ObservableProperty]
        private object _currentView;

        [ObservableProperty]
        private string _currentQuote = "";

        public MainViewModel(
            TrackingTimer trackingTimer,
            INavigationService navigationService
            ) : base(navigationService)
        {
            _trackingTimer = trackingTimer;
        }

        public void Load(object currentView)
        {
            _trackingTimer.Start();
            _navigationService.SetView(currentView);
            _navigationService.NavigateTo<DashboardView>();
        }

        public void ChangeQuote()
        {
            CurrentQuote = Helpers.GetRandomQuote();
        }

        [RelayCommand]
        private void Navigate(string pageName)
        {
            switch (pageName)
            {
                case "Summary":
                    CurrentPageTitle = "Summary";
                    _navigationService.NavigateTo<DashboardView>();
                    break;
                case "Settings":
                    CurrentPageTitle = "Settings";
                    _navigationService.NavigateTo<SettingView>();
                    break;
                default:
                    break;
            }
        }
    }
}
