using Cobble.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cobble
{
    public partial class ViewModelBase(ThemeViewModel themeViewModel) : ObservableObject
    {
        [ObservableProperty]
        ThemeViewModel _themeViewModel = themeViewModel;
    }
}
