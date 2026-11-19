using Cobble.Abstractions;
using Cobble.Messages;
using Cobble.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
namespace Cobble.ViewModels
{
    public partial class SettingsPageViewModel : PageViewModelBase
    {
        private ApplicationData _applicationData;
        [ObservableProperty]
        string _durationInput = "";
        private TimeSpan _duration;
        [ObservableProperty]
        ObservableCollection<string> _tipText = [];
        private bool CheckDataInput(string message, string input, out int value)
        {
            value = default;
            if (int.TryParse(input, out var count))
            {
                TipText.Remove(message);
                value = count;
                return true;
            }
            else
            {
                if (!TipText.Contains(message))
                    TipText.Add(message);
                return false;
            }
        }
        partial void OnDurationInputChanged(string value)
        {
            if (CheckDataInput("输入时间(分)非数字",_durationInput,out int result))
            {
                _duration = TimeSpan.FromMinutes(result);
                _applicationData.GlobalSettingsData.TempStopDashboardDuration = _duration;
            }
        }
        [RelayCommand]
        private void StopAutoShowTimer()
        {
            WeakReferenceMessenger.Default.Send<StopAutoShowTimerMessage>();
            _applicationData.GlobalSettingsData.IsAutoShowDashboard = false;
        }
        [RelayCommand]
        private void StartAutoShowTimer()
        {
            WeakReferenceMessenger.Default.Send<StartAutoShowTimerMessage>();
            _applicationData.GlobalSettingsData.IsAutoShowDashboard = true;
        }

        
        public SettingsPageViewModel(ThemeViewModel themeViewModel, IWindowService windowService,ApplicationData applicationData) : base(themeViewModel, windowService)
        {
            _applicationData = applicationData;
            DurationInput = ((int)applicationData.GlobalSettingsData.TempStopDashboardDuration.TotalMinutes).ToString();
        }
    }
}
