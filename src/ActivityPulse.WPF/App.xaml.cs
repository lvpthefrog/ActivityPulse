using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using ActivityPulse.Application;
using ActivityPulse.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ActivityPulse.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private IServiceProvider _serviceProvider;

        public App()
        {
            var services = ConfigureServices();
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
            DbInitializer.EnsureCreated();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var trackingTimer = _serviceProvider.GetService<TrackingTimer>();
            if (trackingTimer != null)
            {
                trackingTimer.Stop();
            }
            base.OnExit(e);
        }

        private IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddLogging(configure => configure.SetMinimumLevel(LogLevel.Debug));

            // Register Infrastructure and Application services
            services.AddInfrastructureServices();
            services.AddApplicationServices();

            // Register Views
            services.AddScoped<MainWindow>();
            services.AddScoped<DashboardView>();
            services.AddScoped<SettingView>();

            //Register ViewModels
            services.AddScoped<MainViewModel>();
            services.AddScoped<DashboardViewModel>();

            services.AddScoped<INavigationService, NavigationService>();
            services.AddSingleton<TrackingTimer>();
            services.AddSingleton<AggregatorService>();

            return services;
        }
    }

}
