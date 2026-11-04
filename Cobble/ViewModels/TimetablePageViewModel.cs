using Cobble.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Cobble.Models;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using Cobble.Messages;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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
        private ApplicationData _applicationData;
        readonly ILogger<TimetablePageViewModel> _logger;
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
        partial void OnTipTextChanged(ObservableCollection<string> value)
        {
            _applicationData.TimetablePageData.TipText = TipText;
        }
        partial void OnWeeklyLessonInfosChanged(ObservableCollection<DailyLessonInfos> value)
        {
            _applicationData.TimetablePageData.DailyLessonInfos = WeeklyLessonInfos;
        }
        private DailyLessonInfos GetCurrentLessonInfos()
        {
            int dayOfWeek = (int)DateTime.Now.DayOfWeek;
            if (dayOfWeek == 0) dayOfWeek = 7;
            if (dayOfWeek <= _columnCount)
            {
                List<LessonInfo> lessonInfos = [];
                foreach (var y in WeeklyLessonInfos)
                {
                    lessonInfos.Add(y.LessonInfos[dayOfWeek - 1]);
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
            try
            {
                DailyLessonInfos[] map = [];
                await Task.Run(() =>
                {
                    map = [.. Enumerable.Range(0, _rowCount)
                        .Select(_ => new DailyLessonInfos([.. Enumerable.Range(0, _columnCount).Select(_ => new LessonInfo())]))
                    ];
                });

                WeeklyLessonInfos = new(map);
                _popupViewModel.CurrentLessons = GetCurrentLessonInfos();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fire-and-forgot GenerateMap failed");
            }


        }
        private bool CheckDataInput(int max, int min, string message, string input, out int value)
        {
            value = default;
            if (int.TryParse(input, out var count) && count <= max && count >= min)
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
                _applicationData.TimetablePageData.ColumnCount = count;
                _ = GenerateMap();
            }
        }
        partial void OnRowCountInputChanged(string value)
        {
            if (CheckDataInput(18, 1, $"行数据必须在 1 到 18 之间", value, out var count))
            {
                _rowCount = count;
                _applicationData.TimetablePageData.RowCount = count;
                _ = GenerateMap();
            }

        }

        public TimetablePageViewModel(ThemeViewModel themeViewModel, IWindowService windowService, TimetablePopupWindowViewModel timetablePopupWindowViewModel, ILogger<TimetablePageViewModel> logger,ApplicationData applicationData) : base(themeViewModel, windowService)
        {
            _applicationData = applicationData;
            ColumnCountInput = _applicationData.TimetablePageData.ColumnCount.ToString();
            RowCountInput = _applicationData.TimetablePageData.RowCount.ToString();
            _columnCount = _applicationData.TimetablePageData.ColumnCount;
            _rowCount = _applicationData.TimetablePageData.RowCount;
            WeeklyLessonInfos = _applicationData.TimetablePageData.DailyLessonInfos;
            TipText = _applicationData.TimetablePageData.TipText;
            _logger = logger;
            _popupViewModel = timetablePopupWindowViewModel;
            if (WeeklyLessonInfos.Count == 0)
            {
                _ = GenerateMap();
            }
            WeakReferenceMessenger.Default.Register<TimetablePopupClosedMessage>(this, (r, m) =>
            {
                _isPopupWindowVisible = false;
            });
        }


    }
}
