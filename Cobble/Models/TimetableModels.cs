using Cobble.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
namespace Cobble.Models
{
    public partial class LessonInfo : ObservableObject
    {
        [ObservableProperty]
        public partial string LessonName { get; set; } = "";

        partial void OnLessonNameChanged(string value)
        {
            WeakReferenceMessenger.Default.Send<LessonInnerDataChanged>();
        }
    }
    public partial class DailyLessonInfos : ObservableObject
    {
        [ObservableProperty]
        public partial LessonInfo[] LessonInfos { get; set; }

        public DailyLessonInfos(LessonInfo[] lessonInfos)
        {
            LessonInfos = lessonInfos;
        }
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
