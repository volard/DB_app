using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_app.Helpers;

public class DoubleToIntConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is double num)
        {
            return (int)num;
        }
        else { return (int)0; }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is int num)
        {
            return (double)num;
        }
        else { return (double)0; }
    }

}
