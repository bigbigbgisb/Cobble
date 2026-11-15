using Cobble.Abstractions;
using Cobble.Messages;
using Cobble.Models;
using Cobble.Services;
using Cobble.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
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
        public readonly string AppCachePath = Path.Combine(AppContext.BaseDirectory, "CobbleAppCache", "AppCache.json");
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var serviceCollection = new ServiceCollection();
            Log.Logger = new LoggerConfiguration().MinimumLevel
#if DEBUG
                .Debug()
#else
                .Information()
#endif
                .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "CobbleLogs", "cobble-.log"), rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] <{SourceContext}> {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
            serviceCollection.AddLogging(builder =>
            {
                builder.AddSerilog(Log.Logger, dispose: true);
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
            var cacheStore = new ApplicationCacheStore();
            ApplicationData? appData;
            ApplicationCache? appCache;
            try
            {
                appData = await store.LoadAsync(AppDataPath);
            }
            catch (Exception ex)
            {
                appData = ApplicationData.Default;
                Log.Logger.Warning(ex, "读取数据失败");
                _ = Current.Dispatcher.BeginInvoke(() =>
                {
                    WeakReferenceMessenger.Default.Send(new ShowTipMessage("启动时读取数据失败,初始化为空数据", true));
                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
            try
            {
                appCache = await cacheStore.LoadAsync(AppCachePath);
            }
            catch (Exception ex)
            {
                appCache = ApplicationCache.Default;
                Log.Logger.Warning(ex, "读取数据失败");
                _ = Current.Dispatcher.BeginInvoke(() =>
                {
                    WeakReferenceMessenger.Default.Send(new ShowTipMessage("启动时读取数据失败,初始化为空数据", true));
                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
            appData ??= ApplicationData.Default;
            appCache ??= ApplicationCache.Default;
            Log.Logger.Information("软件版本 {Version}", Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString() ?? "Unknown Version");
            Log.Logger.Information("存档版本 {AppDataVersion}", appData.Version);
            if (appCache.LastTime.Date < DateTime.Now.Date)
            {
                appData.MemoPageData.Memos = [];
                appData.MemoPageData.CurrentMemo = "";
                appData.MemoPageData.CurrentSubject = "";
                Log.Logger.Information("时间跨日,已清除昨日备忘录");
            }
            if (appData.Version?.Split('.')[0] != Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString().Split('.')[0])
            {
                _ = Current.Dispatcher.BeginInvoke(() =>
                {
                    WeakReferenceMessenger.Default.Send(new ShowTipMessage("数据主版本与软件当前版本不同,但未报错。请留意存档版本", true));
                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }

            serviceCollection.AddSingleton(sp =>
            {
                return appData;
            });
            serviceCollection.AddSingleton(sp =>
            {
                return appCache;
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
                    Log.Logger.Warning(ex, "设置开机自启动失败");
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
                    Log.Logger.Warning(ex, "关闭开机自启动失败");
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
            var cacheStore = new ApplicationCacheStore();
            var appData = Services?.GetRequiredService<ApplicationData>();
            var appCache = Services?.GetRequiredService<ApplicationCache>();
            if (appData is not null && appCache is not null)
            {
                try
                {
                    if (!Task.Run(()=> store.SaveAsync(AppDataPath, appData)).GetAwaiter().GetResult())
                    {
                        Log.Logger.Warning("写入路径的根目录的根目录 {Path} 不存在", AppDataPath);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Warning(ex, "写入数据失败");
                }
                try
                {
                    appCache.LastTime = DateTime.Now;
                    if (!Task.Run(() => cacheStore.SaveAsync(AppCachePath, appCache)).GetAwaiter().GetResult())
                    {
                        Log.Logger.Warning("写入路径的根目录的根目录 {Path} 不存在", AppCachePath);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Warning(ex, "写入数据失败");
                }
            }

            Log.CloseAndFlush();
            base.OnExit(e);
        }
    }

}
