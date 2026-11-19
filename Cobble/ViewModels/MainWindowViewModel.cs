using Cobble.Abstractions;
using Cobble.Messages;
using Cobble.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
namespace Cobble.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private ILogger<MainWindowViewModel> _logger;
        [ObservableProperty]
        INavigationService _navigationService;
        IWindowService _windowService;
        bool _isDashboardVisible = false;
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
        [RelayCommand]
        private void ToLuckyDrawPage()
        {
            NavigationService.NavigateTo<LuckyDrawPageViewModel>();
        }
        [RelayCommand]
        private void ToSettingsPage()
        {
            NavigationService.NavigateTo<SettingsPageViewModel>();
        }
        [RelayCommand]
        private void ToEasterEggPage()
        {
            NavigationService.NavigateTo<EasterEggPageViewModel>();
        }
        public MainWindowViewModel(ThemeViewModel themeViewModel, INavigationService navigationService, IWindowService windowService, ILogger<MainWindowViewModel> logger,ApplicationData applicationData) : base(themeViewModel)
        {
            if (!applicationData.GlobalSettingsData.IsDarkTheme) ThemeViewModel.ChangeTheme();
            NavigationService = navigationService;
            _logger = logger;
            _windowService = windowService;
            ToTimetablePage();
            WeakReferenceMessenger.Default.Register<ToggleDashboardPopupMessage>(this, (r, m) =>
            {
                if (!_isDashboardVisible)
                {
                    _windowService.ShowWindow<DashboardPopupWindowViewModel>();
                    _isDashboardVisible = true;
                }   
                else
                    _windowService.CloseWindow<DashboardPopupWindowViewModel>();
            });
            WeakReferenceMessenger.Default.Register<ShowDashboardPopupMessage>(this, (r, m) =>
            {
                if (!_isDashboardVisible)
                {
                    _windowService.ShowWindow<DashboardPopupWindowViewModel>();
                    _isDashboardVisible = true;
                }
                  
            });
            WeakReferenceMessenger.Default.Register<HideDashboardPopupMessage>(this, (r, m) =>
            {
                if (_isDashboardVisible)
                    _windowService.CloseWindow<DashboardPopupWindowViewModel>();
            });
            WeakReferenceMessenger.Default.Register<DashboardPopupClosedMessage>(this, (r, m) =>
            {
                _isDashboardVisible = false;
            });
        }
    }
}
