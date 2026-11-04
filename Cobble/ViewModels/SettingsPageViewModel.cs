using Cobble.Abstractions;
using Cobble.Models;
using Cobble.Services;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

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
