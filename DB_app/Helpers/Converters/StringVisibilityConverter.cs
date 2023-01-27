using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DB_app.Helpers;

public class StringVisiblityConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not string) { throw new ArgumentException($"The value must be string type");  }
        if (string.IsNullOrEmpty(((string)value)))
        {
            return Visibility.Collapsed;
        }
        return Visibility.Visible;
    }


    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // not used
        return "";
    }
}
