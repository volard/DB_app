using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DB_app.Helpers;

public class EnumToBooleanConverter : IValueConverter
{
    public EnumToBooleanConverter()
    {
    }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (parameter is string enumString)
        {
            if (!Enum.IsDefined(typeof(ElementTheme), value))
            {
                throw new ArgumentException("Value must be an enum");
            }

            var enumValue = Enum.Parse(typeof(ElementTheme), enumString);

            return enumValue.Equals(value);
        }

        throw new ArgumentException("Parameter must be an enum name");
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (parameter is string enumString)
        {
            return Enum.Parse(typeof(ElementTheme), enumString);
        }

        throw new ArgumentException("Parameter must be an enum name");
    }
}
