using System.Windows;

namespace ActivityPulse.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel => (MainViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MainViewModel viewModel) : this()
        {
            DataContext = viewModel;
            Loaded += MainWindow_Loaded;
            Activated += MainWindow_Activated;
            IsVisibleChanged += MainWindow_IsVisibleChanged;
        }

        private void MainWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ViewModel.ChangeQuote();
        }

        private void MainWindow_Activated(object? sender, EventArgs e)
        {
            ViewModel.ChangeQuote();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Load(MainContentControl);
            ViewModel.ChangeQuote();
        }
    }
}