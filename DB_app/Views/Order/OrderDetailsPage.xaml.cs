using AppUIBasics.Helper;
using CommunityToolkit.Mvvm.Messaging;
using DB_app.Behaviors;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using DB_app.Services.Messages;
using DB_app.ViewModels;
using DB_app.Views.Components;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DB_app.Views;

public sealed partial class OrderDetailsPage : Page
{
    public OrderDetailsViewModel ViewModel { get; } = App.GetService<OrderDetailsViewModel>();

    public INotifyDataErrorInfo? oldDataContext { get; set; }


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

    /// <summary>
    /// Create a new Window once the Tab is dragged outside.
    /// </summary>
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
        bool isOk = await ViewModel.CurrentOrder.SaveAsync();
    }

    private void CancelEdit_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.CurrentOrder.CancelEdit();
    }

    /// <summary>
    /// Check whether there are unsaved changes and warn the user.
    /// </summary>
    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        // Not done yet
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
            var message = "жоская ошебка";
            ////Notification.Content = message;
            //Notification.Show(2000);
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
        var icon = StackPanel.FindName(paramName + "Icon") as FontIcon;
        if (icon != null)
        {
            icon!.Visibility = result is not null ? Visibility.Visible : Visibility.Collapsed;
            if (result is not null)
            {
                ToolTipService.SetToolTip(icon, result.ErrorMessage);
            }
        } 
    }

    /// <summary>
    /// Updates the bindings whenever the data context changes.
    /// </summary>
    private void Element_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
    {
        if (oldDataContext is not null)
        {
            oldDataContext.ErrorsChanged -= DataContext_ErrorsChanged;
        }

        if (args.NewValue is INotifyDataErrorInfo dataContext)
        {
            oldDataContext = dataContext;

            oldDataContext.ErrorsChanged += DataContext_ErrorsChanged;
        }
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

    private async void MedicineMarketGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = MedicineMarketGrid.SelectedItem as Product;

        if (selected == null) { return; }
        var content = new ContentDialogContent(selected.Quantity);

        ContentDialog dialog = new()
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
        if (result == ContentDialogResult.Primary)
        {
            ViewModel.CurrentOrder.AddProduct(selected, content.ViewModel.Current);
        }
    }

    private void MedicineMarketGrid_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {

    }
}
