using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Cobble.Abstactions;

namespace Cobble.Services
{
    public partial class NavigationService : ObservableObject,INavigationService
    {
        [ObservableProperty]
        ViewModelBase? _currentPage;
        public void NavigateTo<T>() where T : ViewModelBase
        {
            var vm = App.Services?.GetRequiredService<T>();
            if (vm is null)
            {
                //日志记录
                return;
            }
            CurrentPage = vm;
        }

    }
}
