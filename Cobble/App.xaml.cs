using Cobble.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Cobble.Abstactions;
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
            Serilog.Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.File(Path.Combine(AppContext.BaseDirectory, "Cobble Log", "cobble-.log"), rollingInterval: RollingInterval.Day,outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] <{SourceContext}> {Message:lj}{NewLine}{Exception}").CreateLogger();
            serviceCollection.AddLogging(builder =>
            {
                builder.AddSerilog(Serilog.Log.Logger,dispose:true);
                builder.SetMinimumLevel(LogLevel.Information);
            });
            serviceCollection.AddSingleton<INavigationService, Cobble.Services.NavigationService>();
            serviceCollection.AddSingleton<IWindowService, WindowService>();
            serviceCollection.AddSingleton<ThemeViewModel>();
            serviceCollection.AddSingleton<TimetablePageViewModel>();
            serviceCollection.AddSingleton<MemoPageViewModel>();
            serviceCollection.AddSingleton<TimetablePopupWindowViewModel>();
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddSingleton<MainWindow>();
            
            Services = serviceCollection.BuildServiceProvider();
            Services.GetRequiredService<MainWindow>().Show();

        }
        protected override void OnExit(ExitEventArgs e)
        {
            Log.CloseAndFlush();
            base.OnExit(e);
        }
    }

}
