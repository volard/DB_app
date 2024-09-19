using DB_app.Behaviors;
using DB_app.Helpers;
using DB_app.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;


namespace DB_app.Views;

public sealed partial class PharmacyWithMedicineReportPage : Page
{
    public PharmacyWithMedicineReportViewModel ViewModel { get; } = App.GetService<PharmacyWithMedicineReportViewModel>();

    public PharmacyWithMedicineReportPage()
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


    private async void MedicinesComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel.SelectedMedicine == null) return;
        await ViewModel.LoadSource();
        if (ViewModel.Source.Count == 0)
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

    private void CommandBarExportButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        NotificationHelper.ShowNotificationMessage(Notification, "gut", NotificationHelper.SuccessStyle);

    }
}
