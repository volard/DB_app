using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Windows.UI.Notifications;

namespace DB_app.Helpers;

public static class NotificationHelper
{
    public static readonly Style SuccessStyle = (Application.Current.Resources["SuccessInAppNavigationStyle"] as Style)!;
    public static readonly Style ErrorStyle   = (Application.Current.Resources["ErrorInAppNavigationStyle"]   as Style)!;
    public static readonly Style InfoStyle    = (Application.Current.Resources["InfoInAppNavigationStyle"]    as Style)!;

    public enum NotificationType
    {
        Success,
        Error,
        Info
    }

    public static void ShowNotificationMessage(InAppNotification notification, string message, Style style, int duration = 2000)
    {
        notification.Content = message;
        notification.Style = style;
        notification.Show(duration);
    }
}