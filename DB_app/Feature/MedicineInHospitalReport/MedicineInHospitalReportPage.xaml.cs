using CommunityToolkit.WinUI.UI.Controls;
using DB_app.Behaviors;
using DB_app.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System.Collections.ObjectModel;

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


    //public CollectionViewSource GroupData(string groupBy = "Range")
    //{
    //    ObservableCollection<GroupInfoCollection<DataGridDataItem>> groups = new ObservableCollection<GroupInfoCollection<DataGridDataItem>>();
    //    var query = from item in _items
    //                orderby item
    //                group item by item.Range into g
    //                select new { GroupName = g.Key, Items = g };
    //    if (groupBy == "Parent_Mountain")
    //    {
    //        query = from item in _items
    //                orderby item
    //                group item by item.Parent_mountain into g
    //                select new { GroupName = g.Key, Items = g };
    //    }
    //    foreach (var g in query)
    //    {
    //        GroupInfoCollection<DataGridDataItem> info = new GroupInfoCollection<DataGridDataItem>();
    //        info.Key = g.GroupName;
    //        foreach (var item in g.Items)
    //        {
    //            info.Add(item);
    //        }

    //        groups.Add(info);
    //    }

    //    groupedItems = new CollectionViewSource();
    //    groupedItems.IsSourceGrouped = true;
    //    groupedItems.Source = groups;

    //    return groupedItems;
    //}

    //public class GroupInfoCollection<T> : ObservableCollection<T>
    //{
    //    public object Key { get; set; }

    //    public new IEnumerator<T> GetEnumerator()
    //    {
    //        return (IEnumerator<T>)base.GetEnumerator();
    //    }
    //}
}
