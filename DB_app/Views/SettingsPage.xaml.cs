using DB_app.Helpers;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Notifications;

namespace DB_app.Views;
using WASDK = Microsoft.WindowsAppSDK;

// Reduces warning noise on parameters that are needed for signature requirements
#pragma warning disable IDE0060

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel { get; } = App.GetService<SettingsViewModel>();

    public SettingsPage()
    {
        InitializeComponent();
    }

    public static string WinAppSdkDetails => string.Format("Windows App SDK {0}.{1}.{2}{3}",
            WASDK.Release.Major, WASDK.Release.Minor, WASDK.Release.Patch, WASDK.Release.VersionShortTag);

    public string AppTitleName = "AppDisplayName".GetLocalizedValue();


    private async void bugRequestCard_Click(object sender, RoutedEventArgs e)
    {
        await Launcher.LaunchUriAsync(new Uri("https://github.com/volard/DB_app/issues/new/choose"));
    }


    private void SettingsCard_Click(object sender, RoutedEventArgs e)
    {
        var package = new DataPackage();
        package.SetText("git clone https://github.com/volard/DB_app");
        Clipboard.SetContent(package);


        Notification.Content = "Copied to clipboard";
        Notification.Style = GetNotificationStyle(NotificationType.Success);
        Notification.Show(1500);
    }


    public static Style GetNotificationStyle(NotificationType type)
    {
        var output = type switch
        {
            NotificationType.Success => Application.Current.Resources["SuccessInAppNavigationStyle"] as Style,
            NotificationType.Error => Application.Current.Resources["ErrorInAppNavigationStyle"] as Style,
            NotificationType.Info => Application.Current.Resources["InfoInAppNavigationStyle"] as Style,
            _ => throw new ArgumentException($"{nameof(type)} not expected ApperienceType value: {type}"),
        };

        return output!;
    }

    
}


#pragma warning restore IDE0060