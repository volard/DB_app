﻿using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using DB_app.Helpers;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using DB_app.Contracts.Services;

namespace DB_app.Views;


public sealed partial class ProductsGridPage : Page
{
    public ProductsGridViewModel ViewModel { get; }

    public ProductsGridPage()
    {
        ViewModel = App.GetService<ProductsGridViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel.DisplayNotification += ShowNotificationMessage;

        if (ViewModel.Source.Count >= 1) return;
        ViewModel.Load();

        base.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        ViewModel.DisplayNotification-= ShowNotificationMessage;
        base.OnNavigatedFrom(e);
    }

    private void ShowNotificationMessage(object? sender, NotificationConfigurationEventArgs e)
    {
        Notification.Content = e.Message;
        Notification.Style = e.Style;
        Notification.Show(2000);
    }

    private void Add_Click(object? sender, RoutedEventArgs e) =>
        App.GetService<INavigationService>().NavigateTo(typeof(ProductDetailsViewModel).FullName!, new ProductWrapper() { IsInEdit = true });


    private void View_Click(object? sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(ProductDetailsPage), ViewModel.SelectedItem, new DrillInNavigationTransitionInfo());



    private async void Delete_Click(object? sender, RoutedEventArgs e) =>
        await ViewModel.DeleteSelected();


    private void Edit_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel.SelectedItem.IsInEdit = true;
        App.GetService<INavigationService>().NavigateTo(typeof(ProductDetailsViewModel).FullName!, ViewModel.SelectedItem);
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.ToggleOutOfStock();
    }
}
