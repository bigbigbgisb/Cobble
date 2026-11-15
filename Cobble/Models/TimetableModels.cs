using Cobble.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Cobble.Models
{
    public partial class LessonInfo : ObservableObject
    {
        [ObservableProperty]
        string _lessonName = "";
        partial void OnLessonNameChanged(string value)
        {
            WeakReferenceMessenger.Default.Send<LessonInnerDataChanged>();
        }
    }
    public partial class DailyLessonInfos(LessonInfo[] lessonInfos) : ObservableObject
    {
        [ObservableProperty]
        LessonInfo[] _lessonInfos = lessonInfos;
    }
    public partial class LessonTime : ObservableObject
    {
        [ObservableProperty]
        public partial DateTime StartTime { get; set; }
        [ObservableProperty]
        public partial DateTime EndTime { get; set; }
        partial void OnStartTimeChanged(DateTime value)
        {
            WeakReferenceMessenger.Default.Send<LessonInnerDataChanged>();
        }
        partial void OnEndTimeChanged(DateTime value)
        {
            WeakReferenceMessenger.Default.Send<LessonInnerDataChanged>();
        }
    }
}
