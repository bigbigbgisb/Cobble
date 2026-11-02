using Cobble.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Cobble.Messages;
using System.Collections.ObjectModel;
using Cobble.Models;
using System.IO;
using System.Windows.Controls;
using System.Diagnostics;
using Microsoft.Extensions.Logging;


namespace Cobble.ViewModels
{
    public partial class MemoPageViewModel : PageViewModelBase
    {
        public record MemoInfo(string Memo, string Key);
        private ILogger<MemoPageViewModel> _logger;
        [ObservableProperty]
        Dictionary<string, ObservableCollection<MemoInfo>> _memos = [];
        [ObservableProperty]
        string? _currentSubject;
        [ObservableProperty]
        string? _currentMemo;
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
        public MemoPageViewModel(ThemeViewModel themeViewModel, IWindowService windowService,ILogger<MemoPageViewModel> logger) : base(themeViewModel, windowService)
        {
            _logger = logger;
            WeakReferenceMessenger.Default.Register<PullDashboardMemoInfoMessage>(this, (r, m) =>
            {
                WeakReferenceMessenger.Default.Send(new UpdateDashboardMemoInfoMessage(new DashboardMemoInfo(Memos)));
            });
            WeakReferenceMessenger.Default.Register<MemoFilePickedMessage>(this, async (r, m) =>
            {
                string filePath = m.FilePath;
                if (File.Exists(filePath))
                {
                    Dictionary<string, ObservableCollection<MemoInfo>> parsed = [];
                    await Task.Run(async () =>
                    {
                        string[] content = await File.ReadAllLinesAsync(filePath);
                        string key = "其他";
                        foreach (var line in content)
                        {
                            if (string.IsNullOrWhiteSpace(line)) continue;
                            if (line.Length < 4) key = line;
                            else if (line[0..4] != "    ") key = line;
                            if (!parsed.ContainsKey(key))
                                parsed[key] = [];
                            if (key != line)
                                parsed[key].Add(new MemoInfo(line[4..], key));
                        }
                    });
                    Memos = parsed;
                    Memos = new Dictionary<string, ObservableCollection<MemoInfo>>(Memos);
                    WeakReferenceMessenger.Default.Send(new UpdateDashboardMemoInfoMessage(new DashboardMemoInfo(Memos)));
                    WeakReferenceMessenger.Default.Send(new ShowTipMessage($"成功读取存档 条目数: {parsed.Count}", false));
                }
                else
                {
                    WeakReferenceMessenger.Default.Send(new ShowTipMessage($"文件 {filePath} 不存在", false));
                }
            });
            WeakReferenceMessenger.Default.Register<ExportMemoMessage>(this, async (r, m) =>
            {
                string folderPath = m.FolderPath;
                if (Directory.Exists(folderPath))
                {
                    string filePath = Path.Combine(folderPath, $"{DateTime.Now:yyMMddHHmm}Memo.txt");
                    string content = "";
                    foreach (var pair in Memos)
                    {
                        content += $"{pair.Key}{Environment.NewLine}";
                        foreach (var item in pair.Value)
                        {
                            content += $"    {item.Memo}{Environment.NewLine}";
                        }
                    }
                    try
                    {
                        await File.WriteAllTextAsync(filePath, content);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to export memo");
                    }
                    WeakReferenceMessenger.Default.Send(new ShowTipMessage($"成功保存备忘录于 {filePath}", false));
                }
                else
                {
                    WeakReferenceMessenger.Default.Send(new ShowTipMessage($"文件夹 {folderPath} 不存在", true));
                }
            });
        }
    }
}
