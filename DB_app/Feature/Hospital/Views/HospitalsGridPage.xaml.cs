using CommunityToolkit.WinUI.UI.Controls;
using DB_app.Behaviors;
using DB_app.Contracts.Services;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using DB_app.ViewModels;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;


namespace DB_app.Views;

// Reduces warning noise on parameters that are needed for signature requirements
#pragma warning disable IDE0060
public sealed partial class HospitalsGridPage : Page
{
    public HospitalsGridViewModel ViewModel { get; } = App.GetService<HospitalsGridViewModel>();

    /// <summary>
    /// Raises the <see cref="ViewModel.DisplayInAppNotification"/> event.
    /// </summary>
    /// <param name="e">The input <see cref="NotificationConfigurationEventArgs"/> instance.</param>
    private void ShowNotificationMessage(object? sender, NotificationConfigurationEventArgs e)
    {
        Notification.Content = e.Message;
        Notification.Style = e.Style;
        Notification.Show(2000);
    }

    /// <summary>
    /// Initializes the page.
    /// </summary>
    public HospitalsGridPage()
    {
        InitializeComponent();

        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    /**************************************/
    #region Navigation Handlers
    /**************************************/

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        //ViewModel.DisplayInAppNotification += ShowNotificationMessage;

        if (ViewModel.Source.Count >= 1) return;
        ViewModel.Load();
    }


    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        ViewModel.DisplayInAppNotification -= ShowNotificationMessage;
        base.OnNavigatedFrom(e);
    }


    #endregion


    /**************************************/
    #region AppBar button click handlers
    /**************************************/

    private void Add_Click(object? sender, RoutedEventArgs e) =>
        App.GetService<INavigationService>().NavigateTo(typeof(HospitalDetailsViewModel).FullName!, new HospitalWrapper() { IsNew = true, IsInEdit = true });


    private void View_Click(object? sender, RoutedEventArgs e) =>
        App.GetService<INavigationService>().NavigateTo(typeof(HospitalDetailsViewModel).FullName!, ViewModel.SelectedItem);


    private void Delete_Click(object? sender, RoutedEventArgs e) =>
        _ = ViewModel.DeleteSelected();


    private void Edit_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedItem == null) return;

        ViewModel.SelectedItem.IsInEdit = true;
        App.GetService<INavigationService>().NavigateTo(typeof(HospitalDetailsViewModel).FullName!, ViewModel.SelectedItem);
    }

    private async void ToggleInactive_Click(object sender, RoutedEventArgs e)
        => await ViewModel.ToggleInactive();

    #endregion


}

#pragma warning restore IDE0060