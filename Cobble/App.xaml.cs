using Cobble.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

using Cobble.Abstractions;
using Cobble.Services;
using Serilog;
using System.IO;
using Microsoft.Extensions.Logging;
namespace Cobble
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static IServiceProvider? Services { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var serviceCollection = new ServiceCollection();
            Serilog.Log.Logger = new LoggerConfiguration().MinimumLevel
#if DEBUG
                .Debug()
#else
                .Information()
#endif
                .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "CobbleLogs", "cobble-.log"), rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] <{SourceContext}> {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
            serviceCollection.AddLogging(builder =>
            {
                builder.AddSerilog(Serilog.Log.Logger, dispose: true);
#if DEBUG
                builder.SetMinimumLevel(LogLevel.Debug);
#else
                builder.SetMinimumLevel(LogLevel.Information);
#endif
            });
            serviceCollection.AddSingleton<INavigationService, Cobble.Services.NavigationService>();
            serviceCollection.AddSingleton<IWindowService, WindowService>();
            serviceCollection.AddSingleton<ThemeViewModel>();
            serviceCollection.AddSingleton<TimetablePageViewModel>();
            serviceCollection.AddSingleton<MemoPageViewModel>();
            serviceCollection.AddSingleton<TimetablePopupWindowViewModel>();
            serviceCollection.AddSingleton<DashboardPopupWindowViewModel>();
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddSingleton<MainWindow>();

            Services = serviceCollection.BuildServiceProvider();
            Services.GetRequiredService<MainWindow>().Show();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            Log.CloseAndFlush();
            base.OnExit(e);
        }
    }

}
