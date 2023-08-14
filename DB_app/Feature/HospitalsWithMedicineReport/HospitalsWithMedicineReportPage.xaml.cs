using DB_app.Behaviors;
using DB_app.Helpers;
using DB_app.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;
using System.Linq;

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

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        await ViewModel.LoadMedicine();
        ViewModel.SelectedMedicine = ViewModel.AvailableMedicines[0];
        base.OnNavigatedTo(e);
    }


    private async void MedicineComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
         await ViewModel.LoadSource();
        if(ViewModel.Source.Count == 0)
        {
            SourceDataGrid.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            NotFoundBlock.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
        }
        else
        {
            SourceDataGrid.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            NotFoundBlock.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
        }
    }

    private async void CommandBarExportButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ExcelExtensions.ExportAsExcel(SourceDataGrid, ViewModel.Source.Select(item =>
        {
            return new List<string>
            {
                item.Hospital.Id.ToString() ?? " ",
                item.Hospital.Surename_main_doctor + " " + item.Hospital.Name_main_doctor + " " + item.Hospital.Middlename_main_doctor ?? " "
            };
        }).ToList(), fileName: "Hospitals With Medicine Report");
        NotificationHelper.ShowNotificationMessage(Notification, "gut", NotificationHelper.SuccessStyle);
    }
}
