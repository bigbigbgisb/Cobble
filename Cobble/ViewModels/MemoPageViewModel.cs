using Cobble.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Ink;

namespace Cobble.ViewModels
{
    public partial class MemoPageViewModel(ThemeViewModel themeViewModel, IWindowService windowService) : PageViewModelBase(themeViewModel, windowService)
    {
        public record MemoInfo(string Memo, string Key);
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
            string key = CurrentSubject ?? replaceText;
            if (Memos.TryGetValue(key, out var value))
            {
                if (value.Select(x => x.Memo).Contains(CurrentMemo)) return;
                value.Add(new MemoInfo(CurrentMemo, key));
            }
            else
            {
                Memos[key] = [];
                Memos[key].Add(new MemoInfo(CurrentMemo, key));
            }
            CurrentMemo = string.Empty;
            //触发 PropertyChanged
            Memos = new Dictionary<string, ObservableCollection<MemoInfo>>(Memos);

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
            }
        }
        private void RemoveZeroLengthSubject()
        {
            List<string> keys = [];
            foreach(var pair in Memos)
            {
                if (pair.Value.Count == 0)
                {
                    keys.Add(pair.Key);
                }
            }
            if (keys.Count > 0)
                foreach (var key in keys) Memos.Remove(key);
        }
    }
}
