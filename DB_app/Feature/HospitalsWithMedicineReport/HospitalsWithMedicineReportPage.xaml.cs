using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace DB_app.Views;

public sealed partial class HospitalsWithMedicineReportPage : Page
{
    public HospitalsWithMedicineReportViewModel ViewModel { get; } = App.GetService<HospitalsWithMedicineReportViewModel>();

    public HospitalsWithMedicineReportPage()
    {
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    private async void HospitalComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
         await ViewModel.LoadSource();

    }
}
