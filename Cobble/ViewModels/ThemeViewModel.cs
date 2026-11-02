using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Windows.Media;



namespace Cobble.ViewModels
{
    
    public partial class ThemeViewModel : ObservableObject
    {
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
        Color _shadowColor = Colors.White;
        [ObservableProperty]
        Brush _warningForeground;
        [ObservableProperty]
        Brush _transparentBrush = Brushes.Transparent;
        public ThemeViewModel(ILogger<ThemeViewModel> logger)
        {
            _logger = logger;
            BaseBrush = ToBrush("#0E1116");
            BaseColor = ToColor("#0E1116");
            SurfaceBrush = ToBrush("#171C24");
            SurfaceColor = ToColor("#171C24");
            PopupBrush = ToBrush("#222A35");
            ThemeBrush = ToBrush("#4476D1");
            MainForeground = ToBrush("#FFFFFF");
            ShadowColor = Colors.White;
            WarningForeground = ToBrush("#C33F3F");
            TransparentBrush = Brushes.Transparent;
        }
    }
}
