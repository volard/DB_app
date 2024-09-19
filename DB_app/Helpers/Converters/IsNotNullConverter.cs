using Microsoft.UI.Xaml.Data;

namespace DB_app.Helpers;


public class IsNotNullConverter : IValueConverter
{
public object Convert(object value, Type targetType, object parameter, string language)
{
    return value is not null;
}

public object ConvertBack(object value, Type targetType, object parameter, string language)
{
    // not used
    return false;
}
}