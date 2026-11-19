using System;
using System.Collections.Generic;
using System.Text;

namespace Cobble.Models
{
    public enum CobbleTheme
    {
        Dark,
        Light,
        Sea,
        Grape,
        Mint,
        Soda,
        Night,
        Star,
    }
    public record ThemeInfo(string BaseHex, string SurfaceHex, string PopupHex, string ThemeHex, string ForegroundHex,string ThemeName);

}
