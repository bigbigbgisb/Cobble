using Cobble.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Cobble.ViewModels
{
    public partial class TimetablePopupWindowViewModel(ThemeViewModel themeViewModel) : ViewModelBase(themeViewModel)
    {
        [ObservableProperty]
        DailyLessonInfos? _currentLessons;

    }
}
