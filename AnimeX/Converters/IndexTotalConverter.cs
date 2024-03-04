using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace AnimeX.Converters
{
    public class IndexTotalConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count != 2)
                return null;

            int currentIndex = System.Convert.ToInt32(values[0]) + 1;
            int totalAnimes = System.Convert.ToInt32(values[1]);

            return $"{currentIndex} / {totalAnimes}";
        }
    }
}
