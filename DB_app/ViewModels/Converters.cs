using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DB_app.ViewModels;

/// <summary>
/// Provides static methods for use in x:Bind function binding to convert bound values to the required value.
/// </summary>
public static class Converters{
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


    public static bool DisabledIfNullOrEmpty(string value) =>
        !string.IsNullOrEmpty(value);

    public static Visibility AndVisibility(bool lhs, bool rhs)
    {
        if (lhs && rhs) return Visibility.Visible; else return Visibility.Collapsed;
    }


    public static Visibility CollapsedIfOneOfTwo(bool lhs, bool rhs)
    {
        if (lhs || rhs) return Visibility.Collapsed; else return Visibility.Visible;
    }

    public static Visibility VisibleIfOneAndNotAnother(bool lhs, bool rhs)
    {
        if (lhs && !rhs) return Visibility.Visible; else return Visibility.Collapsed;
    }
}
