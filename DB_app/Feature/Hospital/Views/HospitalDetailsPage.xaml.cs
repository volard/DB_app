using DB_app.Behaviors;
using DB_app.Models;
using DB_app.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;

namespace DB_app.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class HospitalDetailsPage : Page
{
    public HospitalDetailsViewModel ViewModel { get; }

    public HospitalDetailsPage()
    {
        ViewModel = App.GetService<HospitalDetailsViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });

    }

    private async void MakeInactiveButton_ButtonClicked(object sender, RoutedEventArgs e)
    {
        ContentDialog dialog = new();

        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        dialog.XamlRoot = this.XamlRoot;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "Are you sure?";
        dialog.PrimaryButtonText = "Confirm";
        dialog.CloseButtonText = "Cancel";
        dialog.DefaultButton = ContentDialogButton.Primary;
        dialog.Content = "When you disable hospital, it will be unlinked from its addresses and become read only.";

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
        //await ViewModel._repositoryControllerService.Hospitals.UpdateAsync(ViewModel.CurrentHospital.HospitalData);
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
