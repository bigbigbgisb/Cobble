using Cobble.Abstractions;
using Cobble.Models;
using CommunityToolkit.Mvvm.Input;
namespace Cobble.ViewModels
{
    public partial class SettingsPageViewModel : PageViewModelBase
    {
        private ApplicationData _applicationData;
        [RelayCommand]
        private void ChangeTheme()
        {
            ThemeViewModel.ChangeTheme();
            _applicationData.IsDarkTheme = !_applicationData.IsDarkTheme;
        }
        public SettingsPageViewModel(ThemeViewModel themeViewModel, IWindowService windowService,ApplicationData applicationData) : base(themeViewModel, windowService)
        {
            _applicationData = applicationData;
        }
    }
}
