using Cobble.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Windows.Media;
namespace Cobble.ViewModels
{

    public partial class ThemeViewModel : ObservableObject
    {
        [ObservableProperty]
        Dictionary<CobbleTheme, ThemeInfo> _themeInfos = new()
        {
            [CobbleTheme.Dark] = new("#0E1116", "#171C24", "#222A35", "#4476D1", "#FFFFFF", "深色"),
            [CobbleTheme.Light] = new("#E8E6E3", "#F2F0ED", "#FAF9F6", "#81A2DD", "#1A1A1A", "浅色"),
            [CobbleTheme.Sea] = new("#F4F7FA", "#EBF0F5", "#E2E9F0", "#639CD2", "#1A1A1A", "海洋"),
            [CobbleTheme.Grape] = new("#F6F4F8", "#EDEAF1", "#E4E1EA", "#9477BD", "#1A1A1A", "葡萄"),
            [CobbleTheme.Mint] = new("#F4F8F7", "#EBF1EF", "#E2E9E7", "#58A897", "#1A1A1A", "薄荷"),
            [CobbleTheme.Soda] = new("#F8F4F6", "#F1E9ED", "#EAE0E4", "#D8789E", "#1A1A1A", "汽水"),
            [CobbleTheme.Night] = new("#0A0D14", "#101520", "#182030", "#7289DA", "#FFFFFF", "永夜"),
            [CobbleTheme.Star] = new("#1A191E", "#27252C", "#333138", "#E6AA70", "#FFFFFF", "星尘"),
        };
        [ObservableProperty]
        CobbleTheme _currentTheme;
        private readonly ILogger<ThemeViewModel> _logger;
        private Brush ToBrush(string hexColor)
        {
            Brush? brush;
            try
            {
                brush = new BrushConverter().ConvertFromString(hexColor) as Brush ?? Brushes.Transparent;
            }
            catch
            {
                brush = Brushes.Transparent;
                _logger.LogInformation("Cannot convert hex value {Hex}", hexColor);
            }
            return brush;
        }
        private Color ToColor(string hexColor)
        {
            try
            {
                return (Color)ColorConverter.ConvertFromString(hexColor);
            }
            catch
            {
                _logger.LogInformation("Cannot convert hex value {Hex}", hexColor);
                return Colors.White;
            }
        }
        private ApplicationData _applicationData;
        [ObservableProperty]
        Brush _baseBrush;
        [ObservableProperty]
        Color _baseColor;
        [ObservableProperty]
        Brush _surfaceBrush;
        [ObservableProperty]
        Color _surfaceColor;
        [ObservableProperty]
        Brush _popupBrush;
        [ObservableProperty]
        Brush _themeBrush;
        [ObservableProperty]
        Brush _mainForeground;
        [ObservableProperty]
        Color _shadowColor;
        [ObservableProperty]
        Brush _warningForeground;
        [ObservableProperty]
        Brush _transparentBrush = Brushes.Transparent;
        public ThemeViewModel(ILogger<ThemeViewModel> logger,ApplicationData applicationData)
        {
            _applicationData = applicationData;
            _logger = logger;
            CurrentTheme = CobbleTheme.Dark;
            BaseBrush = ToBrush("#0E1116");
            BaseColor = ToColor("#0E1116");
            SurfaceBrush = ToBrush("#171C24");
            SurfaceColor = ToColor("#171C24");
            PopupBrush = ToBrush("#222A35");
            ThemeBrush = ToBrush("#4476D1");
            MainForeground = ToBrush("#FFFFFF");
            ShadowColor = ToColor("#0E1116");
            WarningForeground = ToBrush("#C33F3F");
            TransparentBrush = Brushes.Transparent;
        }
        partial void OnCurrentThemeChanged(CobbleTheme value)
        {
            ChangeTheme(value);
            _applicationData.GlobalSettingsData.CobbleTheme = value;
        }
        public void ChangeTheme(CobbleTheme theme)
        {
            var infos = ThemeInfos[theme];
            BaseBrush = ToBrush(infos.BaseHex);
            BaseColor = ToColor(infos.BaseHex);
            SurfaceBrush = ToBrush(infos.SurfaceHex);
            SurfaceColor = ToColor(infos.SurfaceHex);
            PopupBrush = ToBrush(infos.PopupHex);
            ThemeBrush = ToBrush(infos.ThemeHex);
            MainForeground = ToBrush(infos.ForegroundHex);
        }
    }
}
