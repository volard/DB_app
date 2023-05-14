using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Models;
using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class MedicineInHospitalReportViewModel : ObservableRecipient, INavigationAware
{

    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    public MedicineInHospitalReportViewModel()
    {

    }

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<Order> Source { get; set; } = new();
    public ObservableCollection<Hospital> AvailableHospitals { get; set; } = new();



    [ObservableProperty]
    private Hospital _selectedHospital;

    /// <summary>
    /// Gets or sets a value that indicates whether to show a progress bar. 
    /// </summary>
    [ObservableProperty]
    private bool _isHospitalsLoading = false;

    [ObservableProperty]
    private bool _isSourceLoading = false;

    public async void OnNavigatedTo(object parameter)
    {
        Source.CollectionChanged += Source_CollectionChanged;
        await LoadAvailableHospitals();
        SelectedHospital = AvailableHospitals[1];
        LoadSource(SelectedHospital);

    }

    private void Source_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Source));
    }

    /// <summary>
    /// Retrieves items from the data source.
    /// </summary>
    public async void LoadSource(Hospital hospital)
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsSourceLoading = true;
            Source.Clear();
        });

        var orders = await _repositoryControllerService.Orders.GetHospitalOrders(hospital.Id);

        List<string> types = new();

        foreach (var order in orders)
        {
            foreach(var item in order.Items)
            {
                if(!types.Contains(item.Product.Medicine.Type))
                {
                    types.Add(item.Product.Medicine.Type);
                }

            }
        }

        List<double> sumsPertype = new();

        foreach (var type in types)
        {
            var money = orders.Select(order => order.Items).Select(items => items.Select(item =>
            {
                if (item.Product.Medicine.Type == type) return item.Price;
                return 0;
            }
            )).Select(item => item.First());

            sumsPertype.Add(money.Sum(money => money));
        }

        Debug.WriteLine("asht");

        //await _dispatcherQueue.EnqueueAsync(() =>
        //{
        //    foreach (var item in items)
        //    {
        //        Source.Add(item);
        //    }

        //    IsSourceLoading = false;
        //});
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



    /// <summary>
    /// Retrieves items from the data source.
    /// </summary>
    public async Task LoadAvailableHospitals()
    {
        await _dispatcherQueue.EnqueueAsync(() =>
        {
            IsHospitalsLoading = true;
            AvailableHospitals.Clear();
        });

        var items = await Task.Run(_repositoryControllerService.Hospitals.GetAsync);

        await _dispatcherQueue.EnqueueAsync(() =>
        {
            foreach (var item in items)
            {
                AvailableHospitals.Add(item);
            }

            IsHospitalsLoading = false;
        });
    }


    public void OnNavigatedFrom()
    {
        Source.CollectionChanged -= Source_CollectionChanged;
    }
}
