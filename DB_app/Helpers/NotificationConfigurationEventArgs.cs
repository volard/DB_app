using Microsoft.UI.Xaml;

namespace DB_app.Helpers;

/// <summary>
/// Configuration data wrapper to setup InAppNotification during apropriate event raising
/// </summary>
public class NotificationConfigurationEventArgs : EventArgs
{
    public string Message { get; set; }

    public Style? Style { get; }

    public NotificationConfigurationEventArgs(string message, ApperienceType type)
    {
        Message = message;
        Style = type switch
        {
            ApperienceType.Success => Application.Current.Resources["SuccessInAppNavigationStyle"] as Style,
            ApperienceType.Error   => Application.Current.Resources["ErrorInAppNavigationStyle"] as Style,
            ApperienceType.Info    => Application.Current.Resources["InfoInAppNavigationStyle"] as Style,
            _                      => throw new ArgumentException($"{nameof(type)} not expected ApperienceType value: {type}"),
        };
        if (Style is null) throw new Exception("Provided InAppNotification style not found in the project");
    }
}

public enum ApperienceType
{
    Success,
    Error,
    Info
}