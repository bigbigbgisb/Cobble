using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Cobble.ViewModels
{
    
    public partial class ThemeViewModel : ObservableObject
    {
        private static Brush ToBrush(string hexColor)
        {
            Brush? brush;
            try
            {
                brush = new BrushConverter().ConvertFromString(hexColor) as Brush ?? Brushes.Transparent;
            }
            catch
            {
                brush = Brushes.Transparent;
                //此处以后做日志记录
            }
            return brush;
        }

        [ObservableProperty]
        Brush _baseColor = ToBrush("#121418");
        [ObservableProperty]
        Brush _surfaceColor = ToBrush("#1C1F26");
        [ObservableProperty]
        Brush _popupColor = ToBrush("#282C35");
        [ObservableProperty]
        Brush _themeColor = ToBrush("#4476D1");
        [ObservableProperty]
        Brush _mainForeground = ToBrush("#FFFFFF");
        [ObservableProperty]
        Brush _warningForeground = ToBrush("#C33F3F");
        [ObservableProperty]
        Brush _transparentColor = Brushes.Transparent;
    }
}
