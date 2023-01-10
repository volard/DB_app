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

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.SaveAsync();
        ViewModel.NotifyGridAboutChange();
        Debug.WriteLine($"So boiii the ViewModel.CurrentHospital now is {ViewModel.CurrentHospital}");

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
        ViewModel.CurrentHospital.BuckupData();
        ViewModel.CurrentHospital.NotifyAboutProperties();
        base.OnNavigatedTo(e);
    }

    private void Name_main_doctorText_TextChanged(object sender, TextChangedEventArgs e) =>
        ViewModel.CurrentHospital.Name_main_doctor = Name_main_doctor.Text;

    private void Middlename_main_doctorText_TextChanged(object sender, TextChangedEventArgs e) =>
        ViewModel.CurrentHospital.Middlename_main_doctor = Middlename_main_doctor.Text;

    private void Surename_main_doctorText_TextChanged(object sender, TextChangedEventArgs e) =>
        ViewModel.CurrentHospital.Surename_main_doctor = Surename_main_doctor.Text;

    private void INNText_TextChanged(object sender, TextChangedEventArgs e) =>
        ViewModel.CurrentHospital.INN = INN.Text;
    

    private void OGRNText_TextChanged(object sender, TextChangedEventArgs e) =>
        ViewModel.CurrentHospital.OGRN = OGRN.Text;
}
