using Cobble.Abstractions;
using Cobble.Messages;
using Cobble.Models;
using Cobble.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;
using WindowsAutostartApi.Abstractions;
using WindowsAutostartApi.Core;

namespace Cobble.ViewModels
{
    public partial class EasterEggPageViewModel : PageViewModelBase
    {
        private readonly string[] _musicNames = ["girlfromsmalltown.mp3"];
        private CodeFileStore _codeFileStore = new();
        private MediaPlayer _mediaPlayer = new();
        [ObservableProperty]
        int _linesOfCode = 0;
        [ObservableProperty]
        int _charsOfCode = 0;
        [ObservableProperty]
        ICSharpCode.AvalonEdit.Document.TextDocument _code = new();
        private bool _isPlaying = false;
        private ApplicationData _data;
        ILogger<EasterEggPageViewModel> _logger;
        [RelayCommand]
        private void EnableAutoStart()
        {
            var manager = new StartupManager();
            try
            {
                manager.Add(new(Name: "Cobble", TargetPath: Environment.ProcessPath!, Scope: StartupScope.CurrentUser, Kind: StartupKind.Run, Arguments: null));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "设置开机自启动失败");
                WeakReferenceMessenger.Default.Send(new ShowTipMessage("设置开机自启动失败", true));
            }
            _data.IsAllowAutoStart = true;


        }
        [RelayCommand]
        private void DisableAutoStart()
        {
            var manager = new StartupManager();
            try
            {
                manager.Remove("Cobble", WindowsAutostartApi.Abstractions.StartupScope.CurrentUser, StartupKind.Run);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "关闭开机自启动失败");
                WeakReferenceMessenger.Default.Send(new ShowTipMessage("关闭开机自启动失败", true));
            }
            _data.IsAllowAutoStart = false;
        }

        [RelayCommand]
        private void PlaySong()
        {
            if (_isPlaying)
            {
                _mediaPlayer.Pause();
                _isPlaying = false;
            }
            else
            {
                _mediaPlayer.Play();
                _isPlaying = true;
            }
        }
        private async void LoadCode()
        {
            Code = new(await _codeFileStore.LoadAsync(AppContext.BaseDirectory) ?? "");
            LinesOfCode = Code.Text.Split(Environment.NewLine).Length;
            CharsOfCode = Code.Text.Length;
        }
        private string GetRandomSongPath()
        {
            int randomNumber = Random.Shared.Next(_musicNames.Length);
            return Path.Combine(AppContext.BaseDirectory, "Assets", _musicNames[randomNumber]);
        }
        public EasterEggPageViewModel(ThemeViewModel themeViewModel, IWindowService windowService, ApplicationData applicationData, ILogger<EasterEggPageViewModel> logger) : base(themeViewModel, windowService)
        {
            _logger = logger;
            _data = applicationData;
            LoadCode();
            _mediaPlayer.Open(new Uri(GetRandomSongPath()));
            _mediaPlayer.MediaEnded += (s, e) =>
            {
                _mediaPlayer.Position = TimeSpan.Zero;
                _mediaPlayer.Open(new Uri(GetRandomSongPath()));
                _mediaPlayer.Play();
            };
        }
    }
}
