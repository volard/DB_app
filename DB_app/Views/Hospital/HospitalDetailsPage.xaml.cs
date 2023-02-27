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
        if (ViewModel.SelectedAddress != null)
        {
            ViewModel.CurrentHospital.HospitalData.AddAddress(ViewModel.SelectedAddress);
            ViewModel.CurrentHospital.IsModified = true;
            ViewModel.AvailableAddresses.Remove(ViewModel.SelectedAddress);
        }
    }

    public void DeleteSelectedButton_Clicked(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedExistingAddress != null)
        {
            ViewModel.AvailableAddresses.Add(ViewModel.SelectedExistingAddress);
            ViewModel.CurrentHospital.HospitalData.RemoveAddress(ViewModel.SelectedExistingAddress);
            ViewModel.CurrentHospital.IsModified = true;
        }
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SaveAsync();
        ViewModel.NotifyGridAboutChange();

        Frame.Navigate(typeof(HospitalsGridPage), null);
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
        ViewModel.CurrentHospital.Backup();
        base.OnNavigatedTo(e);
    }

    //private void Name_main_doctorText_TextChanged(object sender, TextChangedEventArgs e) =>
        //ViewModel.CurrentHospital.Name_main_doctor = Name_main_doctor.Text;

    //private void Middlename_main_doctorText_TextChanged(object sender, TextChangedEventArgs e) =>
        //ViewModel.CurrentHospital.Middlename_main_doctor = Middlename_main_doctor.Text;

    //private void Surename_main_doctorText_TextChanged(object sender, TextChangedEventArgs e) =>
        //ViewModel.CurrentHospital.Surename_main_doctor = Surename_main_doctor.Text;
}
