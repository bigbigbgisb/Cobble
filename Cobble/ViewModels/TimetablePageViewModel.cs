using Cobble.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Cobble.Models;
using CommunityToolkit.Mvvm.Input;

namespace Cobble.ViewModels
{

    public partial class TimetablePageViewModel : PageViewModelBase
    {

        private static readonly Dictionary<int, string> _subjectMapping = new()
        {
            { 1, "语文" },
            { 2, "数学" },
            { 3, "英语" },
            { 4, "物理" },
            { 5, "化学" },
            { 6, "生物" },
            { 7, "道法" },
            { 8, "历史" },
            { 9, "地理" },
            { 10, "体育" },
            { 11, "音乐" },
            { 12, "美术" },
            { 13, "信息技术" },
            { 14, "人工智能基础" },
            { 15, "班会" },
            { 16, "自习" },
            { 17, "课后服务" },
            { 18, "生态ODC课程" },
            { 19, "校本" }
        };
        private readonly TimetablePopupWindowViewModel _popupViewModel;
        private bool _isPopupWindowVisible = false;
        private int _columnCount = 5;
        private int _rowCount = 8;
        public string[] SubjectHints { get; set; } = [.. _subjectMapping.Select(x => $"{x.Key} : {x.Value}")];
        [ObservableProperty]
        ObservableCollection<DailyLessonInfos> _weeklyLessonInfos = [];
        [ObservableProperty]
        ObservableCollection<string> _tipText = [];

        [ObservableProperty]
        string _columnCountInput = "5";
        [ObservableProperty]
        string _rowCountInput = "8";
        public int[] DayNumbers { get; } = [.. Enumerable.Range(1, 7)];
        private DailyLessonInfos GetCurrentLessonInfos() 
        {
            int dayInWeek = (int)DateTime.Now.DayOfWeek;
            if (dayInWeek <= _columnCount)
            {
                List<LessonInfo> lessonInfos = [];
                foreach(var y in WeeklyLessonInfos)
                {
                    lessonInfos.Add(y.LessonInfos[dayInWeek]);
                }
                return new DailyLessonInfos([.. lessonInfos]);
            }
            else
            {

                return new DailyLessonInfos([new() { LessonName = "非学习日" }]);
            }
        }

        private async Task GenerateMap()
        {
            DailyLessonInfos[] map = [];
#pragma warning disable CS0168 //等待日志添加
            try
            {
                await Task.Run(() =>
                {
                    map =
                    Enumerable.Range(0, _rowCount).Select(x => new DailyLessonInfos(Enumerable.Range(0, _columnCount).Select(x => new LessonInfo()).ToArray())).ToArray();
                });
            }
            catch (Exception ex)
            {
                //Going to log it
            }
#pragma warning restore CS0168 

            WeeklyLessonInfos = new(map);
            _popupViewModel.CurrentLessons = GetCurrentLessonInfos();
        }
        private bool CheckDataInput(int max, int min, string message, string input, out int value)
        {
            value = default;
            if (int.TryParse(input, out var count) && count <= max && count >= min)
            {
                if (TipText.Contains(message))
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
        [RelayCommand]
        private void TogglePopupWindow()
        {
            if (!_isPopupWindowVisible)
            {
                _popupViewModel.CurrentLessons = GetCurrentLessonInfos();
                _windowService.ShowWindow<TimetablePopupWindowViewModel>();
                _isPopupWindowVisible = true;
            }
            else
            {
                _windowService.CloseWindow<TimetablePopupWindowViewModel>();
                _isPopupWindowVisible = false;
            }
        }

        partial void OnColumnCountInputChanged(string value)
        {
            if (CheckDataInput(8, 1, $"列数据必须在 1 到 8 之间", value, out var count))
            {
                _columnCount = count;
                _ = GenerateMap();
            }
        }
        partial void OnRowCountInputChanged(string value)
        {
            if (CheckDataInput(18, 1, $"行数据必须在 1 到 18 之间", value, out var count))
            {
                _rowCount = count;
                _ = GenerateMap();
            }

        }

        public TimetablePageViewModel(ThemeViewModel themeViewModel, IWindowService windowService, TimetablePopupWindowViewModel timetablePopupWindowViewModel) : base(themeViewModel, windowService)
        {
            _popupViewModel = timetablePopupWindowViewModel;
            _ = GenerateMap();
        }


    }
}
