using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System.Collections;

namespace DB_app.Helpers;

public class EmptyToVisibleConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value == null)
            return Visibility.Visible;
        else
        {
            if (value is ICollection list)
            {
                if (list.Count == 0)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else
                return Visibility.Collapsed;
        }
    }


    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // not used
        return "";
    }
}
