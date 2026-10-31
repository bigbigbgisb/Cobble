using Cobble.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Cobble.ViewModels
{
    public partial class TimetablePopupWindowViewModel(ThemeViewModel themeViewModel) : ViewModelBase(themeViewModel)
    {
        [ObservableProperty]
        DailyLessonInfos? _currentLessons;
    }
}
