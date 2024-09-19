using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Helpers;

public class EnumToIntConverter : IValueConverter
{
    public EnumToIntConverter()
    {
    }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return (int)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return 0; // Not used
    }
}

