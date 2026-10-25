using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Cobble.Abstactions
{
    public interface INavigationService : INotifyPropertyChanged
    {
        ViewModelBase? CurrentPage { get; }
        void NavigateTo<T>() where T : ViewModelBase;
    }
}
