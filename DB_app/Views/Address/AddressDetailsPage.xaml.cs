using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;

namespace DB_app.Views;


public sealed partial class AddressDetailsPage : Page
{
    public AddressDetailsViewModel ViewModel { get; }

    public AddressDetailsPage()
    {
        ViewModel = App.GetService<AddressDetailsViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }


    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(AddressesGridPage), ViewModel.CurrentAddress);
    }


    /// <summary>
    /// Navigate to the previous page when the user cancels the creation of a new record.
    /// </summary>
    private void AddNewMedicineCanceled(object sender, RoutedEventArgs e)
        => Frame.GoBack();

    /// <summary>
    /// Check whether there are unsaved changes and warn the user.
    /// </summary>
    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        // TODO implement this
    }

    /// <summary>
    /// Loads selected AddressWrapper object or creates a new order.
    /// </summary>
    /// <param newName="e">Info about the event.</param>
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var selectedAddress = (AddressWrapper)e.Parameter;
        if (selectedAddress != null)
        {
            ViewModel.CurrentAddress = selectedAddress;
        }

        base.OnNavigatedTo(e);
    }

    private void CityText_TextChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.City = City.Text;
    }

    private void StreetText_TextChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.Street = Street.Text;
    }

    private void BuildingText_TextChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.Building = Building.Text;
    }
}
