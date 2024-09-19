using Microsoft.UI.Xaml;

namespace DB_app.Helpers;

/// <summary>
/// Configuration data wrapper to setup InAppNotification during appropriate event raising
/// </summary>
public class NotificationConfigurationEventArgs : EventArgs
{
    public string Message { get; set; }
    public Style Style   { get; }

    public NotificationConfigurationEventArgs(string message, Style style)
    {
        Message = message;
        Style   = style;
    }
}