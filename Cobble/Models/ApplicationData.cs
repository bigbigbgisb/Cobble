using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;

namespace Cobble.Models
{
    public class MemoPageData
    {
        public required Dictionary<string, ObservableCollection<MemoInfo>> Memos { get; set; }
        public required string CurrentSubject { get; set; }
        public required string CurrentMemo { get; set; }
    }
    public class TimetablePageData
    {
        public required ObservableCollection<DailyLessonInfos> DailyLessonInfos { get; set; }
        public required int ColumnCount { get; set; }
        public required int RowCount { get; set; }
        public required ObservableCollection<string> TipText { get; set; }
        public required ObservableCollection<LessonTime> LessonTimes { get; set; }
    }
    public class ApplicationData
    {
        public required bool IsDarkTheme { get; set; }
        public required string Version { get; set; }
        public required bool IsAllowAutoStart { get; set; }
        public required MemoPageData MemoPageData { get; set; }
        public required TimetablePageData TimetablePageData { get; set; }

        public static ApplicationData Default { get; } = new()
        {
            MemoPageData = new()
            {
                CurrentMemo = "",
                CurrentSubject = "",
                Memos = [],
            },
            TimetablePageData = new()
            {
                ColumnCount = 5,
                RowCount = 8,
                DailyLessonInfos = [],
                TipText = [],
                LessonTimes = []

            },
            IsDarkTheme = true,
            Version = Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString() ?? "Unknown Version",
            IsAllowAutoStart = true,
        };
        
    }
}
