using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cobble.Abstractions;
using CommunityToolkit.Mvvm.Messaging;
using Cobble.Messages;
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
        public MainWindowViewModel(ThemeViewModel themeViewModel, INavigationService navigationService, IWindowService windowService, ILogger<MainWindowViewModel> logger) : base(themeViewModel)
        {
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
            WeakReferenceMessenger.Default.Register<DashboardPopupClosedMessage>(this, (r, m) =>
            {
                _isDashboardVisible = false;
                _logger.LogDebug("Enter Register DashboardPopupClosedMessage Code");
            });
        }
    }
}
