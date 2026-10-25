using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

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
