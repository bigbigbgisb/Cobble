using Cobble.Messages;
using Cobble.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Threading;
namespace Cobble.ViewModels
{
    public partial class DashboardPopupWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        DashboardMemoInfo _dashboardMemoInfo = new([]);
        [ObservableProperty]
        string? _currentLesson;
        [ObservableProperty]
        string? _nextLesson;
        [ObservableProperty]
        DateTime _currentTime;
        private DispatcherTimer _timer;
        public DashboardPopupWindowViewModel(ThemeViewModel themeViewModel) : base(themeViewModel)
        {
            WeakReferenceMessenger.Default.Register<UpdateDashboardMemoInfoMessage>(this, (r, m) =>
            {
                DashboardMemoInfo = m.DashboardMemoInfo;
            });
            WeakReferenceMessenger.Default.Register<UpdateCurrentAndNextLessonMessage>(this, (r, m) =>
            {
                CurrentLesson = m.CurrentLesson;
                NextLesson = m.NextLesson;
            });
            CurrentTime = DateTime.Now;
            _timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (s, e) =>
            {
                CurrentTime = DateTime.Now;
            };
            _timer.Start();
        }
    }
}
