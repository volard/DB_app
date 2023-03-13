using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Helpers;

// Thanks https://stackoverflow.com/questions/34026332/string-format-using-uwp-and-xbind#34026544
public class StringFormatConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value == null)
            return "";

        if (parameter == null)
            return "";

        if (value is DateTime moment)
        {
            return moment.ToString((string)parameter);
        }

        return string.Format(CultureInfo.InvariantCulture,"{" + (string)parameter + "}", value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
