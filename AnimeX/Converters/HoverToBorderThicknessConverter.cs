using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace AnimeX.Converters
{
    public class HoverToBorderThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isHovered && isHovered)
            {
                return new Thickness(2);
            }
            else
            {
                return new Thickness(1);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
