using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChiefsKiss.Converters
{
    public class RoleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!int.TryParse(value.ToString(), out var roleId) || !(parameter is string roleName))
                return Visibility.Collapsed;

            switch (roleName)
            {
                case "К":
                    if (roleId == 1)
                        return Visibility.Visible;
                    break;
                case "М":
                    if (roleId == 2)
                        return Visibility.Visible;
                    break;
                case "А":
                    if (roleId == 2)
                        return Visibility.Visible;
                    break;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
