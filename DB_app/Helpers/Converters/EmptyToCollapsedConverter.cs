using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System.Collections;

namespace DB_app.Helpers;

public class EmptyToCollapsedConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value == null)
            return Visibility.Collapsed;
        else
        {
            if (value is ICollection list)
            {
                if (list.Count == 0)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
            else
                return Visibility.Visible;
        }
    }


    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // not used
        return "";
    }
}
