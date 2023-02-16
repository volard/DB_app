
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DB_app.Helpers;



public class CollapsedIf : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolValue)
        {
            if (boolValue)
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }
        throw new ArgumentException("value must be bool type");
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // not used
        return false;
    }
}
