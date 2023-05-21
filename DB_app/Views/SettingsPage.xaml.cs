using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;

namespace DB_app.Views;

// ReSharper disable once IdentifierTypo
using WASDK = Microsoft.WindowsAppSDK;

// Reduces warning noise on parameters that are needed for signature requirements
#pragma warning disable IDE0060

public sealed partial class SettingsPage
{
    public SettingsViewModel ViewModel { get; } = App.GetService<SettingsViewModel>();

    private MediaWindow? _mediaWindow;

    /// <summary>
    /// Initializes the page.
    /// </summary>
    public SettingsPage()
    {
        InitializeComponent();
    }

    public static string WinAppSdkDetails => $"Windows App SDK {WASDK.Release.Major}.{WASDK.Release.Minor}.{WASDK.Release.Patch}{WASDK.Release.VersionShortTag}";

    public readonly string AppTitleName = "AppDisplayName".GetLocalizedValue();


    private async void bugRequestCard_Click(object sender, RoutedEventArgs e)
    {
        await Launcher.LaunchUriAsync(new Uri("https://github.com/volard/DB_app/issues/new/choose"));
    }


    private void SettingsCard_Click(object sender, RoutedEventArgs e)
    {
        DataPackage package = new DataPackage();
        package.SetText("git clone https://github.com/volard/DB_app");
        Clipboard.SetContent(package);


        Notification.Content = "Copied to clipboard";
        Notification.Style = NotificationHelper.SuccessStyle;
        Notification.Show(1500);
    }


    /**************************************/
    #region Navigation Handlers
    /**************************************/


    //private void MediaWindow_Closed(object sender, WindowEventArgs args)
    //{
    //    _mediaWindow = new()
    //    {
    //        IsMinimizable = false,
    //        IsAlwaysOnTop = true,
    //        IsResizable = false,
    //        IsShownInSwitchers = false,
    //        IsMaximizable = false,
    //        IsTitleBarVisible = false
    //    };
    //    _mediaWindow.Maximize();
    //    _mediaWindow.Activate();
    //}
    


    #endregion




    private void SettingsCard_Click_1(object sender, RoutedEventArgs e)
    {

        // Restart UI and push VMs to use new data
        App.GetService<HospitalsGridViewModel>().Source.Clear();
        App.GetService<AddressesGridViewModel>().Source.Clear();
        App.GetService<OrdersGridViewModel>().Source.Clear();
        App.GetService<ProductsGridViewModel>().Source.Clear();
        App.GetService<MedicinesGridViewModel>().Source.Clear();
        App.GetService<PharmaciesGridViewModel>().Source.Clear();


        App.GetService<IRepositoryControllerService>().RestartDataBase();

        Notification.Content = "Data restored";
        Notification.Style = NotificationHelper.SuccessStyle;
        Notification.Show(1500);
    }

    private void supportCard_Click(object sender, RoutedEventArgs e)
    {
        if(SoundToggle.IsOn)
        {
            _mediaWindow = new MediaWindow
            {
                IsMinimizable = false,
                IsAlwaysOnTop = true,
                IsResizable = false,
                IsShownInSwitchers = false,
                IsMaximizable = false,
                IsTitleBarVisible = false
            };
            _mediaWindow.Maximize();
            _mediaWindow.Activate();
            //_mediaWindow.Closed += MediaWindow_Closed;
        }
    }

}


#pragma warning restore IDE0060