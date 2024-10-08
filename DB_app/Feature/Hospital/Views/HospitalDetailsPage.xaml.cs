using DB_app.Behaviors;
using DB_app.Models;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;

namespace DB_app.Views;

// Reduces warning noise on parameters that are needed for signature requirements
#pragma warning disable IDE0060


public sealed partial class HospitalDetailsPage : Page
{
    public HospitalDetailsViewModel ViewModel { get; } = App.GetService<HospitalDetailsViewModel>();

    /// <summary>
    /// Initializes the page.
    /// </summary>
    public HospitalDetailsPage()
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
        ContentDialog dialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Are you sure?",
            PrimaryButtonText = "Confirm",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            Content = "When you disable hospital, it will be unlinked from its addresses and become read only."
        };

        ContentDialogResult result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
        }
    }


    private void AddSelectedButton_Clicked(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedAddress == null) return;

        ViewModel.CurrentHospital.ObservableLocations.Add(new HospitalLocation(ViewModel.SelectedAddress));
        ViewModel.AvailableAddresses.Remove(ViewModel.SelectedAddress);
    }


    private void DeleteSelectedButton_Clicked(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedExistingLocation == null) return;

        ViewModel.AvailableAddresses.Add(ViewModel.SelectedExistingLocation.Address);

        ViewModel.CurrentHospital.ObservableLocations.Remove(ViewModel.SelectedExistingLocation);
        ViewModel.CurrentHospital.HospitalData.Locations.Remove(ViewModel.SelectedExistingLocation);
        ViewModel.SelectedExistingLocation = null;
    }


    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.CurrentHospital.SaveAsync();
    }


    /// <summary>
    /// Check whether there are unsaved changes and warn the user.
    /// </summary>
    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) { /* not used */ }

    protected override void OnNavigatedTo(NavigationEventArgs e){ /* not used */ }

    private void BeginEdit_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.LoadAvailableAddresses();
        ViewModel.CurrentHospital.BeginEdit();
    }
}

#pragma warning restore IDE0060