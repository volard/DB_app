﻿using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using DB_app.Helpers;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using DB_app.Contracts.Services;

namespace DB_app.Views;


public sealed partial class PharmaciesGridPage : Page
{
    public PharmaciesGridViewModel ViewModel { get; } = App.GetService<PharmaciesGridViewModel>();

    public PharmaciesGridPage()
    {
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    private void ShowNotificationMessage(object? sender, NotificationConfigurationEventArgs e)
    {
        Notification.Content = e.Message;
        Notification.Style = e.Style;
        Notification.Show(2000);
    }

    /**************************************/
    #region Navigation Handlers
    /**************************************/

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel.OperationRejected += ShowNotificationMessage;

        if (ViewModel.Source.Count >= 1) return;
        ViewModel.Load();

        base.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        ViewModel.OperationRejected -= ShowNotificationMessage;
        base.OnNavigatedFrom(e);
    }

    #endregion


    /**************************************/
    #region AppBar button click handlers
    /**************************************/

    private void Add_Click(object sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(PharmacyDetailsPage), new PharmacyWrapper() { IsInEdit = true }, new DrillInNavigationTransitionInfo());


    private void View_Click(object sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(PharmacyDetailsPage), ViewModel.SelectedItem, new DrillInNavigationTransitionInfo());



    private async void Delete_Click(object sender, RoutedEventArgs e) =>
        await ViewModel.DeleteSelected();


    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedItem == null) return;

        ViewModel.SelectedItem!.IsInEdit = true;
        App.GetService<INavigationService>().NavigateTo(typeof(PharmacyDetailsViewModel).FullName!, ViewModel.SelectedItem);
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.ToggleInactive();
    }

    #endregion
}
