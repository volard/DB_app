using DB_app.Behaviors;
using DB_app.Entities;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
using WinUIEx.Messaging;

namespace DB_app.Views;

public sealed partial class ProductDetailsPage : Page
{
    public ProductDetailsViewModel ViewModel { get; }

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

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SaveAsync();
        ViewModel.NotifyGridAboutChange();

        Frame.Navigate(typeof(ProductsGridPage), null);
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
    }


    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel.CurrentProduct.Backup();
        base.OnNavigatedTo(e);
    }


    private void PriceValue_ValueChanged(object? sender, NumberBoxValueChangedEventArgs e)
    {
        //ViewModel.CurrentProduct.Price = Price.Value is double.NaN ? 1 : Price.Value;

        //ViewModel.CurrentProduct.IsModified = true;
    }

    private void QuantityValue_ValueChanged(object? sender, NumberBoxValueChangedEventArgs e)
    {
        //ViewModel.CurrentProduct.Quantity = Quantity.Value is double.NaN ? 1 : (int)Quantity.Value;
        //ViewModel.CurrentProduct.IsModified = true;
    }

}
