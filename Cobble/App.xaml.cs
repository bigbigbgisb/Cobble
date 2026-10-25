using Cobble.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;
using Cobble.Abstactions;
using Cobble.Abstractions;
using Cobble.Services;
namespace Cobble
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? Services { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<INavigationService, Cobble.Services.NavigationService>();
            serviceCollection.AddSingleton<ThemeViewModel>();
            serviceCollection.AddSingleton<TimetablePageViewModel>();
            serviceCollection.AddSingleton<IWindowService, WindowService>();
            serviceCollection.AddSingleton<TimetablePopupWindowViewModel>();
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddSingleton<MainWindow>();
            Services = serviceCollection.BuildServiceProvider();
            Services.GetRequiredService<MainWindow>().Show();

        }
    }

}
