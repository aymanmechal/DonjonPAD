using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace ProjetPAD.UI.Converters
{
    public class MissionStatusConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool b && b
                ? "⛔ En mission"
                : "✅ Disponible";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}