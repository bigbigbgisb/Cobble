using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cobble.Abstactions;
using System.Windows.Forms;

namespace Cobble.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        INavigationService _navigationService;
        [RelayCommand]
        private void ToTimetablePage()
        {
            NavigationService.NavigateTo<TimetablePageViewModel>();
        }
        [RelayCommand]
        private void ToMemoPage()
        {
            NavigationService.NavigateTo<MemoPageViewModel>();
        }
        public MainWindowViewModel(ThemeViewModel themeViewModel, INavigationService navigationService) : base(themeViewModel)
        {
            NavigationService = navigationService;
            ToTimetablePage();
        }
    }
}
