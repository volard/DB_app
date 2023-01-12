using DB_app.Behaviors;
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
public sealed partial class PharmacyDetailsPage : Page
{
    public PharmacyDetailsViewModel ViewModel { get; }

    public PharmacyDetailsPage()
    {
        ViewModel = App.GetService<PharmacyDetailsViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });

    }

    public void AddSelectedButton_Clicked(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedAddress != null)
        {
            ViewModel.CurrentPharmacy.PharmacyData.Addresses.Add(ViewModel.SelectedAddress);
            ViewModel.CurrentPharmacy.IsModified = true;
            ViewModel.CurrentPharmacy.NotifyAboutAddressesChanged();
            ViewModel.AvailableAddresses.Remove(ViewModel.SelectedAddress);
        }
    }

    public void DeleteSelectedButton_Clicked(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedExistingAddress != null)
        {
            ViewModel.AvailableAddresses.Add(ViewModel.SelectedExistingAddress);
            ViewModel.CurrentPharmacy.PharmacyData.Addresses.Remove(ViewModel.SelectedExistingAddress);
            ViewModel.CurrentPharmacy.IsModified = true;
            ViewModel.CurrentPharmacy.NotifyAboutAddressesChanged();
        }
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SaveAsync();
        ViewModel.NotifyGridAboutChange();
        Debug.WriteLine($"So boiii the ViewModel.CurrentPharmacy now is {ViewModel.CurrentPharmacy}");

        Frame.Navigate(typeof(PharmaciesGridPage), null);
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
        ViewModel.CurrentPharmacy.BuckupData();
        ViewModel.CurrentPharmacy.NotifyAboutProperties();
        base.OnNavigatedTo(e);
    }

    private void NameText_TextChanged(object sender, TextChangedEventArgs e) =>
        ViewModel.CurrentPharmacy.Name = Name.Text;

    private void INNText_TextChanged(object sender, TextChangedEventArgs e) =>
        ViewModel.CurrentPharmacy.INN = INN.Text;

    private void OGRNText_TextChanged(object sender, TextChangedEventArgs e) =>
        ViewModel.CurrentPharmacy.OGRN = OGRN.Text;
}
