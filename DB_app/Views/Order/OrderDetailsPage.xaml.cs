using AppUIBasics.Helper;
using CommunityToolkit.Mvvm.ComponentModel;
using DB_app.Behaviors;
using DB_app.Entities;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;

namespace DB_app.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class OrderDetailsPage : Page
{
    public OrderDetailsViewModel ViewModel { get; }

    public OrderDetailsPage()
    {
        ViewModel = App.GetService<OrderDetailsViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    
    public OrderDetailsPage(OrderDetailsViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });

    }

    public void GetAvailableProducts(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SelectedPharmacy = (Pharmacy)PharmacySellerComboBox.SelectedItem;
    }

    // Create a new Window once the Tab is dragged outside.
    private void Tabs_TabDroppedOutside(object sender, RoutedEventArgs args)
    {
        var newPage = new OrderDetailsPage(ViewModel);
        var orderDetailWindow = new OrderDetailsWindow(newPage);

        orderDetailWindow.Activate();
        Frame.GoBack();
    }


    //public void AddSelectedButton_Clicked(object sender, RoutedEventArgs e)
    //{
    //    if (ViewModel.SelectedAddress != null)
    //    {
    //        ViewModel.CurrentOrder.OrderData.Addresses.Add(ViewModel.SelectedAddress);
    //        ViewModel.CurrentOrder.IsModified = true;
    //        ViewModel.CurrentOrder.NotifyAboutAddressesChanged();
    //        ViewModel.AvailableAddresses.Remove(ViewModel.SelectedAddress);
    //    }
    //}

    //public void DeleteSelectedButton_Clicked(object sender, RoutedEventArgs e)
    //{
    //    if (ViewModel.SelectedExistingAddress != null)
    //    {
    //        ViewModel.AvailableAddresses.Add(ViewModel.SelectedExistingAddress);
    //        ViewModel.CurrentOrder.OrderData.Addresses.Remove(ViewModel.SelectedExistingAddress);
    //        ViewModel.CurrentOrder.IsModified = true;
    //        ViewModel.CurrentOrder.NotifyAboutAddressesChanged();
    //    }
    //}

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SaveAsync();
        ViewModel.NotifyGridAboutChange();
        if (!WindowHelper.ActiveWindows.Any())
        {
            Frame.Navigate(typeof(OrdersGridPage), null);
        }
        
    }

    /// <summary>
    /// Navigate to the previous page when the user cancels the creation of a new record.
    /// </summary>
    private void CancelEdit_Click(object sender, RoutedEventArgs e) => Frame.GoBack();

    /// <summary>
    /// Check whether there are unsaved changes and warn the user.
    /// </summary>
    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        // TODO implement this
    }


    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel.CurrentOrder.BuckupData();
        ViewModel.CurrentOrder.NotifyAboutProperties();
        base.OnNavigatedTo(e);
    }
}
