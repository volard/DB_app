using DB_app.Behaviors;
using DB_app.Models;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.ApplicationModel.Resources;

namespace DB_app.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class HospitalDetailsPage : Page
{
    public HospitalDetailsViewModel ViewModel { get; } = App.GetService<HospitalDetailsViewModel>();

    public HospitalDetailsPage()
    {
        NavigationViewHeaderBehavior.SetHeaderMode(this, NavigationViewHeaderMode.Never);
        InitializeComponent();
    }

    private async void MakeInactiveButton_ButtonClicked(object sender, RoutedEventArgs e)
    {
        ContentDialog dialog = new()
        {
            XamlRoot = this.XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Are you sure?",
            PrimaryButtonText = "Confirm",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            Content = "When you disable hospital, it will be unlinked from its addresses and become read only."
        };

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
        }
    }
    
    public void AddSelectedButton_Clicked(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedAddress == null) return;

        ViewModel.CurrentHospital.ObservableLocations.Add(new HospitalLocation(ViewModel.SelectedAddress));
        ViewModel.CurrentHospital.IsModified = true;
        ViewModel.AvailableAddresses.Remove(ViewModel.SelectedAddress);
    }

    public void DeleteSelectedButton_Clicked(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedExistingLocation == null) return;

        ViewModel.AvailableAddresses.Add(ViewModel.SelectedExistingLocation.Address);
        ViewModel.CurrentHospital.ObservableLocations.Remove(ViewModel.SelectedExistingLocation);
        ViewModel.CurrentHospital.HospitalData.Locations.Remove(ViewModel.SelectedExistingLocation);
        ViewModel.CurrentHospital.IsModified = true;
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.CurrentHospital.SaveAsync();
        //App.GetService<INavigationService>().NavigateTo(typeof(AddressDetailsViewModel).FullName!, ViewModel.SelectedItem);
        //Frame.Navigate(typeof(HospitalsGridPage), new DrillInNavigationTransitionInfo());
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


    protected override void OnNavigatedTo(NavigationEventArgs e){ /* not used */ }
}
