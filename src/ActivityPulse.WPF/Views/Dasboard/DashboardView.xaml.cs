using System.Windows.Controls;

namespace ActivityPulse.WPF
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {

        public DashboardView()
        {
            InitializeComponent();
        }

        public DashboardView(DashboardViewModel viewModel) : this()
        {
            DataContext = viewModel;
            Loaded += DashboardView_Loaded;
            IsVisibleChanged += DashboardView_IsVisibleChanged;
        }

        private async void DashboardView_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            await ((DashboardViewModel)DataContext).LoadAsync();
        }

        private async void DashboardView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await ((DashboardViewModel)DataContext).LoadAsync();
        }
    }
}
