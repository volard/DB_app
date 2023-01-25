using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;

namespace DB_app.Views;

// TODO change "New Address" page header to dynamic one
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

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        bool result = await ViewModel.CurrentAddress.SaveAsync();
        if (result)
        {
            Frame.Navigate(typeof(AddressesGridPage), null);
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

    private void Text_TextChanged(object sender, TextChangedEventArgs e)
    => ViewModel.CurrentAddress.IsModified = true;


}
