using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DB_app.Helpers;

public class StringVisibilityConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not string s) { throw new ArgumentException($"The value must be string type");  }
        return string.IsNullOrEmpty(s) ? Visibility.Collapsed : Visibility.Visible;
    }


    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // not used
        return "";
    }
}
