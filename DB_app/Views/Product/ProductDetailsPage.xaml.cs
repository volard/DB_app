using CommunityToolkit.Mvvm.Messaging;
using DB_app.Behaviors;
using DB_app.Core.Contracts.Services;
using DB_app.Entities;
using DB_app.Services.Messages;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using WinUIEx.Messaging;

namespace DB_app.Views;

public sealed partial class ProductDetailsPage : Page
{
    public ProductDetailsViewModel ViewModel { get; }
    public INotifyDataErrorInfo oldDataContext { get; set; }

    public ProductDetailsPage()
    {
        ViewModel = App.GetService<ProductDetailsViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }


    #region Button handlers

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        bool isOk = await ViewModel.CurrentProduct.SaveAsync();
    }


    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel.CurrentProduct.CancelEdit();
        ViewModel.CurrentProduct.IsInEdit = false;
    }


    private async void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            await App.GetService<IRepositoryControllerService>().Products.DeleteAsync(ViewModel.CurrentProduct.Id);
            Frame.GoBack();
            WeakReferenceMessenger.Default.Send(new DeleteRecordMessage<ProductWrapper>(ViewModel.CurrentProduct));
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
        if (ViewModel.CurrentProduct.IsInEdit)
        {
            ViewModel.CurrentProduct.IsInEdit = false;
        }
        Frame.Navigate(typeof(ProductDetailsPage));
        Frame.BackStack.Remove(Frame.BackStack.Last());
    }

    #endregion


    




    /// <summary>
    /// Refreshes the errors currently displayed.
    /// </summary>
    private void RefreshErrors(string paramName)
    {
        ValidationResult? result = ViewModel.CurrentProduct.GetErrors(paramName).OfType<ValidationResult>().FirstOrDefault();
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
    private void DataContext_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        // sender here is ProductWrapper
        if (e.PropertyName != null)
            RefreshErrors(e.PropertyName);
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshErrors(((ComboBox)sender).Name);
    }


    private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        RefreshErrors(sender.Name);
    }

}
