using DB_app.Behaviors;
using DB_app.Models;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;

namespace DB_app.Views;


public sealed partial class PharmacyDetailsPage : Page
{
    public PharmacyDetailsViewModel ViewModel { get; } = App.GetService<PharmacyDetailsViewModel>();


    /// <summary>
    /// Initializes the page.
    /// </summary>
    public PharmacyDetailsPage()
    {
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    private async void MakeInactiveButton_ButtonClicked(object sender, RoutedEventArgs e)
    {
        ContentDialog dialog = new()
        {
            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            XamlRoot = this.XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Are you sure?",
            PrimaryButtonText = "Confirm",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            Content = "When you disable pharmacy, it will be unlinked from its addresses and become read only."
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            
        }
    }


    public void AddButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedAddress == null) return;
        
        ViewModel.CurrentPharmacy.ObservableLocations.Add(new PharmacyLocation(ViewModel.SelectedAddress));
        ViewModel.AvailableAddresses.Remove(ViewModel.SelectedAddress);
        
    }

    public void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedExistingLocation == null) return;
        
        ViewModel.AvailableAddresses.Add(ViewModel.SelectedExistingLocation.Address);
        ViewModel.CurrentPharmacy.ObservableLocations.Remove(ViewModel.SelectedExistingLocation);
        ViewModel.CurrentPharmacy.PharmacyData.Locations.Remove(ViewModel.SelectedExistingLocation);

        ViewModel.SelectedExistingLocation = null;
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.CurrentPharmacy.SaveAsync();
    }


    /// <summary>
    /// Check whether there are unsaved changes and warn the user.
    /// </summary>
    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) { /* not used */ }

    protected override void OnNavigatedTo(NavigationEventArgs e) { /* not used */ }

    private void NameText_TextChanged(object sender, TextChangedEventArgs e) =>
        ViewModel.CurrentPharmacy.Name = Name.Text;

    private void BeginEdit_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.LoadAvailableAddresses();
        ViewModel.CurrentPharmacy.BeginEdit();
    }
}
