using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI.Controls;
using DB_app.Behaviors;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using DB_app.Services.Messages;
using DB_app.ViewModels;
using DB_app.Views.Components;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace DB_app.Views;


public sealed partial class OrderDetailsPage : Page
{

    public OrderDetailsViewModel ViewModel { get; } = App.GetService<OrderDetailsViewModel>();


    /**************************************/
    #region Constructors

    public OrderDetailsPage()
    {
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }


    public OrderDetailsPage(OrderDetailsViewModel viewModel) : this()
    {
        ViewModel = viewModel;
    }


    #endregion
    /**************************************/
    

    /// <summary>
    /// Create a new Window once the Tab is dragged outside.
    /// </summary>
    private void Tabs_TabDroppedOutside(object sender, RoutedEventArgs args)
    {
        OrderDetailsPage newPage = new OrderDetailsPage(ViewModel);
        OrderDetailsWindow orderDetailWindow = new OrderDetailsWindow(newPage);

        orderDetailWindow.Activate();
        Frame.GoBack();
    }


    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.CurrentOrder.SaveAsync();
    }


    private void CancelEdit_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.CurrentOrder.CancelEdit();
    }


    /// <summary>
    /// Check whether there are unsaved changes and warn the user.
    /// </summary>
    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e){  /* Not done yet */ }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is not OrderWrapper model) return;
        ViewModel.CurrentOrder = model;
        ViewModel.CurrentOrder.Backup();

        if (ViewModel.CurrentOrder.IsInEdit)
        {
            await ViewModel.LoadProducts();
            await ViewModel.CurrentOrder.LoadAvailableHospitals();
        }


    }

    private async void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            await App.GetService<IRepositoryControllerService>().Products.DeleteAsync(ViewModel.CurrentOrder.Id);
            Frame.GoBack();
            WeakReferenceMessenger.Default.Send(new DeleteRecordMessage<OrderWrapper>(ViewModel.CurrentOrder));
        }
        catch (Exception)
        {
            Notification.Content = "Error";
            Notification.Style = NotificationHelper.ErrorStyle;
            Notification.Show(1500);
        }
    }


    private void AddButton_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel.CurrentOrder.IsInEdit)
        {
            ViewModel.CurrentOrder.IsInEdit = false;
        }
        Frame.Navigate(typeof(OrderDetailsPage));
        Frame.BackStack.Remove(Frame.BackStack.Last());
    }

    


    /// <summary>
    /// Handle user's intention to add a product from market's list
    /// </summary>
    private async void MedicineMarketGrid_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        DataGridRow? clickedRow = XamlHelpres.FindParent<DataGridRow>((UIElement)e.OriginalSource);

        if (clickedRow == null || clickedRow?.DataContext is not Product selectedProduct) { return; }
        
        OrderItemDialog content = new OrderItemDialog(selectedProduct.Quantity);

        ContentDialog dialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Select quantity",
            PrimaryButtonText = "Add",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            Content = content
        };

        // TODO replace with nice function
        ContentDialogResult result = await dialog.ShowAsync();
        if (result != ContentDialogResult.Primary)
        {
            return;
        }
        
        ViewModel.InsertOrderItem(selectedProduct, content.ViewModel.Current);
        InitializeComponent();
    }


    /// <summary>
    /// Handle user's intention to change or delete order SelectedOrderItem
    /// </summary>
    private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (OrderList.SelectedItem == null) return;
        if (OrderList.SelectedItem is not OrderItem selectedOrderItem) return;
        OrderList.SelectedItem = null;

        OrderItemDialog content = new OrderItemDialog(
            selectedOrderItem.Product.Quantity + selectedOrderItem.Quantity, 
            selectedOrderItem.Quantity
        );
        
        // TODO replace with nice function
        ContentDialog dialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Select quantity",
            PrimaryButtonText = "Save",
            SecondaryButtonText = "Delete",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            Content = content
        };

        ContentDialogResult userDecision = await dialog.ShowAsync();

        switch (userDecision)
        {
            case ContentDialogResult.Primary:
                await ViewModel.UpdateOrderItem(selectedOrderItem, content.Difference);
                break;
            case ContentDialogResult.Secondary:
                await ViewModel.RemoveOrderItem(selectedOrderItem);
                break;
        }
    }

    
    private async void BeginEdit_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadProducts();
        await ViewModel.CurrentOrder.LoadAvailableHospitals();

        HospitalCustomerComboBox.SelectedItem = ViewModel.CurrentOrder.OrderHospital;
        ViewModel.CurrentOrder.BeginEdit();
    }

    
    private async void HospitalCustomerComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox comboBox) return;
        if (comboBox.SelectedItem is not Hospital selectedHospital) return;
        await ViewModel.CurrentOrder.LoadAvailableShippingAddresses(selectedHospital.Id);
        ShippingAddressComboBox.SelectedItem = ViewModel.CurrentOrder.SelectedAddress;
    }

}
 