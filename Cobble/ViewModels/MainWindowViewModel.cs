using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Navigation;
using Cobble.Abstactions;

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
        public MainWindowViewModel(ThemeViewModel themeViewModel, INavigationService navigationService) : base(themeViewModel)
        {
            NavigationService = navigationService;
            ToTimetablePage();
        }


    }
}
