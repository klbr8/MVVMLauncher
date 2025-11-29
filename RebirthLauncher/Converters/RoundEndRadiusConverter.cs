using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RebirthLauncher.Converters
{
    internal class RoundEndRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height && height > 0)
            {
                return new CornerRadius(height / 2); // Calculate half the height
            }
            return new CornerRadius(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
