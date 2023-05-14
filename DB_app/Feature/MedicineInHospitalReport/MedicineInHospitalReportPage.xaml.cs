using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace DB_app.Views;

public sealed partial class MedicineInHospitalReportPage : Microsoft.UI.Xaml.Controls.Page
{
    public MedicineInHospitalReportViewModel ViewModel { get; } = App.GetService<MedicineInHospitalReportViewModel>();

    public MedicineInHospitalReportPage()
    {
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    private void HospitalComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.LoadSource(ViewModel.SelectedHospital);

    }
}
