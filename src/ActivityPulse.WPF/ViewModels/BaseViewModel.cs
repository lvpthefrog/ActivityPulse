using CommunityToolkit.Mvvm.ComponentModel;

namespace ActivityPulse.WPF
{
    public class BaseViewModel : ObservableValidator
    {
        protected readonly INavigationService _navigationService;
        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
