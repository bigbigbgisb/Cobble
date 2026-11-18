using Cobble.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
namespace Cobble
{
    public partial class ViewModelBase(ThemeViewModel themeViewModel) : ObservableObject
    {
        [ObservableProperty]
        ThemeViewModel _themeViewModel = themeViewModel;
    }
}
