using System.Globalization;
using System.Windows.Data;

namespace Cobble.Converters
{
    public sealed class TwoArgStringTupleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            
            if (values.Length < 2) throw new ArgumentException($"Cannot convert {values.Length} arg(s) to a 2-arg tuple");
            return ((string)values[0], (string)values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Converting a tuple to args is not supported");
        }
    }
}
