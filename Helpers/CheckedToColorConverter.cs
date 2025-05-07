using System.Globalization;

namespace swissbyte.Helpers
{
    public class CheckedToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var appResources = Application.Current.Resources;
            return (bool)value ? appResources["CheckedItemText"] : appResources["GrayText"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
