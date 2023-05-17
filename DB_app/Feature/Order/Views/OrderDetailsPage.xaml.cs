using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.UI.Controls;
using DB_app.Behaviors;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using DB_app.Services.Messages;
using DB_app.ViewModels;
using DB_app.Views.Components;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
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
    ///  Finds the DataGridRow that was clicked I traverse the visual tree
    ///  Thanks to https://stackoverflow.com/questions/70429745/how-to-know-when-a-datagridrow-is-clicked
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="childElement"></param>
    /// <returns></returns>
    private static T? FindParent<T>(DependencyObject childElement) where T : Control
    {
        DependencyObject currentElement = childElement;

        while (currentElement != null)
        {
            if (currentElement is T matchingElement)
            {
                return matchingElement;
            }

            currentElement = VisualTreeHelper.GetParent(currentElement);
        }

        return null;
    }


    /// <summary>
    /// Handle user's intention to add a product from market's list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void MedicineMarketGrid_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        DataGridRow? clickedRow = FindParent<DataGridRow>((UIElement)e.OriginalSource);

        if (clickedRow == null || clickedRow?.DataContext is not Product selectedProduct) { return; }


        ContentDialogContent content = new ContentDialogContent(selectedProduct.Quantity);

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

        ContentDialogResult result = await dialog.ShowAsync();
        if (result != ContentDialogResult.Primary)
        {
            return;
        }
        
        ViewModel.CurrentOrder.AddOrderItem(selectedProduct, content.ViewModel.Current);
        InitializeComponent();
    }


    /// <summary>
    /// Handle user's intention to change or delete order SelectedOrderItem
    /// </summary>
    private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (OrderList.SelectedItem == null) return;
        if (OrderList.SelectedItem is not OrderItem SelectedOrderItem) return;
        OrderList.SelectedItem = null;

        var content = new ContentDialogContent(
            SelectedOrderItem.Product.Quantity + SelectedOrderItem.Quantity, 
            SelectedOrderItem.Quantity
        );

        ContentDialog dialog = new()
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

        ContentDialogResult UserDecision = await dialog.ShowAsync();

        if (UserDecision == ContentDialogResult.Primary)
        {
            int new_value = content.ViewModel.Current;
            await ViewModel.CurrentOrder.UpdateOrderItem(SelectedOrderItem, new_value);
        }
        else if (UserDecision == ContentDialogResult.Secondary)
        {
            await ViewModel.CurrentOrder.RemoveOrderItem(SelectedOrderItem);
        }
    }

    
    private void BeginEdit_Click(object sender, RoutedEventArgs e)
    {
        _ = ViewModel.LoadAvailableHospitalsAsync();
        ViewModel.CurrentOrder.BeginEdit();
    }

    
    private void HospitalCustomerComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox comboBox) return;
        if (comboBox.SelectedItem is not Hospital selectedHospital) return;
        _ = ViewModel.LoadAvailableShippingAddressesAsync(selectedHospital);
    }

}
