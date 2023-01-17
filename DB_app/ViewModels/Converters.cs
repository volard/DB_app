using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DB_app.ViewModels;

/// <summary>
/// Provides static methods for use in x:Bind function binding to convert bound values to the required value.
/// </summary>
public static class Converters // TODO all this class is peace of shit https://stackoverflow.com/questions/4450866/conditional-element-in-xaml-depending-on-the-binding-content
{
    /// <summary>
    /// Returns the reverse of the provided value.
    /// </summary>
    public static bool Not(bool value)
    {
        return !value;
    }

    /// <summary>
    /// Returns true if the specified value is not null; otherwise, returns false.
    /// </summary>
    public static bool IsNotNull(object? value)
    {
        if (value == null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Returns Visibility.Collapsed if the specified value is true; otherwise, returns Visibility.Visible.
    /// </summary>
    public static Visibility CollapsedIf(bool value) =>
        value ? Visibility.Collapsed : Visibility.Visible;

    /// <summary>
    /// Returns Visibility.Visible if the specified value is true; otherwise, returns Visibility.Collapsed.
    /// </summary>
    public static Visibility VisibleIf(bool value) =>
        value ? Visibility.Visible : Visibility.Collapsed;

    /// <summary>
    /// Returns Visibility.Collapsed if the specified value is null; otherwise, returns Visibility.Visible.
    /// </summary>
    public static Visibility CollapsedIfNull(object value) =>
        value == null ? Visibility.Collapsed : Visibility.Visible;

    /// <summary>
    /// Returns Visibility.Collapsed if the specified string is null or empty; otherwise, returns Visibility.Visible.
    /// </summary>
    public static Visibility CollapsedIfNullOrEmpty(string value) =>
        string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
}


public class StringVisiblityConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (string.IsNullOrEmpty(((string)value)))
        {
            return Visibility.Collapsed;
        }
        return Visibility.Visible;
    }


    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}