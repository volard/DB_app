using CommunityToolkit.WinUI.UI.Controls;
using DB_app.Behaviors;
using DB_app.Helpers;
using DB_app.Models;
using DB_app.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
namespace DB_app.Views;

public sealed partial class MedicineInHospitalReportPage
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

    //Handle the LoadingRowGroup event to alter the grouped header property value to be displayed
    private void dg_loadingRowGroup(object sender, DataGridRowGroupHeaderEventArgs e)
    {
        ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
        Medicine item = (group.GroupItems[0] as Medicine)!;
        e.RowGroupHeader.PropertyValue = item!.Type + " [ " + ViewModel.QuantityPerType[item.Type].ToString() + " ] ";
    }

    private async void HospitalComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        await ViewModel.LoadSource(ViewModel.SelectedHospital);
        SourceDataGrid.ItemsSource = ViewModel.GroupedItemsViewSource.View;

        if (ViewModel.GroupedOrders.Count == 0)
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
        Dictionary<string, List<string>> collection = ViewModel.GroupedOrders.ToDictionary(item => item.Key.ToString()!, item => item.Select(medicine => medicine.Name).ToList()); ;
        await ExcelExtensions.ExportAsExcel(SourceDataGrid, collection, fileName: "MedicineReport");
        NotificationHelper.ShowNotificationMessage(Notification, "File saved successfully", NotificationHelper.SuccessStyle);
    }
}
