using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;
using DB_app.Contracts.ViewModels;
using DB_app.Core.Contracts.Services;
using DB_app.Helpers;
using DB_app.Models;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Data;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DB_app.ViewModels;

public partial class MedicineInHospitalReportViewModel : ObservableRecipient, INavigationAware
{

    public MedicineInHospitalReportViewModel()
    {
        var some = await _repositoryControllerService.Hospitals.GetHospitalsOrderItems();
    }

    private readonly IRepositoryControllerService _repositoryControllerService = App.GetService<IRepositoryControllerService>();

    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

 

    /// <summary>
    /// DataGrid's data collection
    /// </summary>
    public ObservableCollection<Order> Source { get; init; } = new ObservableCollection<Order>();
    public ObservableCollection<Hospital> AvailableHospitals { get; } = new ObservableCollection<Hospital>();



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
        CollectionsHelper.LoadCollectionAsync(AvailableHospitals, _dispatcherQueue, _repositoryControllerService.Hospitals.GetAsync);
        SelectedHospital = AvailableHospitals[1];
        LoadSource(SelectedHospital);

    }

    private void Source_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Source));
    }

    // Create grouping for collection
    ObservableCollection<GroupInfoCollection<Order>> orders = new ObservableCollection<GroupInfoCollection<Order>>();

    

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

        

        IEnumerable<Order>? orders = await _repositoryControllerService.Orders.GetHospitalOrders(hospital.Id);

        List<string> types = new List<string>();

        IEnumerable<Order> enumerable = orders.ToList();
        foreach (Order? order in enumerable)
        {
            foreach(OrderItem item in order.Items)
            {
                if(!types.Contains(item.Product.Medicine.Type))
                {
                    types.Add(item.Product.Medicine.Type);
                }

            }
        }

        List<double> sumsPertype = new List<double>();

        foreach (string type in types)
        {
            IEnumerable<double> money = enumerable.Select(order => order.Items).Select(items => items.Select(item =>
            {
                if (item.Product.Medicine.Type == type) return item.Price;
                return 0;
            }
            )).Select(item => item.First());

            sumsPertype.Add(money.Sum(money => money));
        }

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
    //    ObservableCollection<GroupInfoCollection<Order>> groups = new ObservableCollection<GroupInfoCollection<Order>>();
    //    var query = from item in _items
    //                orderby item
    //                group item by item.Type into g
    //                select new { GroupName = g.Key, Items = g };
        
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



    public void OnNavigatedFrom()
    {
        Source.CollectionChanged -= Source_CollectionChanged;
    }
}
