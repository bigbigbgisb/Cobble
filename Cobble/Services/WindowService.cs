using Cobble.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows;
using System.Windows.Controls;

namespace Cobble.Services
{
    public class WindowService(ILogger<WindowService> logger) : IWindowService
    {
        private readonly ILogger _logger = logger;
        public Dictionary<ViewModelBase, Window> Windows { get; set; } = [];
        public void ShowWindow<T>() where T : ViewModelBase
        {
            var vm = App.Services?.GetRequiredService<T>();
            string? fullName = vm?.GetType().FullName;
            if (fullName is not null && vm is not null)
            {
                string windowName = fullName.Replace(".ViewModels.", ".Views.").Replace("WindowViewModel", "Window").Replace("ViewModel", "Window");
                Type? type = Type.GetType(windowName);
                Window window = new()
                {
                    Content = new TextBlock() { Text = $"Cannot find view {windowName}" }
                };
                if (type is not null)
                {
                    if (Activator.CreateInstance(type) is Window w)
                    {
                        window = w;
                    }
                }
                else
                {
                    _logger.LogWarning("Cannot find view {WindowName}", windowName);
                }
                window.DataContext = vm;
                Windows[vm] = window;
                window.Closed += (s, e) =>
                {
                    if (Windows.ContainsKey(vm))
                    {
                        Windows.Remove(vm);
                    }
                };
                window.Show();
            }
        }
        public void CloseWindow<T>() where T : ViewModelBase
        {
            var vm = App.Services?.GetRequiredService<T>();
            if (vm is not null && Windows.ContainsKey(vm))
            {
                Windows[vm].Close();
                Windows.Remove(vm);
            }
            else
            {
                _logger.LogWarning("Cannot close an already closed window {ViewModel} (viewmodel) ", typeof(T).FullName);
            }

        }
    }
}
