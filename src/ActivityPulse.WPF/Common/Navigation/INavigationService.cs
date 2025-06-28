using System.Windows.Controls;

namespace ActivityPulse.WPF
{
    public interface INavigationService
    {
        void SetView(object view);
        void NavigateTo<TView>(Action<TView>? before = null, Action<TView>? after = null) where TView : UserControl;
    }
}
