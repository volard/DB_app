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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DB_app.Views;


public sealed partial class OrderDetailsPage : Page
{

    public OrderDetailsViewModel ViewModel { get; } = App.GetService<OrderDetailsViewModel>();


    private INotifyDataErrorInfo? OldDataContext { get; set; }

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

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if(ViewModel.CurrentOrder.IsInEdit)
            ViewModel.LoadProducts();

        base.OnNavigatedTo(e);
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
    /// Refreshes the errors currently displayed.
    /// </summary>
    private void RefreshErrors(string paramName)
    {
        ValidationResult? result = ViewModel.CurrentOrder.GetErrors(paramName).OfType<ValidationResult>().FirstOrDefault();
        FontIcon? icon = StackPanel.FindName(paramName + "Icon") as FontIcon;
        if (icon == null) { return; }
        icon!.Visibility = result is not null ? Visibility.Visible : Visibility.Collapsed;
        if (result is not null)
        {
            ToolTipService.SetToolTip(icon, result.ErrorMessage);
        }
    }


    /// <summary>
    /// Updates the bindings whenever the data context changes.
    /// </summary>
    private void Element_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
    {
        if (OldDataContext is not null)
        {
            OldDataContext.ErrorsChanged -= DataContext_ErrorsChanged;
        }

        if (args.NewValue is not INotifyDataErrorInfo dataContext) return;
        
        OldDataContext = dataContext;
        OldDataContext.ErrorsChanged += DataContext_ErrorsChanged;
    }


    /// <summary>
    /// Invokes <see cref="RefreshErrors"/> whenever the data context requires it.
    /// </summary>
    /// <param name="sender"><see cref="ProductWrapper"/> object</param>
    /// <param name="e"></param>    
    private void DataContext_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        if (e.PropertyName != null)
            RefreshErrors(e.PropertyName);
    }


    /// <summary>
    /// Refreshes errors on combobox selection changes
    /// </summary>
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshErrors(((ComboBox)sender).Name);
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

    
    private void BeginEdit_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.LoadAvailableHospitals();
        ViewModel.CurrentOrder.BeginEdit();
    }

    
    private void HospitalCustomerComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox comboBox) return;
        if (comboBox.SelectedItem is not Hospital selectedHospital) return;
        ViewModel.LoadAvailableShippingAddresses(selectedHospital.Id);
    }

}
