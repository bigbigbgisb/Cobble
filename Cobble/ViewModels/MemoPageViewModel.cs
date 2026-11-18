using Cobble.Abstractions;
using Cobble.Messages;
using Cobble.Models;
using Cobble.Parsers;
using Cobble.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.IO;
namespace Cobble.ViewModels
{
    public partial class MemoPageViewModel : PageViewModelBase
    {
        private ApplicationData _applicationData;
        private ILogger<MemoPageViewModel> _logger;
        private MemoParser _memoParser = new();
        private MemoFileStore _memoFileStore = new();
        [ObservableProperty]
        Dictionary<string, ObservableCollection<MemoInfo>> _memos = [];
        [ObservableProperty]
        string? _currentSubject;
        [ObservableProperty]
        string? _currentMemo;
        partial void OnCurrentMemoChanged(string? value)
        {
            _applicationData.MemoPageData.CurrentMemo = CurrentMemo ?? "";
        }
        partial void OnCurrentSubjectChanged(string? value)
        {
            _applicationData.MemoPageData.CurrentSubject = CurrentSubject ?? "";
        }
        partial void OnMemosChanged(Dictionary<string, ObservableCollection<MemoInfo>> value)
        {
            _applicationData.MemoPageData.Memos = Memos;
        }
        [RelayCommand]
        private void AddMemo()
        {
            if (string.IsNullOrWhiteSpace(CurrentMemo)) return;
            const string replaceText = "其他";
            string key;
            if (string.IsNullOrWhiteSpace(CurrentSubject))
                key = replaceText;
            else
                key = CurrentSubject;
            if (Memos.TryGetValue(key, out var value))
            {
                if (value.Select(x => x.Memo).Contains(CurrentMemo)) return;
                value.Add(new MemoInfo(CurrentMemo, key));
                Memos = new Dictionary<string, ObservableCollection<MemoInfo>>(Memos);
            }
            else
            {
                Memos[key] = [];
                Memos[key].Add(new MemoInfo(CurrentMemo, key));
                Memos = new Dictionary<string, ObservableCollection<MemoInfo>>(Memos);
                WeakReferenceMessenger.Default.Send(new UpdateDashboardMemoInfoMessage(new DashboardMemoInfo(Memos)));
            }
            CurrentMemo = string.Empty;
        }
        [RelayCommand]
        private void DeleteMemo((string Memo, string Key) infos)
        {
            MemoInfo? memoInfo = null;
            if (Memos.TryGetValue(infos.Key, out var value))
            {
                foreach (var info in value)
                {
                    if (info.Memo == infos.Memo)
                    {
                        memoInfo = info;
                        break;
                    }
                }
                if (memoInfo is not null)
                    value.Remove(memoInfo);
                RemoveZeroLengthSubject();
                Memos = new Dictionary<string, ObservableCollection<MemoInfo>>(Memos);          
                WeakReferenceMessenger.Default.Send(new UpdateDashboardMemoInfoMessage(new DashboardMemoInfo(Memos)));
            }
        }
        [RelayCommand]
        private void TogglePopupWindow()
        {
            WeakReferenceMessenger.Default.Send<ToggleDashboardPopupMessage>();
        }
        private void RemoveZeroLengthSubject()
        {
            List<string> keys = [];
            foreach (var pair in Memos)
            {
                if (pair.Value.Count == 0)
                {
                    keys.Add(pair.Key);
                }
            }
            if (keys.Count > 0)
                foreach (var key in keys) Memos.Remove(key);
        }
        public MemoPageViewModel(ThemeViewModel themeViewModel, IWindowService windowService, ILogger<MemoPageViewModel> logger,ApplicationData applicationData) : base(themeViewModel, windowService)
        {
            _logger = logger;
            _applicationData = applicationData;
            Memos = _applicationData.MemoPageData.Memos;
            CurrentSubject = applicationData.MemoPageData.CurrentSubject;
            CurrentMemo = applicationData.MemoPageData.CurrentMemo;
            WeakReferenceMessenger.Default.Register<PullDashboardMemoInfoMessage>(this, (r, m) =>
            {
                WeakReferenceMessenger.Default.Send(new UpdateDashboardMemoInfoMessage(new DashboardMemoInfo(Memos)));
            });
            WeakReferenceMessenger.Default.Register<MemoFilePickedMessage>(this, async (r, m) =>
            {
                string filePath = m.FilePath;
                try
                {
                    string? content = await _memoFileStore.LoadAsync(filePath);
                    if (content is null)
                    {
                        WeakReferenceMessenger.Default.Send(new ShowTipMessage($"文件 {filePath} 不存在", false));
                        return;
                    }
                    Memos = _memoParser.Deserialize(content);
                    WeakReferenceMessenger.Default.Send(new ShowTipMessage($"成功读取存档 科目数: {Memos.Count}", false));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to read memo");
                    WeakReferenceMessenger.Default.Send(new ShowTipMessage("读取文件错误", true));
                }
            });
            WeakReferenceMessenger.Default.Register<ExportMemoMessage>(this, async (r, m) =>
            {
                string folderPath = m.FolderPath;
                string filePath = Path.Combine(folderPath, $"{DateTime.Now:yyMMddHHmm}Memo.txt");
                string content = _memoParser.Serialize(Memos);
                try
                {
                    if (!await _memoFileStore.SaveAsync(filePath, content))
                    {
                        WeakReferenceMessenger.Default.Send(new ShowTipMessage($"文件夹 {folderPath} 不存在", true));
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to export memo");
                    WeakReferenceMessenger.Default.Send(new ShowTipMessage($"保存备忘录失败", true));
                    return;
                }
                WeakReferenceMessenger.Default.Send(new ShowTipMessage($"成功保存备忘录于 {filePath}", false));
            });
        }
    }
}
