using CommunityToolkit.Mvvm.ComponentModel;
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

// Reduces warning noise on parameters that are needed for signature requirements
#pragma warning disable IDE0060

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
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

        Surename_main_doctor.CustomTextChanged += new TextChangedEventHandler(Field_FieldChanged<TextChangedEventArgs>);
        Name_main_doctor.CustomTextChanged += new TextChangedEventHandler(Field_FieldChanged<TextChangedEventArgs>);
        Middlename_main_doctor.CustomTextChanged += new TextChangedEventHandler(Field_FieldChanged<TextChangedEventArgs>);
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
        //ViewModel.CurrentHospital.IsModified = true;
        ViewModel.AvailableAddresses.Remove(ViewModel.SelectedAddress);
    }

    public void DeleteSelectedButton_Clicked(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedExistingLocation == null) return;

        ViewModel.AvailableAddresses.Add(ViewModel.SelectedExistingLocation.Address);

        ViewModel.CurrentHospital.ObservableLocations.Remove(ViewModel.SelectedExistingLocation);
        ViewModel.CurrentHospital.HospitalData.Locations.Remove(ViewModel.SelectedExistingLocation);
        ViewModel.SelectedExistingLocation = null;
        //ViewModel.CurrentHospital.IsModified = true;
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
    private void CancelEdit_Click(object sender, RoutedEventArgs e)
    {
        Frame.GoBack();
    }

    /// <summary>
    /// Check whether there are unsaved changes and warn the user.
    /// </summary>
    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
    }

    private void Field_FieldChanged<T>(object sender, T e)
    {
        //if (ViewModel.CurrentHospital.IsInEdit)
            //ViewModel.CurrentHospital.IsModified = true;
    }

    private void Text_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (ViewModel.CurrentHospital.IsInEdit)
        {
            //ViewModel.CurrentHospital.IsModified = true;
        }
    }

    protected override void OnNavigatedTo(NavigationEventArgs e){ /* not used */ }

    private void BeginEdit_Click(object sender, RoutedEventArgs e)
    {
        _ = ViewModel.LoadAvailableAddressesAsync();
        ViewModel.CurrentHospital.BeginEdit();
    }
}

#pragma warning restore IDE0060