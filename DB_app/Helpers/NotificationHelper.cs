using Microsoft.UI.Xaml;

namespace DB_app.Helpers;

public class NotificationHelper
{
    public static readonly Style SuccessStyle = (Application.Current.Resources["SuccessInAppNavigationStyle"] as Style)!;
    public static readonly Style ErrorStyle  = (Application.Current.Resources["ErrorInAppNavigationStyle"]   as Style)!;
    public static readonly Style InfoStyle   = (Application.Current.Resources["InfoInAppNavigationStyle"]    as Style)!;

    public enum NotificationType
    {
        Success,
        Error,
        Info
    }
}