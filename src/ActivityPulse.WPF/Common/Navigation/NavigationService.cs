using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
namespace ActivityPulse.WPF
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _provider;

        private ContentControl? _root;

        public NavigationService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void SetView(object view)
        {
            _root = view as ContentControl;
        }

        public void NavigateTo<TView>(
            Action<TView>? before = null,
            Action<TView>? after = null
        ) where TView : UserControl
        {
            if (_root == null)
                throw new InvalidOperationException("Root view not set");

            var newView = _provider.GetRequiredService<TView>();
            before?.Invoke(newView);
            _root.Content = newView;
            after?.Invoke(newView);
        }
    }
}