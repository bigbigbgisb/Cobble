using Cobble.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Cobble.Services
{
    public class WindowService : IWindowService
    {
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
                window.DataContext = vm;
                Windows[vm] = window;
                window.Show();
            }
        }
        public void CloseWindow<T>() where T : ViewModelBase
        {
            var vm = App.Services?.GetRequiredService<T>();
            if (vm is not null)
            {
                Windows[vm].Close();
                Windows.Remove(vm);
            }
                
        }
    }
}
