using CommunityToolkit.Mvvm.ComponentModel;

namespace Cobble.Models
{
    public partial class LessonInfo : ObservableObject
    {
        [ObservableProperty]
        string _lessonName = "";
    }
    public partial class DailyLessonInfos(LessonInfo[] lessonInfos) : ObservableObject
    {
        [ObservableProperty]
        LessonInfo[] _lessonInfos = lessonInfos;
    }
}
