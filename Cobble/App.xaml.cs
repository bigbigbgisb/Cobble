using Cobble.Abstractions;
using Cobble.Messages;
using Cobble.Models;
using Cobble.Services;
using Cobble.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using WindowsAutostartApi.Abstractions;
using WindowsAutostartApi.Core;
namespace Cobble
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static IServiceProvider? Services { get; private set; }
        public readonly string AppDataPath = Path.Combine(AppContext.BaseDirectory, "CobbleAppData", "AppData.json");
        protected override async void OnStartup(StartupEventArgs e)
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
            //Build basic singletons.
            serviceCollection.AddSingleton<INavigationService, Cobble.Services.NavigationService>();
            serviceCollection.AddSingleton<IWindowService, WindowService>();
            serviceCollection.AddSingleton<ThemeViewModel>();
            serviceCollection.AddSingleton<TimetablePageViewModel>();
            serviceCollection.AddSingleton<MemoPageViewModel>();
            serviceCollection.AddSingleton<TimetablePopupWindowViewModel>();
            serviceCollection.AddSingleton<DashboardPopupWindowViewModel>();
            serviceCollection.AddSingleton<LuckyDrawPageViewModel>();
            serviceCollection.AddSingleton<SettingsPageViewModel>();
            serviceCollection.AddSingleton<EasterEggPageViewModel>();
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddSingleton<MainWindow>();
            //Add app data to container.
            var store = new ApplicationDataStore();
            ApplicationData? appData;
            try
            {
                appData = await store.LoadAsync(AppDataPath);
            }
            catch (Exception ex)
            {
                appData = ApplicationData.Default;
                Serilog.Log.Logger.Warning(ex, "读取数据失败");
                _ = Current.Dispatcher.BeginInvoke(() =>
                {
                    WeakReferenceMessenger.Default.Send(new ShowTipMessage("启动时读取数据失败,初始化为空数据", true));
                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
            appData ??= ApplicationData.Default;
            Serilog.Log.Logger.Information("软件版本 {Version}", Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString() ?? "Unknown Version");
            Serilog.Log.Logger.Information("存档版本 {AppDataVersion}", appData.Version);

            if (appData.Version?.Split('.')[0] != Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString().Split('.')[0])
            {
                _ = Current.Dispatcher.BeginInvoke(() =>
                {
                    WeakReferenceMessenger.Default.Send(new ShowTipMessage("数据主版本与软件当前版本不同,但未报错。请留意存档版本", true));
                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
            
            serviceCollection.AddSingleton<ApplicationData>(sp =>
            {
                return appData;
            });
            
            //Set auto start
            var manager = new StartupManager();
            if (appData.IsAllowAutoStart)
            {
                try
                {
                    manager.Add(new(Name: "Cobble", TargetPath: Environment.ProcessPath!, Scope: StartupScope.CurrentUser, Kind: StartupKind.Run, Arguments: null));
                }
                catch (Exception ex)
                {
                    Serilog.Log.Logger.Warning(ex, "设置开机自启动失败");
                }
            }
            else
            { 
                try
                {
                    manager.Remove("Cobble", WindowsAutostartApi.Abstractions.StartupScope.CurrentUser, StartupKind.Run);
                }
                catch (Exception ex)
                {
                    Serilog.Log.Logger.Warning(ex, "关闭开机自启动失败");
                    WeakReferenceMessenger.Default.Send(new ShowTipMessage("关闭开机自启动失败", true));
                }
            }

            //Build.
            Services = serviceCollection.BuildServiceProvider();
            Services.GetRequiredService<MainWindow>().Show();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnExit(ExitEventArgs e)
        {

            var store = new ApplicationDataStore();
            var appData = Services?.GetRequiredService<ApplicationData>();
            if (store is not null && appData is not null)
            {
                try
                {
                    if (!await store.SaveAsync(AppDataPath, appData))
                    {
                        Serilog.Log.Logger.Warning("写入路径的根目录的根目录 {Path} 不存在", AppDataPath);
                    }
                }
                catch (Exception ex)
                {
                    Serilog.Log.Logger.Warning(ex, "写入数据失败");
                }


            }
            Log.CloseAndFlush();
            base.OnExit(e);
        }
    }

}
