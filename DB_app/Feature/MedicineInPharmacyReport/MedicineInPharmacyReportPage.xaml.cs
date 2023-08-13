using CommunityToolkit.WinUI.UI.Controls;
using DB_app.Behaviors;
using DB_app.Models;
using DB_app.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System.Collections.ObjectModel;

namespace DB_app.Views;

public sealed partial class MedicineInPharmacyReportPage : Page
{
    public MedicineInPharmacyReportViewModel ViewModel { get; } = App.GetService<MedicineInPharmacyReportViewModel>();

    public MedicineInPharmacyReportPage()
    {
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    private void PharmacyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.LoadSource(ViewModel.SelectedPharmacy);
        SourceDataGrid.Columns[0].SortDirection = DataGridSortDirection.Ascending;
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

    private void dg_Sorting(object sender, DataGridColumnEventArgs e)
    {
        if (e.Column.Tag.ToString() == "Name")
        {
            List<Product> _items = new(ViewModel.Source);
            ViewModel.Source.Clear();

            if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
            {
                foreach (var line in new ObservableCollection<Product>( from item in _items
                                                                                orderby item.Medicine.Name ascending
                                                                                select item
                                                                               ))
                {
                    ViewModel.Source.Add(line);
                }
                e.Column.SortDirection = DataGridSortDirection.Ascending;
            }
            else
            {
                foreach (var line in new ObservableCollection<Product>(from item in _items
                                                                       orderby item.Medicine.Name descending
                                                                       select item
                                                                               ))
                {
                    ViewModel.Source.Add(line);
                }
                e.Column.SortDirection = DataGridSortDirection.Descending;
            }

            // Remove sorting indicators from other columns
            foreach (var SourceDataGridColumn in SourceDataGrid.Columns)
            {
                if (SourceDataGridColumn.Tag.ToString() != e.Column.Tag.ToString())
                {
                    SourceDataGridColumn.SortDirection = null;
                }
            }
        }
    }
}
