using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DB_app.Helpers;

class VisibleIfNotNull : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not null)
        {
            return Visibility.Visible;
        }
        else
        {
            return Visibility.Collapsed;
        }
    }

    public object? ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is Visibility model)
        {
            if (model == Visibility.Visible)
            {
                return true;
            }
            else { return null; }
        }
        throw new ArgumentException("Value must be of Visibility type");
    }
}
