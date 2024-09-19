using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DB_app.Helpers;

public class VisibleIfConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }
        throw new ArgumentException("value must be bool type");
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // not used
        return false;
    }
}
